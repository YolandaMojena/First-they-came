using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour {

    [SerializeField]
    float DESIRED_SCALE = 0.12f;
    [SerializeField]
    float GROWTH_RATE = 1.2f;
    [SerializeField]
    float GROWTH_FREQ = 0.1f;
    [SerializeField]
    float MAX_LENGTH = 3.0f;
    [SerializeField]
    float SPEED = 0.75f;

    Vector3 initialPos;
    LineRenderer stemRenderer;
    bool growing = false;

    //used to add less points to the lineRenderer
    const float NEW_POINT_FREQ = 24;
    float frameCounter;

    // Use this for initialization
    void Start () {

        initialPos = transform.position;
        stemRenderer = GetComponent<LineRenderer>();
        stemRenderer.SetPosition(0, transform.position);
        stemRenderer.SetPosition(1, transform.position);
        StartCoroutine(GrowBase());
	}
	
	// Update is called once per frame
	void Update () {

        if (growing)
            GrowStem();    
    }

    void GrowStem()
    {
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if ((initialPos - transform.position).magnitude >= MAX_LENGTH)
            growing = false;

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
    }

    IEnumerator GrowBase()
    {
        while(transform.localScale.x < DESIRED_SCALE)
        {
            yield return new WaitForSecondsRealtime(GROWTH_FREQ);
            transform.localScale *= GROWTH_RATE;
        }

        growing = true;
    }
}
