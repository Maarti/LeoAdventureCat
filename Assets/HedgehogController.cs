using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HedgehogController : AbstractEnemy
{
	public float chargeVelocity = 6f, chargeCooldown = 2f;


	protected override void Update ()
	{
		base.Update ();
		if (isAtacking) {
			Vector2 moveVel = rb.velocity;
			moveVel.x = (facingRight) ? chargeVelocity : -chargeVelocity;
			rb.velocity = moveVel;
		}
	}

	public override void Attack ()
	{
		base.Attack ();
		rb.velocity = Vector2.zero;
		Charge ();
	}

	void Charge ()
	{
		Vector2 newVelocity = rb.velocity;
		newVelocity.x += (facingRight) ? chargeVelocity : -chargeVelocity;
		StartCoroutine (ChargeCooldown ());
	}

	IEnumerator ChargeCooldown ()
	{
		yield return new WaitForSeconds (chargeCooldown);
		isAtacking = false;
		isMoving = true;
	}

}