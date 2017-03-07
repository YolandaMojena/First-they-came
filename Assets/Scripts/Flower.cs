using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour {

    [SerializeField]
    GameObject stem;

    [SerializeField]
    float desiredScale;
    [SerializeField]
    float heightStep = 0.2f;
    [SerializeField]
    float sideStep = 0.6f;

    float initialHeight;
    float initialX;

    float targetX;
    float targetY;

    [SerializeField]
    float MAX_HEIGHT = 2.0f;
    [SerializeField]
    float MAX_DISPLACEMENT = 5.0f;

    LineRenderer stemRenderer;

    bool fullyGrown = false;

    // Use this for initialization
    void Start () {

        initialHeight = transform.position.y;
        targetY = initialHeight;
        initialX = transform.position.x;
        targetX = initialX;
        stemRenderer = GetComponent<LineRenderer>();
        StartCoroutine(GrowBase());
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey(KeyCode.LeftControl) && !fullyGrown)
            GrowStem();

        if (!fullyGrown && Input.GetKeyUp(KeyCode.LeftControl))
            fullyGrown = true;
	}

    void GrowStem()
    {
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if(Mathf.Abs(transform.position.y - initialHeight) < MAX_HEIGHT)
        {
            if (targetPos.y < transform.position.y)
                targetPos.y = transform.position.y;

            if (Mathf.Abs(transform.position.x - initialX) >= MAX_DISPLACEMENT)
                targetPos.x = transform.position.x;

            targetPos.z = transform.position.z;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime);
        }
    }

    IEnumerator GrowBase()
    {
        while(transform.localScale.x < desiredScale)
        {
            yield return new WaitForSecondsRealtime(0.05f);
            transform.localScale *= 1.2f;
        }
    }
}
