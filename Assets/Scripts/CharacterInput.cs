using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInput : MonoBehaviour {

    CharacterMovement controller;

	// Use this for initialization
	void Start () {
        controller = GetComponent<CharacterMovement>();
	}
	
	// Update is called once per frame
	void Update () {
        int run = 0;
        if (Input.GetKey(KeyCode.D))
            run += 1;
        if (Input.GetKey(KeyCode.A))
            run += -1;

        //controller.SetInput(run, Input.GetKeyDown(KeyCode.Space), Input.GetKey(KeyCode.Space));
    }
}
