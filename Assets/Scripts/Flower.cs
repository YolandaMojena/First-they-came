using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour {

    [SerializeField]
    Transform Target;

    Vector3 previousPosition;
    public Vector3 Velocity = Vector3.zero;

    //[SerializeField]
    float DESIRED_SCALE = 1f;
    //[SerializeField]
    float GROWTH_RATE = 1.2f;
    //[SerializeField]
    float GROWTH_FREQ = 0.1f;
    //[SerializeField]
    float MAX_LENGTH = 4f;//3.0f;
    //[SerializeField]
    float SPEED = 1.25f;//0.75f;

    //float WIDTH;

    Vector3 initialPos;
    LineRenderer stemRenderer;
    public bool Growing = false;

    //used to add less points to the lineRenderer
    const float NEW_POINT_FREQ = 24;
    float frameCounter;

    // Use this for initialization
    void Start () {
        //WIDTH = GetComponent<BoxCollider2D>().size.x * transform.lossyScale.x;
        initialPos = transform.position;
        stemRenderer = GetComponent<LineRenderer>();
        stemRenderer.SetPosition(0, transform.position);
        stemRenderer.SetPosition(1, transform.position);
        StartCoroutine(GrowBase());

        if (!Target)
            Target = GameObject.FindGameObjectWithTag("PlantEntity").transform;
	}
	
	// Update is called once per frame
	void Update () {

        if (Growing)
            GrowStem();    
    }

    void GrowStem()
    {
        //Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        previousPosition = transform.position;

        float traveledDistance = (initialPos - transform.position).magnitude;

        float xDif = (Target.position.x - transform.position.x);
        xDif *= Mathf.Clamp01(traveledDistance);
        xDif = Mathf.Clamp(xDif * 1.5f, -1.5f, 1.5f);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * Mathf.Sign(xDif), 1.2f, LayerMask.GetMask("Wall"));
        if (hit)
        {
            if (Mathf.Sign(hit.normal.x) != Mathf.Sign(xDif))
                xDif = 0;
        }


        Vector3 targetPos = new Vector3(xDif + Mathf.Sin(traveledDistance * Mathf.PI) * 0.33f, 1f + Mathf.Cos(traveledDistance * Mathf.PI)*0.5f);
        targetPos.Normalize();
        targetPos += transform.position;

        if ((initialPos - transform.position).magnitude >= MAX_LENGTH)
        {
            Growing = false;
            Velocity = Vector3.zero;
        }
        else
        {
            if (targetPos.y < transform.position.y)
                targetPos.y = transform.position.y;

            targetPos.z = transform.position.z;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * SPEED);

            frameCounter++;
            if (frameCounter >= NEW_POINT_FREQ)
            {
                stemRenderer.numPositions += 1;
                frameCounter = 0;
            }

            stemRenderer.SetPosition(stemRenderer.numPositions - 1, transform.position);
        }

        Velocity = transform.position - previousPosition;
    }

    IEnumerator GrowBase()
    {
        while(transform.localScale.x < DESIRED_SCALE)
        {
            yield return new WaitForSecondsRealtime(GROWTH_FREQ);
            transform.localScale *= GROWTH_RATE;
        }

        Growing = true;
    }
}
