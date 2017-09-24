using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HedgehogController : AbstractEnemy
{
	public float chargeVelocity = 3f, chargeCooldown = 0.5f;

	float speedInit;
	//SpriteRenderer[] sprites;
	//Animator animator;

	protected override void Start ()
	{
		base.Start ();
		speedInit = speed;
		//this.sprites = GetComponentsInChildren <SpriteRenderer> ();
		//animator = GetComponent<Animator> ();
		animator.SetFloat ("chargeVelocity", chargeVelocity); //adapt the charging animation to the charge velocity
	}

	protected override void FixedUpdate ()
	{
		base.FixedUpdate ();
		if (isAtacking) {
			Vector2 moveVel = rb.velocity;
			moveVel.x = (facingRight) ? speed : -speed;
			rb.velocity = moveVel;
		}
	}

	public override void Attack ()
	{
		base.Attack ();
		rb.velocity = Vector2.zero;
		animator.SetBool ("charging", false);
		animator.SetTrigger ("spotted");
	}

	// Charge blindly, then return to safely patrolling while decelarating
	// Called by "attack_begin" Animation
	void Charge ()
	{
		animator.SetBool ("charging", true);
		speed = chargeVelocity;
		StartCoroutine (ChargeCooldown ());
	}

	IEnumerator ChargeCooldown ()
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
	}

	public override void Defend (GameObject attacker, int damage, Vector2 bumpVelocity, float bumpTime)
	{
		//invincible if charging
		if (!isAtacking)
			base.Defend (attacker, damage, bumpVelocity, bumpTime);
	}
}