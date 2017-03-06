using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterMovement : MonoBehaviour {

    const float SKIN_WIDTH = 0.015f;
    int verticalRayNum = 3;
    float verticalRaySpacing = 0.115f;
    int horizontalRayNum = 3;
    float horizontalRaySpacing = 0.9f;


    const float JUMP_FORCE = 5f;
    const float JUMP_HOLD_TIME = 0.25f;

    const float RUN_ACCEL = 8f;
    const float RUN_MAX_SPEED = 6f;
    const float AIR_RUN_ACCEL = 4f;
    const float FRICTION = 9f;

    

    Vector2 GRAVITY = new Vector2(0f, -19.81f);
    const float HORIZONTAL_DRAG = 0.5f;
    public Vector2 acceleration = Vector2.zero;
    public Vector2 velocity = Vector2.zero;
    public Vector2 translation = Vector2.zero;
    Vector2 position2D;

    struct Bounds
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    Bounds bounds = new Bounds();

    float slopeAngle = 0f;

    [SerializeField]
    bool grounded = false;
    float jumpHoldTime = 0f;
    int run = 0;

    void Start()
    {
        CalculateBounds();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.D))
            run = 1;
        else if (Input.GetKey(KeyCode.A))
            run = -1;
        else
            run = 0;

        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Jump!");
            velocity.y = JUMP_FORCE;
            jumpHoldTime = 0f;
            grounded = false;
        }
    }

    void FixedUpdate()
    {
        position2D = transform.position;
        acceleration = Vector2.zero;


        
        if(!grounded && Input.GetKey(KeyCode.Space))
        {
            if (jumpHoldTime < JUMP_HOLD_TIME)
            {
                jumpHoldTime += Time.deltaTime;
                acceleration += -GRAVITY;
            }
        }

        InputToMovement();


        CalculateDynamics();

        HorizontalCollisions();
        VerticalCollisions();

        ApplyPositionChange();
        AdjustFacing();
    }

    void CalculateBounds()
    {
        float width = 0.23f;
        float height = 1.23f;
        bounds.topLeft = new Vector2(-width / 2f + SKIN_WIDTH, height - SKIN_WIDTH);
        bounds.topRight = new Vector2(width / 2f - SKIN_WIDTH, height - SKIN_WIDTH);
        bounds.bottomLeft = new Vector2(-width / 2f + SKIN_WIDTH, SKIN_WIDTH);
        bounds.bottomRight = new Vector2(width / 2f - SKIN_WIDTH, SKIN_WIDTH);
    }

    void InputToMovement()
    {
        if (grounded)
        {
            if (run != 0)
            {
                if (Mathf.Sign(run) == Mathf.Sign(velocity.x))
                {
                    if (Mathf.Abs(velocity.x) < RUN_MAX_SPEED)
                        acceleration.x += RUN_ACCEL * run;
                }
                else
                {
                    velocity.x /= 1 + FRICTION * Time.deltaTime;
                    acceleration.x += RUN_ACCEL * run;
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
                if (Mathf.Abs(velocity.x) < RUN_MAX_SPEED)
                {
                    acceleration.x += AIR_RUN_ACCEL * run;
                }
            }
            else
            {

            }
        }
    }

    void CalculateDynamics()
    {
        if(!grounded)
            velocity += GRAVITY * Time.deltaTime;
        velocity += acceleration * Time.deltaTime;
        translation = velocity * Time.deltaTime;
    }

    void HorizontalCollisions()
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(translation.x) + SKIN_WIDTH;
        for(int i = 0; i < horizontalRayNum; i++)
        {
            Vector2 rayOrigin = ((Vector2)transform.position) + (directionX == -1 ? bounds.bottomLeft : bounds.bottomRight) + i * Vector2.up * horizontalRaySpacing;
            Vector2 rayDirection = Vector2.right * directionX;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayLength, LayerMask.GetMask("Wall"));
            if (hit)
            {
                velocity.x = 0f;
                translation.x = (hit.distance - SKIN_WIDTH) * directionX;
                rayLength = hit.distance;
            }

            Debug.DrawRay(rayOrigin, rayDirection, Color.red);
        }
    }
    void VerticalCollisions()
    {
        float directionY = Mathf.Sign(velocity.y);
        if (directionY == 1)
            return;
        float rayLength = Mathf.Abs(translation.y) + SKIN_WIDTH;
        for (int i = 0; i < verticalRayNum; i++)
        {
            Vector2 rayOrigin = ((Vector2)transform.position) + (directionY == -1 ? bounds.bottomLeft : bounds.topLeft) + i * Vector2.right * verticalRaySpacing;
            Vector2 rayDirection = Vector2.up * directionY;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayLength, LayerMask.GetMask("Platform"));
            if (hit)
            {
                translation.y = (hit.distance - SKIN_WIDTH) * directionY;
                rayLength = hit.distance;
                if (directionY == -1)
                {
                    if (!grounded)
                    {
                        velocity.y = -0.1f;
                        acceleration.y = 0f;
                    }
                    grounded = true;
                }
            }
            else
            {
                grounded = false;
            }

            Debug.DrawRay(rayOrigin, rayDirection, Color.red);
        }
    }


    void ApplyPositionChange()
    {
        transform.Translate(translation);
    }

    void AdjustFacing()
    {
        if(translation.x > 0.018f || translation.x < -0.018f)
            GetComponentInChildren<SpriteRenderer>().flipX = translation.x < 0;
    }
}
