using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HedgehogController : AbstractEnemy
{
	public float chargeVelocity = 3f, chargeCooldown = 0.5f;

	float speedInit;

	protected override void Start ()
	{
		base.Start ();
		speedInit = speed;
	}

	protected override void Update ()
	{
		base.Update ();
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

}