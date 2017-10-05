using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointObjectController : MonoBehaviour
{
	bool isActivated = false;
	Animator animator;

	// Use this for initialization
	void Start ()
	{
		animator = GetComponent<Animator> ();
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (!isActivated && other.gameObject.tag == "Player") {
			CheckPointController.cc.Check (this.gameObject.name);
			isActivated = true;
			animator.SetTrigger ("checked");
		}
	}
}
