using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{

	public GameObject uiElementOn, uiElementOff;

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player") {
			SetHalo ();
			Destroy (this.gameObject);
		}
	}

	void SetHalo ()
	{
		if (uiElementOn)
			uiElementOn.GetComponent<Animator> ().SetBool ("halo", true);
		if (uiElementOff)
			uiElementOff.GetComponent<Animator> ().SetBool ("halo", false);
	}

}
