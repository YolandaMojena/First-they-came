using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneElement : MonoBehaviour {

    SpriteRenderer spriteRenderer;
    Material spriteMaterial;
    SpriteRenderer goldRenderer;
    Vector2 pos;

    float threshold;
    const float FREQUENCY = 0.05f;
    const float STEP = 0.025f;
    

    public bool Orificated = false;

	// Use this for initialization
	void Start () {

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteMaterial = spriteRenderer.material;
        pos = transform.position;	
	}
	
	// Update is called once per frame
	void Update () {

        // This should be substituted by a collision check
        if (Input.GetKeyDown(KeyCode.A) && !Orificated)
        {
            Orificated = true;
            StartCoroutine("Orificate");
        }	
	}

    // Coroutine which turns gradually into gold
    IEnumerator Orificate()
    {
        threshold = spriteMaterial.GetFloat("_Threshold");
        while (threshold < 1)
        {
            yield return new WaitForSecondsRealtime(FREQUENCY);
            spriteMaterial.SetFloat("_Threshold", threshold + STEP);
            threshold = spriteMaterial.GetFloat("_Threshold");
        }
    }
}
