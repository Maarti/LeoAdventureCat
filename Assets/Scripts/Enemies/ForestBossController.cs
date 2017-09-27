using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestBossController : MonoBehaviour, IDefendable
{
	public float waitingBetweenPhases = 4f;
	public int life = 40;
	public GameObject weapon;
	public Transform nutPrefab;
	int currentPhase = 0, previousPhase = 0;
	bool waitingForNextPhase = false;
	GameObject player;
	Rigidbody2D playerRb;
	Transform projectilePosition;


	// Use this for initialization
	void Start ()
	{
		player = GameObject.FindWithTag ("Player");
		playerRb = player.GetComponent<Rigidbody2D> ();
		projectilePosition = transform.Find ("Body/ProjectilePosition").transform;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (life > 0) {
			if (currentPhase == 0 && !waitingForNextPhase) {
				Invoke ("BeginNextPhase", waitingBetweenPhases);
				waitingForNextPhase = true;
			}

		}
	}

	void BeginNextPhase ()
	{		
		if (previousPhase == 1) {
			this.currentPhase = 2;
			StartCoroutine (ThrowingPhase ());
		} else {
			this.currentPhase = 1;
			StartCoroutine (AspirationPhase ());
		}
		waitingForNextPhase = false;
	}

	IEnumerator AspirationPhase ()
	{
		Debug.Log ("begin AspriationPhase");
		weapon.SetActive (true);
		yield return new WaitForSeconds (5f);
		weapon.SetActive (false);
		currentPhase = 0;
		previousPhase = 1;
		Debug.Log ("end AspriationPhase");
	}

	IEnumerator ThrowingPhase ()
	{
		Debug.Log ("begin ThrowingPhase");
		int nbThrown = 0;
		while (nbThrown < 3) {
			Transform nutProjectile = (Transform)Instantiate (nutPrefab, projectilePosition.position, projectilePosition.rotation);
			nutProjectile.GetComponent<ProjectileController> ().timeToLive = 2f;
			Vector2 nutForce = new Vector2 (1, 3);
			nutForce.x = (player.transform.position.x - nutProjectile.transform.position.x) + playerRb.velocity.x;
			nutProjectile.GetComponent<Rigidbody2D> ().velocity = nutForce;
			nbThrown++;
			yield return new WaitForSeconds (1f);
		}
		currentPhase = 0;
		previousPhase = 2;
		Debug.Log ("end ThrowingPhase");
	}


	// Bump player if touched
	void OnCollisionEnter2D (Collision2D other)
	{
		if (other.transform.tag == "Player")
			other.gameObject.GetComponent<PlayerController> ().Defend (this.gameObject, 1, new Vector2 (-3, 2), 0.5f);
	}

	public void Defend (GameObject attacker, int damage, Vector2 bumpVelocity, float bumpTime)
	{
		this.life -= damage;
		if (life <= 0)
			GameObject.Destroy (this.gameObject);
		Debug.Log ("boss attacked, life = " + life);
	}
}
