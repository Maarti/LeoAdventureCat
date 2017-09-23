using System;
using System.Collections;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
	public DialogEnum dialog;
	GameUIController guic;

	void Start ()
	{
		guic = GameObject.Find ("Canvas/GameUI").GetComponent<GameUIController> ();
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player") {
			guic.DisplayDialog (dialog);
			Destroy (this.gameObject);
		}
	}
}