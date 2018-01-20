using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DogCatcherController : MonoBehaviour, IDefendable
{
	public float waitingBetweenPhases = 4f;
	public int life = 40;
	public Transform nutPrefab;
	public AudioClip throwSound, shopVacSound, bossHitSound;
    public Slider lifebar;

	int currentPhase = 0, previousPhase = 0;
	bool waitingForNextPhase = false, facingRight = false;
	GameObject player, attackPointEffector;
	Rigidbody2D playerRb;
	Transform projectilePosition;
	Animator animator;
	float lastTimeCollision = -2f;
	AudioSource audioSource;


	// Use this for initialization
	void Start ()
	{
		player = GameObject.FindWithTag ("Player");
		playerRb = player.GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator> ();
		projectilePosition = transform.Find ("Body/Arm2/ProjectilePosition");
		attackPointEffector = transform.Find ("Body/Weapon/AttackPointEffector").gameObject;
		attackPointEffector.SetActive (false);
		audioSource = GetComponent<AudioSource> ();
        lifebar.maxValue = life;
        lifebar.value = life;
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
		animator.SetTrigger ("absorb");
		yield return new WaitForSeconds (5f);
		StopAbsorb ();
		currentPhase = 0;
		previousPhase = 1;
	}

	//called by animator
	void StartAbsorb ()
	{
		animator.SetBool ("isAbsorbing", true);
		attackPointEffector.SetActive (true);
		audioSource.PlayOneShot (shopVacSound);
	}

	public void StopAbsorb ()
	{
		animator.SetBool ("isAbsorbing", false);
		attackPointEffector.SetActive (false);
		Invoke ("StopAudio", 1f);
	}

	IEnumerator ThrowingPhase ()
	{
		int nbThrown = 0;
		while (nbThrown < 3) {
			animator.SetTrigger ("throw");
			nbThrown++;
			yield return new WaitForSeconds (1f);
		}
		currentPhase = 0;
		previousPhase = 2;
	}

	// called by animator
	void ThrowPojectile ()
	{
		Transform nutProjectile = (Transform)Instantiate (nutPrefab, projectilePosition.position, projectilePosition.rotation);
		nutProjectile.GetComponent<ProjectileController> ().timeToLive = 2f;
		Vector2 nutForce = new Vector2 (1, 3);
		nutForce.x = (player.transform.position.x - nutProjectile.transform.position.x) + playerRb.velocity.x;
		nutProjectile.GetComponent<Rigidbody2D> ().velocity = nutForce;
		audioSource.PlayOneShot (throwSound, 0.5f);
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
			else {
				animator.SetTrigger ("hit");
				if (!audioSource.isPlaying)
					audioSource.PlayOneShot (bossHitSound);
			}
            lifebar.value = life;
		}
	}

	void Die ()
	{
		StopAbsorb ();
		//StopCoroutine ("ThrowingPhase");
		StopAllCoroutines ();
		animator.SetTrigger ("die");
		Physics2D.IgnoreCollision (GetComponent<Collider2D> (), player.GetComponent<Collider2D> ());
		audioSource.Stop ();
		audioSource.pitch = 0.8f;
		audioSource.PlayOneShot (bossHitSound);
        lifebar.gameObject.SetActive(false);
        // Achievement
        PlayGamesScript.UnlockAchievement(Config.DEFEAT_DOGCATCHER);
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

	public void StopAudio ()
	{
		audioSource.Stop ();
	}
}
