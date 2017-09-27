using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{

	public float timeBeforeFall = 1f;


	void OnCollisionEnter2D (Collision2D other)
	{
		if (other.transform.tag == "Player") {
			transform.parent.GetComponent<Animator> ().SetTrigger ("collision");
			Invoke ("Fall", timeBeforeFall);
		}
	}

	void Fall ()
	{
		transform.parent.GetComponent<Animator> ().enabled = false;
		GetComponent<Rigidbody2D> ().isKinematic = false;
	}
}
