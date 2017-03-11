using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFlowers : MonoBehaviour {

    const float COOLDOWN = 2f;
    float cooldown = 0f;

    CharacterMovement controller;

    // Use this for initialization
    void Start () {
        controller = GetComponent<CharacterMovement>();
	}
	
	// Update is called once per frame
	void Update () {
        if (cooldown > 0f)
            cooldown -= Time.deltaTime;
        else
        {
            if (Input.GetKeyDown(KeyCode.S) && controller.Grounded)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.25f, LayerMask.GetMask("Slope"));
                if (hit)
                {
                    GameObject.Instantiate(Resources.Load("Prefabs/Flower"), hit.point, Quaternion.identity);
                    cooldown = COOLDOWN;

                    // PARTICLES AND SOUND
                    LevelManager.levelManager.Plant();        
                }
            }
        }
	}
}