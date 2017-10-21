using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangGliderController : MonoBehaviour
{
	public float glideDrag = 1f;
	float originalDrag = 0f;
	PlayerController pc;
	bool isGliding = false;
	Rigidbody2D rb;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (isGliding && pc && pc.isGrounded) {
			StopGlid ();
		}
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.transform.tag == "Player") {
			pc = other.gameObject.GetComponent<PlayerController> ();
			StartGlid (other.gameObject);
		}
	}

	void StartGlid (GameObject glider)
	{
		isGliding = true;
		GetComponent<Collider2D> ().enabled = false;
		this.transform.parent = glider.transform;
		transform.localPosition = glider.GetComponent<IGlider> ().GetHangGliderPosition ();
		rb = glider.GetComponent<Rigidbody2D> ();
		originalDrag = rb.drag;
		rb.drag = glideDrag;
	}

	void StopGlid ()
	{
		rb.drag = originalDrag;
		Destroy (this.gameObject);
	}
}
