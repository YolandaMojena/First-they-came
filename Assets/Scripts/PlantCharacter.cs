using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantCharacter : MonoBehaviour {


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.S) && GetComponent<CharacterMovement>().Grounded)
            GrowPlant();	
	}

    void GrowPlant()
    {
        GameObject plant = GameObject.Instantiate(Resources.Load("Prefabs/Flower") as GameObject, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
    }
}
