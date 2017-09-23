using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelController : MonoBehaviour, IAttackable
{
	public Transform nutPrefab;
	public Vector2[] nutForces;
	public float nutRespawnTime = 2f, nutTimeToLive = 3f;
	public bool aimPlayer = true;
	public LayerMask playerLayer;

	bool readyToAttack = true;
	GameObject player;
	Rigidbody2D playerRb;
	Vector2 nutForce;
	int currentNut = 0;
	Transform losTop, losBottom, nutTransform;
	Animator animator;

	// Use this for initialization
	void Start ()
	{
		player = GameObject.FindWithTag ("Player");
		playerRb = player.GetComponent<Rigidbody2D> ();
		nutForce = nutForces [0];
		losTop = transform.Find ("lineOfSightTop").transform;
		losBottom = transform.Find ("lineOfSightBottom").transform;
		nutTransform = transform.Find ("Body").Find ("HazelNut").transform;
		animator = GetComponent<Animator> ();
	}

	void Update ()
	{
		if (readyToAttack && Physics2D.OverlapArea (losTop.position, losBottom.position, playerLayer))
			Attack ();
	}

	public void Attack ()
	{
		readyToAttack = false;
		animator.SetTrigger ("attack");
		//ThrowNut ();
		if (!aimPlayer) {
			currentNut++;
			if (currentNut > nutForces.Length - 1)
				currentNut = 0;
			nutForce = nutForces [currentNut];
		}
	}

	// called by animator
	void ThrowNut ()
	{
		Transform nutProjectile = (Transform)Instantiate (nutPrefab, nutTransform.position, nutTransform.rotation);
		nutProjectile.GetComponent<ProjectileController> ().timeToLive = nutTimeToLive;
		if (aimPlayer)
			nutForce.x = (player.transform.position.x - nutProjectile.transform.position.x) + playerRb.velocity.x;
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
