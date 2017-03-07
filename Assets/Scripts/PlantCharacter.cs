using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantCharacter : MonoBehaviour {

    Collider2D feet;
    bool collidingWithPlant = false;

	// Use this for initialization
	void Start () {

        feet = GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () {

        //if (Input.GetKeyDown(KeyCode.LeftControl) && !collidingWithPlant)
            //GrowPlant();	
	}

    void GrowPlant()
    {
        GameObject plant = GameObject.Instantiate(Resources.Load("Prefabs/Flower") as GameObject, new Vector3(transform.position.x, transform.position.y - 1.6f, 0), Quaternion.identity);
        collidingWithPlant = true;
    }
}
