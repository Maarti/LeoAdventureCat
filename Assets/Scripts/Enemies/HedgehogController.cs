using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HedgehogController : AbstractEnemy
{
	public float chargeVelocity = 3f, chargeCooldown = 0.5f;

	float speedInit;
	SpriteRenderer[] sprites;

	protected override void Start ()
	{
		base.Start ();
		speedInit = speed;
		this.sprites = GetComponentsInChildren <SpriteRenderer> ();
	}

	protected override void FixedUpdate ()
	{
		base.FixedUpdate ();

		// remove this when the animation will be finished
		if (this.sprites [0].color != Color.yellow && !isAtacking)
			foreach (SpriteRenderer sprite in this.sprites)
				sprite.color = Color.yellow;
		else if (this.sprites [0].color != Color.red && isAtacking)
			foreach (SpriteRenderer sprite in this.sprites)
				sprite.color = Color.red;
		
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
		Charge ();
	}

	// Charge blindly, then return to safely patrolling while decelarating
	void Charge ()
	{
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
	}

	public override void Defend (GameObject attacker, float damage, Vector2 bumpVelocity, float bumpTime)
	{
		//invincible if charging
		if (!isAtacking)
			base.Defend (attacker, damage, bumpVelocity, bumpTime);
	}
}