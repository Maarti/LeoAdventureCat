using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour, IDefendable
{

	public Transform scratchPrefab;
	public float speed = 10, jumpVelocity = 10;
	public LayerMask playerMask;
	public bool isGrounded = false, canMoveInAir = true;
	public Vector2 offensiveBumpVelocity = new Vector2 (-1.5f, 0.5f);
	public int life = 5, damage = 1;

	const int lifeMax = 10;
	float hInput = 0;
	bool facingRight = false, isBumped = false;
	Rigidbody2D rb;
	Animator animator;
	Transform checkGroundTop, checkGroundBottom, attackLocation;
	MouthController mouth;

	void Awake ()
	{
		animator = GetComponent<Animator> ();
	}

	void Start ()
	{
		rb = GetComponent<Rigidbody2D> ();
		checkGroundTop = GameObject.Find (this.name + "/ground_check_top").transform;
		checkGroundBottom = GameObject.Find (this.name + "/ground_check_bottom").transform;
		attackLocation = GameObject.Find (this.name + "/AttackLocation").transform;
		mouth = GameObject.Find (this.name + "/Body/Head/Mouth").GetComponent<MouthController> ();
	}

	void Update ()
	{
		#if !UNITY_ANDROID && !UNITY_IPHONE && !UNITY_BLACKBERRY && !UNITY_WINRT || UNITY_EDITOR
		Move (Input.GetAxisRaw ("Horizontal"));
		if (Input.GetButtonDown ("Jump"))
			Jump ();
		else if (Input.GetButtonUp ("Jump"))
			StopJump ();
		#else
			Move (hInput);
		#endif

		#if !UNITY_ANDROID && !UNITY_IPHONE && !UNITY_BLACKBERRY && !UNITY_WINRT || UNITY_EDITOR
		if (Input.GetButtonDown ("Fire1"))
			Attack ();
		#endif
	}

	void FixedUpdate ()
	{
		isGrounded = Physics2D.OverlapArea (checkGroundTop.position, checkGroundBottom.position, playerMask);
		animator.SetBool ("isGrounded", isGrounded);
		animator.SetFloat ("y.velocity", rb.velocity.y);

	}

	void Move (float horizonalInput)
	{
		if (!canMoveInAir && !isGrounded)
			return;

		if (isBumped)
			return;

		Vector2 moveVel = rb.velocity;
		moveVel.x = horizonalInput * speed;
		rb.velocity = moveVel;

		// Update animator
		animator.SetFloat ("speed", Mathf.Abs (horizonalInput));

		// Flip if direction changed
		if (horizonalInput > 0 && !facingRight)
			Flip ();
		else if (horizonalInput < 0 && facingRight)
			Flip ();
	}

	public void Jump ()
	{
		if (isGrounded) {
			animator.SetTrigger ("jump");
			rb.velocity = jumpVelocity * Vector2.up;
			if (Random.value > 0.87f)	//play sound (13% chance)
				mouth.Meowing ("jump");
		}
		
	}

	public void StopJump ()
	{
		if (!isGrounded && rb.velocity.y > 0)
			rb.velocity /= 2;
	}

	public void StartMoving (float horizonalInput)
	{
		hInput = horizonalInput;
	}

	void Flip ()
	{
		facingRight = !facingRight;
		Vector3 theScale = new Vector3 (transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
		transform.localScale = theScale;
	}

	public void Attack ()
	{
		string[] attacksAnim = { "attack01", "attack02" };
		string randomAttackAnim = attacksAnim [Random.Range (0, attacksAnim.Length)];
		animator.SetTrigger (randomAttackAnim);
		Transform attack = (Transform)Instantiate (scratchPrefab, attackLocation.position, Quaternion.identity);
		attack.gameObject.GetComponent<ScratchController> ().Init (this.damage, this.offensiveBumpVelocity, 0.35f);
	}

	void OnCollisionEnter2D (Collision2D other)
	{
		if (other.transform.tag == "MovingPlatform")
			transform.parent = other.transform;
	}

	void OnCollisionExit2D (Collision2D other)
	{
		if (other.transform.tag == "MovingPlatform")
			transform.parent = null;
	}

	public void Defend (GameObject attacker, int damage, Vector2 bumpVelocity, float bumpTime)
	{
		if (bumpVelocity != Vector2.zero) {
			if ((transform.position.x - attacker.transform.position.x) > 0) // if attacker come from the left, bump to right
				bumpVelocity.x *= -1;
			animator.SetTrigger ("jump");
			rb.velocity = bumpVelocity;
			StartCoroutine (BeingBump (bumpTime));
		}
		GetInjured (damage);
		mouth.Meowing ("hit");
	}

	IEnumerator BeingBump (float timeBeingBumped)
	{
		isBumped = true;
		yield return new WaitForSeconds (timeBeingBumped);
		isBumped = false;
	}

	void GetInjured (int dmg)
	{
		dmg = Mathf.Clamp (dmg, 0, life);
		this.life -= dmg;
		GameController.gc.PlayerInjured (dmg);
		if (this.life <= 0)
			GameController.gc.GameOver ();
	}

	public void CollectKittyz (int amount = 1)
	{
		GameController.gc.CollectKittyz (amount);
	}
}