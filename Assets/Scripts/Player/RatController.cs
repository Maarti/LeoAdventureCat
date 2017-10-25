using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatController : MonoBehaviour
{

	public float speed = 10;
	Rigidbody2D rb;
	bool facingRight = true;
	Animator animator;


	void Awake ()
	{
		animator = GetComponent<Animator> ();
	}

	void Start ()
	{
		rb = GetComponent<Rigidbody2D> ();

	}

	void FixedUpdate ()
	{
		#if !UNITY_ANDROID && !UNITY_IPHONE && !UNITY_BLACKBERRY && !UNITY_WINRT || UNITY_EDITOR
		Move (Input.GetAxisRaw ("Horizontal"));
		#else
		Move (hInput);
		#endif



	}

	void Move (float horizonalInput)
	{
		

		Vector2 moveVel = rb.velocity;
		moveVel.x = horizonalInput * speed;
		rb.velocity = moveVel;

		animator.SetFloat ("speed", Mathf.Abs (horizonalInput));

		// Flip if direction changed
		if (horizonalInput > 0 && !facingRight)
			Flip ();
		else if (horizonalInput < 0 && facingRight)
			Flip ();
	}

	void Flip ()
	{
		facingRight = !facingRight;
		Vector3 theScale = new Vector3 (transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
		transform.localScale = theScale;
	}
}
