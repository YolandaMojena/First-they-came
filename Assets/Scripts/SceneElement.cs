using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneElement : MonoBehaviour {

    SpriteRenderer spriteRenderer;
    Material spriteMaterial;
    SpriteRenderer goldRenderer;
    Vector2 pos;

    //Dissolve
    float threshold;
    [SerializeField]
    float FREQUENCY = 0.05f;
    [SerializeField]
    float STEP = 0.025f;

    public bool Orificated = false;

	// Use this for initialization
	void Start () {

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteMaterial = spriteRenderer.materials[0];
        pos = transform.position;	
	}

    public void TurnIntoGold()
    {
        if (!Orificated)
        {
            Orificated = true;
            gameObject.tag = "Orificated";
            StartCoroutine("Orificate");
        }
    }

    // Coroutine which turns gradually into gold
    IEnumerator Orificate()
    {
        threshold = spriteRenderer.material.GetFloat("_Threshold");
        while (threshold < 1)
        {   
            spriteRenderer.material.SetFloat("_Threshold", threshold + STEP);
            threshold = spriteRenderer.material.GetFloat("_Threshold");
            yield return new WaitForSecondsRealtime(FREQUENCY);
        }
    }
}
