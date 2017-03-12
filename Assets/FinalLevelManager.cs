using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalLevelManager : MonoBehaviour {

	public static FinalLevelManager levelManager;

	public GameObject currentCheckpoint;

	public GameObject player;

	public GameObject goldParticle; // PJ_1 orifica

	public GameObject goldParticleInf; // PJ_1 orifica, version en loop

	public GameObject deathParticle; // PJ_2 toca oro

	public GameObject checkpointParticle; // PJ_2 aparece en checkpoint
	public float checkpointDelay; // The time between death and re-generation

	public GameObject plantParticle; // PJ_2 plants a flower

	public GameObject finalGameOrification;

	[SerializeField]
	GameObject goldCharacter;
	[SerializeField]
	GameObject plantCharacter;
	[SerializeField]
	GameObject levelEnd;

	void Awake()
	{
		levelManager = this;
	}

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<CharacterMovementFinal>().gameObject;
	}

	void Update()
	{
		if (goldCharacter.transform.position.x >= levelEnd.transform.position.x && goldCharacter.activeSelf) {
			StartCoroutine ("WaitForResetFinal");
		}
		if(plantCharacter.transform.position.x >= 39 && plantCharacter.activeSelf){
			StartCoroutine ("FinalOfTheGame");
		}
	}
		
	public void RespawnPlayer(){
		StartCoroutine ("RespawnPlayerCoroutine");
	}

	public IEnumerator RespawnPlayerCoroutine(){

		//PARTICLES AND SOUNDS
		Instantiate(deathParticle, player.GetComponentInChildren<SpriteRenderer>().bounds.center, player.transform.rotation);
		yield return new WaitForSeconds (0.2f);
		player.SetActive(false);
		yield return new WaitForSeconds (checkpointDelay);
		player.transform.position = currentCheckpoint.transform.position;
		//player.SetActive(true);
		player.GetComponent<CharacterMovementFinal>().isDead = false;
		yield return new WaitForSeconds (0.5f);
		Instantiate (checkpointParticle, player.transform.position, player.transform.rotation);
		Camera.main.GetComponent<CameraMovement>().ResetCamera();

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

	public void MakeHearthGold(string typeParticle, bool isAlreadyGold, Vector3 position, Quaternion rotation){
		if (!isAlreadyGold) {
			if (typeParticle == "Finite") {
				Instantiate (goldParticle, position, rotation);
			} else {
				Instantiate (goldParticleInf, position, rotation);
			}
		}
	}

	public void Plant(){
		Instantiate (plantParticle, player.transform.position, player.transform.rotation);
	}

	IEnumerator WaitForResetFinal()
	{
		yield return new WaitForSecondsRealtime(1.5f);
		plantCharacter.SetActive(true);
		Camera.main.GetComponent<CameraMovement>().ResetCamera();
        goldCharacter.SetActive(false);
		//goldCharacter.GetComponent<CharacterMovementFinal> ().enabled = false;
		player = plantCharacter;
	}

	IEnumerator FinalOfTheGame()
	{
		player.GetComponent<CharacterMovementFinal> ().enabled = false;
		Camera.main.GetComponentInChildren<AudioSource> ().Play ();
		finalGameOrification.SetActive (true);
		finalGameOrification.GetComponent<SceneElementFinal> ().TurnIntoGold ();
		//Orificar suelo y árboles, crear más partículas
		yield return new WaitForSeconds (1.2f);
		Instantiate(deathParticle, player.GetComponentInChildren<SpriteRenderer>().bounds.center, player.transform.rotation);
		player.SetActive(false);
		//fundido en negro
		//poema
		//créditos
	}
}

