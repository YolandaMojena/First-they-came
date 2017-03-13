using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Intro : MonoBehaviour {

	public Image jamImage;
	public Image playersImage;
	public Image tutorialImage;
	public Image titleImage;


	public bool jamTime = false;
	public bool playersTime = false;
	public bool tutorialTime = false;
	public bool blackTime = false;
	public bool titleTime = false;

	public Image black;

	public float fadeTime = 1.5f; // Must be lees than 3
	public GameObject music;

	int i = 1;


	void Awake()
	{
		StartCoroutine ("WaitForStarting");
	}

	void Update()
	{
		if (jamTime)
			Fade ("jam");
		else if (checkAlpha (jamImage) <= 0.02f && playersTime) {
			Fade ("players");
		} else if (checkAlpha (playersImage) <= 0.02f && tutorialTime) {
			Fade ("tutorial");
		}else if (checkAlpha (tutorialImage) <= 0.02f && blackTime) {
			Fade ("black");
		}
		else if (checkAlpha(black) <= 0.02f && titleTime)
			Fade ("title");
	}
		
	void Fade(string what)
	{
		switch (what) {
		case "jam":
			jamTime = false;
			jamImage.CrossFadeAlpha (0f, fadeTime, false);
			StartCoroutine ("WaitForContinuing");
			break;
		case "players":
			playersTime = false;
			playersImage.CrossFadeAlpha (0f, fadeTime, false);
			StartCoroutine ("WaitForContinuing");
			break;
		case "tutorial":
			//tutorialTime = false;
			tutorialImage.CrossFadeAlpha (0f, fadeTime + 1f, false);
			StartCoroutine ("WaitForContinuing");
			break;
		case "black":
			blackTime = false;
			tutorialImage.CrossFadeAlpha (0f, fadeTime + 1f, false);
			music.GetComponent<AudioSource> ().Play();
			black.CrossFadeAlpha (0f, 0.7f, false);
			StartCoroutine ("WaitForContinuing");
			break;
		case "title":
			titleTime = false;
			titleImage.CrossFadeAlpha (0f, fadeTime, false);
			break;
		}
	}

	float checkAlpha(Image image)
	{
		return image.GetComponent<CanvasRenderer>().GetAlpha ();
	}

	public IEnumerator WaitForContinuing(){

		yield return new WaitForSeconds (3f);

		if (i == 1)
			jamTime = true;
		else if (i == 2) {
			playersTime = true;
		} else if (i == 3) {
			yield return new WaitForSeconds (8f);
			tutorialTime = true;
		}
		else if (i == 4) {
			tutorialTime = false;
			blackTime = true;
		}
		else if (i == 5)
			titleTime = true;
		i += 1;
	}
		
	public IEnumerator WaitForStarting(){
		yield return new WaitForSeconds (2);
		jamTime = true;
	}
}