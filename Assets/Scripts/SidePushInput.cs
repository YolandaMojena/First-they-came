using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SidePushInput : MonoBehaviour {

    public CharacterMovement Controller;
    public float Mass = 3f;

	// Use this for initialization
	void Start () {
        Controller = GetComponent<CharacterMovement>();
        Transform current = transform;
        while (!Controller)
        {
            current = transform.parent;
            Controller = current.gameObject.GetComponent<CharacterMovement>();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Push(Vector2 velocity)
    {
        Controller.AddExternalVelocity(velocity / Mass);
    }
}
