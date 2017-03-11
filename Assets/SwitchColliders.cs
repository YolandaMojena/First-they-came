using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchColliders : MonoBehaviour {

    bool switched = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (tag == "Orificated" && !switched)
        {
            foreach(Collider2D c in GetComponentsInChildren<Collider2D>())
            {
                c.enabled = !c.isActiveAndEnabled;
                switched = true;
            }
        }
    }
}
