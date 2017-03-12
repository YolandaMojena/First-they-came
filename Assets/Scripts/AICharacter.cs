using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacter : MonoBehaviour {

    [SerializeField]
    CharacterMovement chmov;
    [SerializeField]
    SpriteRenderer sprite;

    // Use this for initialization
    void Start () {
        chmov = GetComponent<CharacterMovement>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        if(sprite)
            SetRandomColor();

	}
	
	// Update is called once per frame
	void Update () {
        if (chmov)
        {
            if (Random.value < 0.05f)
            {
                float r = Random.value;
                if (r < 0.25f)
                    chmov.run = -1;
                else if (r < 0.5f)
                    chmov.run = 1;
                else
                    chmov.run = 0;

            }
        }
	}

    void SetRandomColor()
    {

        int COLOR_MAX = 255;
        int COLOR_MIN = 200;
        int[] rgb = { COLOR_MIN, COLOR_MIN, COLOR_MIN };
        int maxes = 0;

        for(int i = 0; i < rgb.Length; i++)
        {
            if (maxes < 2 && Random.value < 0.5f)
            {
                rgb[i] = COLOR_MAX;
                maxes++;
            }
        }
        if (maxes == 0)
        {
            int r = Mathf.FloorToInt(Random.value * 0.99f * 3f);
            rgb[r] = COLOR_MAX;
        }

        sprite.color = new Color(rgb[0]/255f, rgb[1]/255f, rgb[2]/255f);
    }
}
