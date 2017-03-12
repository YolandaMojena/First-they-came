using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManagement : MonoBehaviour {

	public LevelManager levelManager;

	private bool converted = false;

	// Use this for initialization
	void Start () {
		levelManager = FindObjectOfType<LevelManager> ();
	}


	void OnTriggerEnter2D (Collider2D other) {
		
		if (other.name == "Player1") {
			
			if (gameObject.tag == "ConvertGold") {
				levelManager.MakeGold ("Finite", converted, gameObject.transform.position, gameObject.transform.rotation);
				converted = true;
			}

			if (gameObject.tag == "ConvertGoldForever") {
				levelManager.MakeGold ("Infinite", converted, gameObject.transform.position, gameObject.transform.rotation);
				converted = true;
			}

		} 

		else if (other.name == "Player2") 
		{
			if (gameObject.tag == "Killer") {
				levelManager.RespawnPlayer ();
			}
		}
	}

	void OnTriggerStay2D (Collider2D other){
		if (other.name == "Player2") {
			if (gameObject.tag == "Grass") {
				if (Input.GetKeyDown(KeyCode.F)) {
					levelManager.Plant ();
				}
			}
		}
	}

}
