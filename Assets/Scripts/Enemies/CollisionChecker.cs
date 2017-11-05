using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
	public string tagName = "Player";
	public GameObject listener;

	// If collision is detected, we inform the listener object
	void OnTriggerEnter2D (Collider2D other)
	{		
		if (other.gameObject.tag == tagName) {
			listener.GetComponent<ICollisionDetectionListener> ().CollisionEnter (other);
		}

	}

	void OnTriggerExit2D (Collider2D other)
	{		
		if (other.gameObject.tag == tagName) {
			listener.GetComponent<ICollisionDetectionListener> ().CollisionExit (other);
		}

	}
}
