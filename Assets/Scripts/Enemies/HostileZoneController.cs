using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostileZoneController : MonoBehaviour
{

	public int damage = 1;
	public Vector2 bumpVelocity = new Vector2 (1, 2);
	public float bumpDuration = 0.5f;

	void OnTriggerEnter2D (Collider2D other)
	{
		Debug.Log ("bark triggered with " + other.gameObject.name);
		string layer = LayerMask.LayerToName (other.gameObject.layer);
		if (other.gameObject.tag == "Player") {
			other.gameObject.GetComponent<IDefendable> ().Defend (this.gameObject, damage, bumpVelocity, bumpDuration);
		}

	}
}
