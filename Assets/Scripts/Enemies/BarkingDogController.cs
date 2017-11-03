using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarkingDogController : AbstractEnemy
{
	public float gruntingTime = 1f, attackCooldown = 1f;
	Collider2D barkCollider;

	protected override void Start ()
	{
		base.Start ();
	}

	protected override void FixedUpdate ()
	{
		base.FixedUpdate ();
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
		yield return new WaitForSeconds (gruntingTime);

		// Bark
		animator.SetBool ("isGrunting", false);

		// Return to patroling
		yield return new WaitForSeconds (1f);
		isAtacking = false;
		isMoving = true;
	}



	/*	IEnumerator ChargeCooldown ()
	{
		yield return new WaitForSeconds (chargeCooldown);
		isAtacking = false;
		isMoving = true;
		while (speed > speedInit) {
			speed -= Time.deltaTime * chargeVelocity; //deceleration
			yield return null;
		}
		speed = speedInit;
		animator.SetBool ("charging", false);
	}*/

	/*public override void Defend (GameObject attacker, int damage, Vector2 bumpVelocity, float bumpTime)
	{
		//invincible if charging
		if (!isAtacking)
			base.Defend (attacker, damage, bumpVelocity, bumpTime);
	}*/
}