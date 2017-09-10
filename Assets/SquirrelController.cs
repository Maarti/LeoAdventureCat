using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelController : MonoBehaviour, IAttackable
{
	public Transform nutPrefab;
	public Vector2[] nutForces;
	public float nutRespawnTime = 2f;
	public bool aimPlayer = true;

	bool readyToAttack = true;
	GameObject player;
	Rigidbody2D playerRb;
	Vector2 nutForce;
	int currentNut = 0;


	// Use this for initialization
	void Start ()
	{
		player = GameObject.FindWithTag ("Player");
		playerRb = player.GetComponent<Rigidbody2D> ();
		nutForce = nutForces [0];
	}

	void Update ()
	{
		if (readyToAttack)
			Attack ();
	}

	public void Attack ()
	{
		readyToAttack = false;
		//animator.SetTrigger ("attack");
		ThrowNut ();
		if (!aimPlayer) {
			currentNut++;
			if (currentNut > nutForces.Length - 1)
				currentNut = 0;
			nutForce = nutForces [currentNut];
		}
	}


	void ThrowNut ()
	{
		Transform nutProjectile = (Transform)Instantiate (nutPrefab, transform.position, transform.rotation);
		if (aimPlayer)
			nutForce.x = (player.transform.position.x - this.transform.position.x) + playerRb.velocity.x;
		nutProjectile.GetComponent<Rigidbody2D> ().velocity = nutForce;
		//GameObject.Destroy (nutProjectile.gameObject, nutTimeToLive);
		StartCoroutine (RespawnNut (nutRespawnTime));
	}

	IEnumerator RespawnNut (float time)
	{
		yield return new WaitForSeconds (time); // waiting time before respawning dart
		readyToAttack = true; // ready to attack again
	}
	

}
