using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarkingDogController : AbstractEnemy, ICollisionDetectionListener
{
	public float gruntingTime = 1f, attackCooldown = 1f;
	public AudioClip growlingClip, barkClip;

	public GameObject detectedEnnemy;
	public bool isEnnemyDetected = false;

	protected override void Start ()
	{
		base.Start ();
	}

	protected override void FixedUpdate ()
	{
		base.FixedUpdate ();

		// If ennemy is detected, we look at him
		if (isEnnemyDetected) {
			if (detectedEnnemy.transform.position.x >= this.gameObject.transform.position.x) {
				if (!facingRight)
					Flip ();
			} else {
				if (facingRight)
					Flip ();
			}
		}
	}

	protected override void Update ()
	{
		base.Update ();
		animator.SetFloat ("x.velocity", Mathf.Abs (rb.velocity.x));
	}

	public override void Attack ()
	{
		base.Attack ();
		rb.velocity = Vector2.zero;
		StartCoroutine (Grunting ());
	}


	IEnumerator Grunting ()
	{
		// Begin grunting
		animator.SetBool ("isGrunting", true);
		PlayGrowlingSound ();
		yield return new WaitForSeconds (gruntingTime);

		// Bark
		//StopSound ();
		animator.SetBool ("isGrunting", false);

		// Return to patroling
		yield return new WaitForSeconds (1f);
		isAtacking = false;
		isMoving = true;
	}

	// The LoS of the barking dog is a Circle collider, not a Raycast
	public override GameObject CheckLoS ()
	{
		if (isEnnemyDetected) {
			return detectedEnnemy;
		} else {
			return null;
		}
	}

	// Called by HostileZoneController when player collids
	public void CollisionEnter (Collider2D collider)
	{
		// Attack only if the dog is visible on screen
		if (childRenderer.isVisible) {
			isEnnemyDetected = true;
			detectedEnnemy = collider.gameObject;
		}
	}

	public void CollisionExit (Collider2D collider)
	{
		isEnnemyDetected = false;
		detectedEnnemy = null;
	}

	public void PlayBarkSound ()
	{
		audioSource.Stop ();
		audioSource.PlayOneShot (barkClip);
	}

	void PlayGrowlingSound ()
	{
		audioSource.Stop ();
		audioSource.loop = true;
		audioSource.clip = growlingClip;
		audioSource.Play ();
	}

	void StopSound ()
	{
		audioSource.Stop ();
	}

	public override void Die ()
	{
		// we add "isAlive" boolean to each animation from "Any state" so they are not played when dead
		animator.SetBool ("isAlive", false);
		base.Die ();
	}
}