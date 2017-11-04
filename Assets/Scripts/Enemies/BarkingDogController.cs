using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarkingDogController : AbstractEnemy
{
	public float gruntingTime = 1f, attackCooldown = 1f;
	public AudioClip growlingClip, barkClip;

	protected override void Start ()
	{
		base.Start ();
	}

	protected override void FixedUpdate ()
	{
		base.FixedUpdate ();
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