using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PoemCredits : MonoBehaviour {



	public Image[] images;

	public float fadeTime;

	public bool poem1Time = false;
	private bool poem2Time = false;
	private bool creditsTime = false;

	int i = 1;

	void Awake()
	{
		foreach(Image image in images)
		{
			image.GetComponent<CanvasRenderer>().SetAlpha(0.0f);
		}
	}

	void Update()
	{
			if (checkAlpha(images[0]) <= 0.02f && poem1Time) {
				Fade ("poem1");
			} else if (checkAlpha(images[1]) <= 0.02f && poem2Time)
				Fade ("poem2");
			else if (checkAlpha(images[2]) <= 0.02f && creditsTime)
				Fade ("credits");
	}

	void Fade(string what)
	{
		switch (what) {
		case "poem1":
			poem1Time = false;
			images[0].GetComponent<CanvasRenderer>().SetAlpha(0.001f);
			images[0].CrossFadeAlpha (1f, fadeTime, false);
			StartCoroutine ("WaitForContinuing");
			break;
		case "poem2":
			poem2Time = false;
			images[1].GetComponent<CanvasRenderer>().SetAlpha(0.001f);
			images[1].CrossFadeAlpha (1f, fadeTime, false);
			StartCoroutine ("WaitForContinuing");
			break;
		case "credits":
			images[2].GetComponent<CanvasRenderer>().SetAlpha(0.001f);
			images[2].CrossFadeAlpha (1f, fadeTime, false);
			StartCoroutine ("WaitForContinuing");
			break;
		}
	}

	float checkAlpha(Image image)
	{
		return image.GetComponent<CanvasRenderer>().GetAlpha ();
	}

	public IEnumerator WaitForContinuing(){
		if (i == 1) {
			yield return new WaitForSeconds (15f);
			images[0].CrossFadeAlpha (0f, fadeTime, false);
			yield return new WaitForSeconds (3f);
			poem2Time = true;
		} else if (i == 2) {
			yield return new WaitForSeconds (15f);
			images[1].CrossFadeAlpha (0f, fadeTime, false);
			yield return new WaitForSeconds (3f);
			creditsTime = true;

		} else if (i == 3) {
			Destroy (this);
		}
		i += 1;
		//yield return new WaitForSeconds (0.01f);

	}
}
