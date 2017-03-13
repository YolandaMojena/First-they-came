using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Tutorial : MonoBehaviour {

	public Image[] images;

	public GameObject[] childs;

	public float fadeTime;


	void Awake()
	{
		foreach(Image image in images)
		{
			Color tmp = image.GetComponent<Image>().color;
			tmp.a = 0f;
			image.GetComponent<Image>().color = tmp;
		}

		StartCoroutine ("WaitForStarting");
	}
	
	public void TriggerEnter2DChild(Collider2D other, string childName) {
		if (other.tag == "GoldEntity") {
			if (childName == "Moverse") {
				images[0].CrossFadeAlpha (1f, fadeTime, false);
				images[1].CrossFadeAlpha (1f, fadeTime, false);
				StartCoroutine ("QuitImages");
			}
		}

	}


	IEnumerator QuitImages()
	{
		yield return new WaitForSeconds (fadeTime + 1);
		/*foreach(Image image in images)
		{
			if (image.color.a >= 0.9f)
				image.CrossFadeAlpha (0f, fadeTime - 0.5f, false);
		}*/
	}



	public IEnumerator WaitForStarting(){
		yield return new WaitForSeconds (8);

		foreach(GameObject child in childs){
			child.GetComponent<CircleCollider2D> ().enabled = true;
		}
	}
}
