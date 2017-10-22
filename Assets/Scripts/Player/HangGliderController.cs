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
	AudioSource audioSource;
	Animator anim;

	// Use this for initialization
	void Start ()
	{
		audioSource = GetComponent<AudioSource> ();
		anim = GetComponentInParent<Animator> ();
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
		anim.SetTrigger ("start_glide");
		this.transform.parent.parent = glider.transform;
		transform.parent.localPosition = glider.GetComponent<IGlider> ().GetHangGliderPosition ();
		rb = glider.GetComponent<Rigidbody2D> ();
		originalDrag = rb.drag;
		rb.drag = glideDrag;

		audioSource.Play ();
	}

	void StopGlid ()
	{
		rb.drag = originalDrag;
		audioSource.Stop ();
		Destroy (this.gameObject);
	}
}
