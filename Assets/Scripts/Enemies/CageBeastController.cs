using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageBeastController : MonoBehaviour
{
	public int damageOnCollision = 1;
	public Vector2 bumpOnCollision = new Vector2 (-2, 3);
	public float speed = 1f;

	bool readyToAttack = false;
	Animator animator;

	// Use this for initialization
	void Start ()
	{
		animator = GetComponent<Animator> ();
		animator.SetFloat ("speed", speed);
	}

	void OnTriggerStay2D (Collider2D other)
	{
		if (readyToAttack && other.transform.tag == "Player")
			Attack (other.gameObject);		
	}

	void Attack (GameObject target)
	{
		target.GetComponent<PlayerController> ().Defend (this.gameObject, damageOnCollision, bumpOnCollision, 0.5f);
		animator.SetTrigger ("attack");
	}

	public void isReadyToAttack ()
	{
		this.readyToAttack = true;
	}

	public void isNotReadyToAttack ()
	{
		this.readyToAttack = false;
	}
}
