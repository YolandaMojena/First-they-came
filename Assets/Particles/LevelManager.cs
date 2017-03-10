using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public GameObject currentCheckpoint;

	private PlayerController player;

	public GameObject goldParticle; // PJ_1 orifica

	public GameObject goldParticleInf; // PJ_1 orifica, version en loop

	public GameObject goldLeavesParticle; // Hojas de oro (nivel 2.1)


	public GameObject deathParticle; // PJ_2 toca oro

	public GameObject checkpointParticle; // PJ_2 aparece en checkpoint
	public float checkpointDelay; // The time between death and re-generation

	public GameObject plantParticle; // PJ_2 plants a flower

	public GameObject grassParticle; // PJ_2 is on grass
	public float grassDelay; // The time between in_grass particles generation


	// Use this for initialization
	void Start () {
		player = FindObjectOfType<PlayerController>();
	}

	public void MakeGold(string typeParticle, bool isAlreadyGold, Vector3 position, Quaternion rotation){
		if (!isAlreadyGold) {
			if (typeParticle == "Finite") {
				Instantiate (goldParticle, position, rotation);
			} else {
				Instantiate (goldParticleInf, position, rotation);
			}
		}
	}

	public void RespawnPlayer(){
		StartCoroutine ("RespawnPlayerCoroutine");
	}

	public IEnumerator RespawnPlayerCoroutine(){
		yield return new WaitForSeconds (0.6f);
		Instantiate (deathParticle, player.transform.position, player.transform.rotation);
		player.enabled = false;
		yield return new WaitForSeconds (checkpointDelay);
		player.transform.position = currentCheckpoint.transform.position;
		player.enabled = true;
		yield return new WaitForSeconds (0.2f);
		Instantiate (checkpointParticle, player.transform.position, player.transform.rotation);
	}

	public void Plant(){
		Instantiate (plantParticle, player.transform.position, player.transform.rotation);
	}
}
