using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour, IDefendable
{

	public Transform scratchPrefab;
	public float speed = 10, jumpVelocity = 10;
	public LayerMask playerMask;
	public bool isGrounded = false, canMoveInAir = true;

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
		bool newisGrounded = Physics2D.OverlapArea (checkGroundTop.position, checkGroundBottom.position, playerMask);
		if (!isGrounded && newisGrounded) // landing
			isBumped = false;
		isGrounded = newisGrounded;
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
			//rb.velocity += jumpVelocity * Vector2.up;
			rb.velocity = jumpVelocity * Vector2.up;
			if (Random.value > 0.85f)	//play sound (15% chance)
				mouth.Meowing ("jump");
		}
		
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
		Instantiate (scratchPrefab, attackLocation.position, Quaternion.identity);
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

	public void Defend (float damage, float bumpVelocity)
	{
		Debug.Log ("defend");
		if (bumpVelocity > 0) {
			animator.SetTrigger ("jump");
			isBumped = true;
			rb.velocity = (Vector2.up.normalized + Vector2.left.normalized) * bumpVelocity;
		}
	}
}