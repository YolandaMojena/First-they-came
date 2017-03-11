using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public static LevelManager levelManager;

	public GameObject currentCheckpoint;

	public GameObject player;

	public GameObject goldParticle; // PJ_1 orifica

	public GameObject goldParticleInf; // PJ_1 orifica, version en loop

	public GameObject goldLeavesParticle; // Hojas de oro (nivel 2.1)

	public GameObject deathParticle; // PJ_2 toca oro

	public GameObject checkpointParticle; // PJ_2 aparece en checkpoint
	public float checkpointDelay; // The time between death and re-generation

	public GameObject plantParticle; // PJ_2 plants a flower

	public GameObject grassParticle; // PJ_2 is on grass
	public float grassDelay; // The time between in_grass particles generation

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
		player = FindObjectOfType<CharacterMovement>().gameObject;
	}

    void Update()
    {
        if (goldCharacter.transform.position.x >= levelEnd.transform.position.x && goldCharacter.activeSelf)
            StartCoroutine("WaitForReset");

        else if (plantCharacter.transform.position.x >= levelEnd.transform.position.x && goldCharacter.activeSelf)
            StartCoroutine("WaitForEnd");
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

        //PARTICLES AND SOUNDS
        Instantiate(deathParticle, player.GetComponentInChildren<SpriteRenderer>().bounds.center, player.transform.rotation);
        yield return new WaitForSeconds (0.2f);
		player.SetActive(false);
		yield return new WaitForSeconds (checkpointDelay);
		player.transform.position = currentCheckpoint.transform.position;
		//player.SetActive(true);
        player.GetComponent<CharacterMovement>().isDead = false;
		yield return new WaitForSeconds (0.5f);
		//Instantiate (checkpointParticle, player.transform.position, player.transform.rotation);
        Camera.main.GetComponent<CameraMovement>().ResetCamera();
    }

	public void Plant(){
		Instantiate (plantParticle, player.transform.position, player.transform.rotation);
	}

    IEnumerator WaitForReset()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        plantCharacter.SetActive(true);
        Camera.main.GetComponent<CameraMovement>().ResetCamera();
        goldCharacter.SetActive(false);
        LevelManager.levelManager.player = plantCharacter;
    }

    IEnumerator WaitForEnd()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        //HAY QUE CONFIGURAR LOS BUILD SETTINGS PARA LAS ESCENAS
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1, LoadSceneMode.Additive);
    }
}
