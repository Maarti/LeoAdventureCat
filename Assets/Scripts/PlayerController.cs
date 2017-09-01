using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{


	bool facingRight = false;
	private Rigidbody2D rb;
	private Animator animator;

	public float speed = 10, jumpVelocity = 10;
	public LayerMask playerMask;
	public bool canMoveInAir = true;
	Transform checkGroundTop, checkGroundBottom;
	public bool isGrounded = false;
	float hInput = 0;

	void Awake ()
	{
		// Get the animator
		animator = GetComponent<Animator> ();

	}

	void Start ()
	{
		rb = GetComponent<Rigidbody2D> ();
		//tagGround = GameObject.Find (this.name + "/tag_ground").transform;
		checkGroundTop = GameObject.Find (this.name + "/ground_check_top").transform;
		checkGroundBottom = GameObject.Find (this.name + "/ground_check_bottom").transform;
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
	}

	void FixedUpdate ()
	{
		//isGrounded = Physics2D.Linecast (myTrans.position, tagGround.position, playerMask);
		isGrounded = Physics2D.OverlapArea (checkGroundTop.position, checkGroundBottom.position, playerMask);
		animator.SetBool ("isGrounded", isGrounded);
		animator.SetFloat ("y.velocity", rb.velocity.y);

	}

	void Move (float horizonalInput)
	{
		if (!canMoveInAir && !isGrounded)
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
			rb.velocity += jumpVelocity * Vector2.up;
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
}