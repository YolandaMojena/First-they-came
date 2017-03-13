using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ramas_Interruptor : MonoBehaviour {

    bool initialized = false;

    Vector3 initialPosition;
    const float DISPLACEMENT = 3f;
    const float SPEED = 1f;

	// Use this for initialization
	void Start () {
        initialPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if(!initialized && tag == "Orificated")
        {
            if (Vector3.Distance(transform.position, initialPosition) < DISPLACEMENT)
                transform.position += Vector3.up * SPEED * Time.deltaTime;
            else
                initialized = true;
        }
	}
}
