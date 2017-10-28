using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopePartController : MonoBehaviour, IDefendable
{
	public int life = 10;
	Animator animator;

	void Awake ()
	{
		animator = GetComponent<Animator> ();
	}

	public void Defend (GameObject attacker, int damage, Vector2 bumpVelocity, float bumpTime)
	{
		animator.SetTrigger ("hit");
		life -= damage;
		if (life <= 0)
			Destroy (this.gameObject);
		else
			animator.SetInteger ("life", life);
	}
}
