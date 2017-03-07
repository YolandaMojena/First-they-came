using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSprite : MonoBehaviour {

    public Transform target;
    float multiplier = 20f;
    Vector3 offset;

	// Use this for initialization
	void Start () {
        transform.parent = null;
        offset =  transform.position - target.position;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * multiplier);
	}
}
