using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogCatcherController : MonoBehaviour, IDefendable
{
	public float waitingBetweenPhases = 4f;
	public int life = 40;
	public Transform nutPrefab;
	int currentPhase = 0, previousPhase = 0;
	bool waitingForNextPhase = false, facingRight = false;
	GameObject player, attackPointEffector;
	Rigidbody2D playerRb;
	Transform projectilePosition;
	Animator animator;
	float lastTimeCollision = -2f;


	// Use this for initialization
	void Start ()
	{
		player = GameObject.FindWithTag ("Player");
		playerRb = player.GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator> ();
		projectilePosition = transform.Find ("Body/Arm2/ProjectilePosition");
		attackPointEffector = transform.Find ("Body/Weapon/AttackPointEffector").gameObject;
		attackPointEffector.SetActive (false);
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
		if (life <= 0)
			return;
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
		animator.SetTrigger ("absorb");
		yield return new WaitForSeconds (5f);
		StopAbsorb ();
		currentPhase = 0;
		previousPhase = 1;
		Debug.Log ("end AspriationPhase");
	}

	//called by animator
	void StartAbsorb ()
	{
		animator.SetBool ("isAbsorbing", true);
		attackPointEffector.SetActive (true);
		Debug.Log ("start Abs");
	}

	public void StopAbsorb ()
	{
		animator.SetBool ("isAbsorbing", false);
		attackPointEffector.SetActive (false);
		Debug.Log ("stop Abs");
	}

	IEnumerator ThrowingPhase ()
	{
		Debug.Log ("begin ThrowingPhase");
		int nbThrown = 0;
		while (nbThrown < 3) {
			animator.SetTrigger ("throw");
			nbThrown++;
			yield return new WaitForSeconds (1f);
		}
		currentPhase = 0;
		previousPhase = 2;
		Debug.Log ("end ThrowingPhase");
	}

	// called by animator
	void ThrowPojectile ()
	{
		Transform nutProjectile = (Transform)Instantiate (nutPrefab, projectilePosition.position, projectilePosition.rotation);
		nutProjectile.GetComponent<ProjectileController> ().timeToLive = 2f;
		Vector2 nutForce = new Vector2 (1, 3);
		nutForce.x = (player.transform.position.x - nutProjectile.transform.position.x) + playerRb.velocity.x;
		nutProjectile.GetComponent<Rigidbody2D> ().velocity = nutForce;
	}


	// Bump player if touched
	void OnCollisionStay2D (Collision2D other)
	{
		if (other.transform.tag == "Player" && Time.time - lastTimeCollision >= 2f) {
			other.gameObject.GetComponent<PlayerController> ().Defend (this.gameObject, 1, new Vector2 (-3, 2), 0.5f);
			lastTimeCollision = Time.time;
		}
	}

	public void Defend (GameObject attacker, int damage, Vector2 bumpVelocity, float bumpTime)
	{
		if (life > 0) {
			this.life -= damage;
			if (life <= 0)
				Die ();
			else
				animator.SetTrigger ("hit");
			//StartCoroutine ("FadeToRed", .25f);
		}
		Debug.Log ("boss attacked, life = " + life);
	}

	void Die ()
	{
		StopAbsorb ();
		//StopCoroutine ("ThrowingPhase");
		StopAllCoroutines ();
		animator.SetTrigger ("die");
		Physics2D.IgnoreCollision (GetComponent<Collider2D> (), player.GetComponent<Collider2D> ());
	}

	public void Flip ()
	{
		facingRight = !facingRight;
		Vector3 theScale = new Vector3 (transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
		transform.localScale = theScale;
	}

	public void Run (bool run = true)
	{
		animator.SetBool ("isRunning", run);
	}
}
