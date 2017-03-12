using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneElement : MonoBehaviour {

    SpriteRenderer spriteRenderer;
    Material spriteMaterial;
    SpriteRenderer goldRenderer;
    Vector2 pos;

    //Dissolve
    float threshold = 1f;
    [SerializeField]
    float FREQUENCY = 0.05f;
    [SerializeField]
    float STEP = 0.025f;

    public SceneElement[] othersToOrificate;
    public bool Orificated = false;

	// Use this for initialization
	void Start () {
        gameObject.tag = "Orificable";
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteMaterial = spriteRenderer.materials[0];
        spriteRenderer.material.SetFloat("_Threshold", 1f);
        pos = transform.position;	
	}

    public void TurnIntoGold()
    {
        if (!Orificated)
        {
            Orificated = true;
            gameObject.tag = "Orificated";
            foreach (Transform t in transform)
                t.tag = "Orificated";
            StartCoroutine("Orificate");
        }
    }

    // Coroutine which turns gradually into gold
    IEnumerator Orificate()
    {
        // PARTICLES AND SOUNDS
        LevelManager.levelManager.MakeGold("Finite", false, transform.position, transform.rotation);

        threshold = spriteRenderer.material.GetFloat("_Threshold");
        while (threshold > 0)
        {   
            spriteRenderer.material.SetFloat("_Threshold", threshold - STEP);
            threshold = spriteRenderer.material.GetFloat("_Threshold");
            yield return new WaitForSecondsRealtime(FREQUENCY);
        }
    }
}
