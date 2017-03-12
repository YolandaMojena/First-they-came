using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterMovement : MonoBehaviour {

	public AudioSource[] sounds;
	public AudioSource jump;
	public AudioSource footsteps;

    // TO BE MANUALLY ASSIGNED IN EDITOR
    [SerializeField]
    SpriteRenderer sprite;
    [SerializeField]
    Animator animator;

    // CONSTANTS
    [SerializeField]
    float WIDTH = 0.23f, HEIGHT = 1.23f;

    public const float SKIN_WIDTH = 0.015f;
    int verticalRayNum = 3;
    float verticalRaySpacing = 0.11f;
    [SerializeField]
    Vector2 rayMargins = Vector2.zero;
    int horizontalRayNum = 4;
    float horizontalRaySpacing = 0.60f;


    const float JUMP_FORCE = 5f;
    const float JUMP_BOOST = 1.4f;
    const float JUMP_HOLD_TIME = 0.2f;
    const float JUMP_HOLD_MULTIPLYIER = 0.75f;

    const float RUN_ACCEL = 8f;
    const float RUN_MAX_SPEED = 3f;
    const float AIR_RUN_ACCEL = 10f;
    const float AIR_MAX_SPEED = 2.5f;
    const float FRICTION = 15f;

    const float MAX_SLOPE_ANGLE = 45f;
    const float MAX_DESCEND_ANGLE = 45f;
    const float SLOPE_RUN_HANDICAP = 1f / 20f; // 1f/10f


    // ATRIBUTES
    bool isPlayer = false;
    public bool isDead = false;
    // Physics
    Vector2 GRAVITY = new Vector2(0f, -19.81f);
    const float HORIZONTAL_DRAG = 0.5f;
    public Vector2 acceleration = Vector2.zero;
    public Vector2 velocity = Vector2.zero;
    public Vector2 traslation = Vector2.zero;

    public Vector2 externalVelocity = Vector2.zero;

    Vector3 startPos;

    // Aux
    struct Bounds
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    Bounds bounds = new Bounds();

    // Controller
    [SerializeField]
    public bool Grounded = false;
    [SerializeField]
    float slopeAngle = 0f;
    [SerializeField]
    bool climbSlope = false;
    [SerializeField]
    bool descendSlope = false;
    float jumpHoldTime = 0f;
    [SerializeField]
    int run = 0;

    public float LateralBlock = 0;

    // METHODS
    void Start()
    {
        startPos = transform.position;

        CalculateBounds();
        isPlayer = gameObject.layer == LayerMask.NameToLayer("Character");

        if (isPlayer) {
            if (!sprite)
            {
                sprite = gameObject.GetComponentInChildren<SpriteRenderer>();
            }
            if (!animator)
                animator = GetComponentInChildren<Animator>();
            //WIDTH = sprite.sprite.bounds.size.x;
            //HEIGHT = sprite.sprite.bounds.size.y;
        }
        else
        {
            if (!sprite)
            {
                sprite = GetComponent<SpriteRenderer>();
                if (!sprite)
                    sprite = GetComponentInChildren<SpriteRenderer>();
            }

            verticalRaySpacing = (WIDTH-rayMargins.x) / (verticalRayNum - 1);
            horizontalRaySpacing = HEIGHT / (horizontalRayNum - 1);
        }

		sounds = GetComponents<AudioSource>();
		jump = sounds [0];
		footsteps = sounds [1];
    }

    void Update()
    {
        if (isPlayer && !isDead)
            GatherInput();
    }

    void FixedUpdate()
    {
        acceleration = Vector2.zero;

        if (isPlayer)
        {
            
            if (tag == "PlantEntity")
                FlowerSpeed();

            InputToMovement();
        }

        CalculateDynamics();

        descendSlope = false;
        if (velocity.y <= 0)
            DescendSlope();
        if(velocity.x != 0 || externalVelocity.x != 0)
            HorizontalCollisions();
        VerticalCollisions();

        ApplyPositionChange();
        AdjustSprite();

        externalVelocity = Vector2.zero;
    }



    void CalculateBounds()
    {
        //float width = 0.23f;
        //float height = 1.23f;
        bounds.topLeft = new Vector2(-WIDTH / 2f + SKIN_WIDTH, HEIGHT - SKIN_WIDTH);
        bounds.topRight = new Vector2(WIDTH / 2f - SKIN_WIDTH, HEIGHT - SKIN_WIDTH);
        bounds.bottomLeft = new Vector2(-WIDTH / 2f + SKIN_WIDTH, SKIN_WIDTH);
        bounds.bottomRight = new Vector2(WIDTH / 2f - SKIN_WIDTH, SKIN_WIDTH);
    }

    void GatherInput()
    {
        run = 0;
		if (Input.GetKey (KeyCode.D)) {
			run += 1;
		}
		if (Input.GetKey (KeyCode.A)) {
			run += -1;
		}

        if (Input.GetKeyDown(KeyCode.Space) && (Grounded || Physics2D.Raycast(transform.position, Vector3.down, 4 * SKIN_WIDTH - traslation.y, LayerMask.GetMask("Slope", "Wall", "Platform"))))
        {
            //Debug.Log("Jump!");
            velocity.y = JUMP_FORCE;
            velocity.x *= JUMP_BOOST;
            jumpHoldTime = 0f;
            Grounded = false;
			jump.Play (); 
        }

		//FOOTSTEPS SOUND

		if (Input.GetKeyDown (KeyCode.D) || Input.GetKeyDown(KeyCode.A) && Grounded)
			footsteps.Play ();

		if ((Input.GetKeyUp (KeyCode.D) || Input.GetKeyUp (KeyCode.A))) {
			if(Input.GetKey(KeyCode.D) == false &&  (Input.GetKey(KeyCode.A) == false))
				{
					footsteps.Stop();
				}
		}
    }

    public void AddExternalVelocity(Vector2 vel)
    {
        externalVelocity += vel;
    }

    void InputToMovement()
    {
        if (Grounded)
        {
            // RUNNING
            if (run != 0)
            {
                if (Mathf.Sign(run) == Mathf.Sign(velocity.x))
                {
                    if (Mathf.Abs(velocity.x) < RUN_MAX_SPEED)
                    {
                        acceleration.x += RUN_ACCEL * run * Mathf.Cos(slopeAngle * Mathf.Deg2Rad);
                    }
                }
                else
                {
                    velocity.x /= 1 + FRICTION * Time.deltaTime;
                    acceleration.x += RUN_ACCEL * run * Mathf.Cos(slopeAngle * Mathf.Deg2Rad);
                }
            }
            else
            {
                velocity.x /= 1 + FRICTION * Time.deltaTime;
            }
        }
        else
        {
            if (run != 0)
            {
                if (Mathf.Sign(run) != Mathf.Sign(velocity.x) || Mathf.Abs(velocity.x) < AIR_MAX_SPEED)
                {
                    acceleration.x += AIR_RUN_ACCEL * run;
                }
            }
            else
            {

            }

            // HOLD_JUMP
            if (Input.GetKey(KeyCode.Space))
            {
                if (jumpHoldTime < JUMP_HOLD_TIME)
                {
                    jumpHoldTime += Time.deltaTime;
                    acceleration += -GRAVITY * JUMP_HOLD_MULTIPLYIER;
                }
            }
        }
    }

    void FlowerSpeed()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 4f*SKIN_WIDTH, LayerMask.GetMask("Slope"));
        if (hit && hit.collider.tag == "Flower")
        {
            Flower flower = hit.collider.gameObject.GetComponent<Flower>();
            if (flower.Growing && Grounded)
            {
                //velocity = new Vector2(flower.Velocity.x, flower.Velocity.y);
                //transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
                transform.position += flower.Velocity;
				footsteps.Stop ();
            }
        }
    }

    void CalculateDynamics()
    {
        if (!Grounded)
            velocity += GRAVITY * Time.deltaTime;
        else
            velocity.y = -0.1f;
        velocity += acceleration * Time.deltaTime;
        traslation = (velocity + externalVelocity) * Time.deltaTime;
    }

    void DescendSlope()
    {
        float directionX = Mathf.Sign(traslation.x);
        Vector2 rayOrigin = ((Vector2)transform.position) + (directionX == 1 ? bounds.bottomLeft : bounds.bottomRight);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, 1f, LayerMask.GetMask("Slope"));
        Debug.DrawRay(rayOrigin + new Vector2(0.01f,0f), Vector2.down, Color.green);

        if (hit)
        {
            Debug.DrawRay(hit.point, hit.normal, Color.yellow);
            float descendSlopeAngle = Vector2.Angle(hit.normal, Vector2.up) * Mathf.Sign(hit.normal.x) * -1;
            if(descendSlopeAngle != 0 && Mathf.Abs(descendSlopeAngle) <= MAX_DESCEND_ANGLE)
            {
                if (Mathf.Sign(hit.normal.x) == directionX)
                {
                    //if (grounded || hit.distance - SKIN_WIDTH <= Mathf.Tan(Mathf.Abs(descendSlopeAngle) * Mathf.Deg2Rad) * Mathf.Abs(traslation.x))
                    if (Grounded || hit.distance - SKIN_WIDTH <= Mathf.Abs(traslation.y))
                    {
                        //Debug.Log("DescendSlope!");
                        /*float moveDistance = Mathf.Abs(traslation.x);
                        float descendTraslation = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance * -1;

                        traslation.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(traslation.x);
                        traslation.y -= descendTraslation;*/
                        traslation.y -= traslation.x * Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(traslation.x) * -1;

                        //if(traslation.y < 0)
                        //    velocity.y += traslation.y / Time.deltaTime;
                        descendSlope = true;
                        Grounded = true;
                    }
                }
            }
            slopeAngle = descendSlopeAngle;
        }
    }

    void HorizontalCollisions()
    {
        LateralBlock = 0;
		GameObject objectToOrificate = null;
        float directionX = Mathf.Sign(traslation.x);
        float rayLength = Mathf.Abs(traslation.x) + SKIN_WIDTH;
        for(int i = 0; i < horizontalRayNum; i++)
        {
            Vector2 rayOrigin = ((Vector2)transform.position) + (directionX == -1 ? bounds.bottomLeft : bounds.bottomRight) + i * Vector2.up * horizontalRaySpacing;
            Vector2 rayDirection = Vector2.right * directionX;
            LayerMask layerMask = isPlayer ? LayerMask.GetMask("Wall", "Pushable") : LayerMask.GetMask("Wall");
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayLength, layerMask);

            if (hit)
            {
                bool pushing = false;
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Pushable"))
                {
                    SidePushInput sidePushInput = hit.collider.gameObject.GetComponent<SidePushInput>();
                    if (sidePushInput.Controller.LateralBlock != directionX)
                    {
                        pushing = true;

                        sidePushInput.Push(new Vector2(velocity.x, 0f));
                        //velocity.x /= sidePushInput.Mass;
                        traslation.x /= sidePushInput.Mass;
                    }
                }
                
                if(!pushing) // else
                {
                    velocity.x = 0f;
                    LateralBlock = directionX;
                }
                objectToOrificate = null;

                if (gameObject.tag == "GoldEntity" && hit.collider.gameObject.tag == "Orificable")
                    objectToOrificate = hit.collider.gameObject;
                else if (gameObject.tag == "PlantEntity" && hit.transform.tag == "Orificated")
                    KillPlantEntity();

                traslation.x = (hit.distance - SKIN_WIDTH) * directionX;
                rayLength = hit.distance;
            }

            Debug.DrawRay(rayOrigin, rayDirection, Color.red);
        }

        if (objectToOrificate != null)
        {
            SceneElement sceneElement = objectToOrificate.GetComponent<SceneElement>();
            Transform currentTransform = objectToOrificate.transform;
            while (!sceneElement && currentTransform.parent != null)
            {
                currentTransform = currentTransform.parent;
                sceneElement = currentTransform.gameObject.GetComponent<SceneElement>();
            }
            if (sceneElement)
            {
                sceneElement.TurnIntoGold();
                if (sceneElement.othersToOrificate != null)
                    foreach (SceneElement s in sceneElement.othersToOrificate)
                        s.TurnIntoGold();
            }   
        }
    }

    void VerticalCollisions()
    {
        GameObject objectToOrificate = null;
        bool someoneHit = false;
        float directionY = Mathf.Sign(velocity.y);
        float directionX = Mathf.Sign(velocity.x);
        if (directionY == 1)
            return;
        float rayLength = Mathf.Abs(traslation.y) + SKIN_WIDTH;
        for (int i = 0; i < verticalRayNum; i++)
        {
            Vector2 rayOrigin = ((Vector2)transform.position) + (directionX == 1 ? bounds.bottomLeft + rayMargins/2f : bounds.bottomRight - rayMargins / 2f) + i * Vector2.right * directionX * verticalRaySpacing;
            Vector2 rayDirection = Vector2.up * directionY;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayLength, LayerMask.GetMask("Platform", "Slope"));
            if (hit)
            {
                if(gameObject.tag == "GoldEntity" && hit.collider.gameObject.tag == "Orificable")
                    objectToOrificate = hit.collider.gameObject;
                else if (gameObject.tag == "PlantEntity" && hit.transform.tag == "Orificated")
                    KillPlantEntity();

                traslation.y = (hit.distance - SKIN_WIDTH) * directionY;

                //float angle = Vector2.Angle(Vector2.up, hit.normal) * Mathf.Sign(hit.normal.x);
                //if (Mathf.Abs(angle) > 90f+30f)
                //    continue;

                rayLength = hit.distance;
                //if (directionY == -1)
                //{
                if (!Grounded)
                {
                    if(hit.collider.tag != "Flower")
                        velocity.y = -0.1f;
                    acceleration.y = 0f;
                }
                someoneHit = true;
                //grounded = true;
                //}
                climbSlope = false;
                slopeAngle = Vector2.Angle(Vector2.up, hit.normal) * Mathf.Sign(hit.normal.x) * -1;
                if (Mathf.Abs(slopeAngle) < MAX_SLOPE_ANGLE) { }
                    ClimbSlope();
            }
            else
            {
                if (!descendSlope)
                {
                    Grounded = false;
                    slopeAngle = 0f;
                }
            }
            Grounded = someoneHit;
            if (Grounded)
                if (Mathf.Abs(velocity.x) > RUN_MAX_SPEED)
                    velocity.x = RUN_MAX_SPEED * Mathf.Sign(velocity.x);

            Debug.DrawRay(rayOrigin, rayDirection, Color.red);
        }

        if (Grounded && objectToOrificate != null)
        {
            SceneElement sceneElement = objectToOrificate.GetComponent<SceneElement>();
            Transform currentTransform = objectToOrificate.transform;
            while (!sceneElement && currentTransform.parent != null)
            {
                currentTransform = currentTransform.parent;
                sceneElement = currentTransform.gameObject.GetComponent<SceneElement>();
            }
            if (sceneElement)
            {
                sceneElement.TurnIntoGold();
                if (sceneElement.othersToOrificate != null)
                    foreach (SceneElement s in sceneElement.othersToOrificate)
                        s.TurnIntoGold();
            }
        }
    }

    void ClimbSlope()
    {
        /*float moveDistance = Mathf.Abs(translation.x);
        float climbTranslation = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
        if(climbTranslation > translation.y)
            translation.y = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;*/
        traslation.x *= Mathf.Pow(Mathf.Abs(Mathf.Cos(slopeAngle * Mathf.Deg2Rad)), SLOPE_RUN_HANDICAP);
        traslation.y += traslation.x * Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(traslation.x) * -1;
        Grounded = true;
        climbSlope = true;
    }


    void ApplyPositionChange()
    {
        transform.Translate(traslation);
        //velocity = translation / Time.deltaTime;
    }

    void AdjustSprite()
    {
        Transform sloppedTransform = transform;

        if (isPlayer)
        {
            sloppedTransform = sprite.transform;
            if (traslation.x > 0.018f || traslation.x < -0.018f)
                sprite.flipX = traslation.x < 0;

            animator.SetBool("run", (Grounded && Mathf.Abs(traslation.x) > 0.0075f));
            int vertical = 0;
            if (!Grounded && Mathf.Abs(traslation.y) > 0.0075f)
                vertical = Mathf.FloorToInt(Mathf.Sign(traslation.y));
            animator.SetInteger("vertical", vertical);
        }

        if (Mathf.Abs(slopeAngle) < MAX_SLOPE_ANGLE)
        {
            //sprite.transform.eulerAngles = Vector3.Lerp(sprite.transform.eulerAngles, new Vector3(0f, 0f, targetAngle / 3.5f), Time.deltaTime * 10f);
            Quaternion currentRotation = sloppedTransform.transform.rotation;
            sloppedTransform.transform.eulerAngles = new Vector3(sloppedTransform.transform.eulerAngles.x, sloppedTransform.transform.eulerAngles.y, slopeAngle / 3f);
            Quaternion targetRotation = sloppedTransform.transform.rotation;
            sloppedTransform.transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, Time.deltaTime * 10f);
        }     
    }

    void KillPlantEntity()
    {
        if (!isDead)
        {
            isDead = true;
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Flower"))
                GameObject.DestroyImmediate(g);

            //transform.position = startPos;
            LevelManager.levelManager.RespawnPlayer();
            //Camera.main.GetComponent<CameraMovement>().ResetCamera();
        }
    }
}
