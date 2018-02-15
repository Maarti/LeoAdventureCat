using System.Collections;
using UnityEngine;

public class SquirrelController : MonoBehaviour, IAttackable
{
	public Transform nutPrefab;
	public Vector2[] nutForces;
	public float nutRespawnTime = 2f, nutTimeToLive = 3f;
    public bool aimPlayer = true;           // targets the player (taking into account his movement)
    public bool aimPlayerPosition = false;  // target the player position only (set aimPlayer to true also)
	public LayerMask playerLayer;

	bool readyToAttack = true;
	GameObject player;
	Rigidbody2D playerRb;
	Vector2 nutForce;
	int currentNut = 0;
	Transform losTop, losBottom, nutTransform;
	Animator animator;
	AudioSource audioSource;

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
		audioSource = GetComponent<AudioSource> ();
        aimPlayer = (aimPlayerPosition) ? true : aimPlayer; // retrocompatibility
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
		if (aimPlayer) {
            // aim at the current player position
            nutForce.x = player.transform.position.x - nutProjectile.transform.position.x;
            // aim at the current player position + forecast his movement
            if (!aimPlayerPosition)
			    nutForce.x += playerRb.velocity.x;
        }
        nutProjectile.GetComponent<Rigidbody2D> ().velocity = nutForce;
		//GameObject.Destroy (nutProjectile.gameObject, nutTimeToLive);
		audioSource.Play ();
		StartCoroutine (RespawnNut (nutRespawnTime));
	}

	IEnumerator RespawnNut (float time)
	{
		yield return new WaitForSeconds (time); // waiting time before respawning dart
		readyToAttack = true; // ready to attack again
	}
	

}
