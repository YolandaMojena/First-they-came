using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialChildCollider : MonoBehaviour {

	private Tutorial parent;

	void Awake()
	{
		parent = GetComponentInParent<Tutorial> ();
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "GoldEntity" || other.tag == "PlantEntity")
			parent.TriggerEnter2DChild (other, this.gameObject.name);
		}
	}
