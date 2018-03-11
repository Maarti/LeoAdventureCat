using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour, IDefendable, IGlider, IKittyzCollecter
{
	public Transform scratchPrefab, landingSmokePrefab;
	public float speed = 10, jumpVelocity = 10;
	public LayerMask playerMask;
	public bool isGrounded = false, canMoveInAir = true, freeze = false;
	public Vector2 offensiveBumpVelocity = new Vector2 (-1.5f, 0.5f);
	public int life = 3, damage = 1;
	public Vector3 hangGliderPosition;

	const int lifeMax = 10;
    #if UNITY_ANDROID || UNITY_IOS
    float hInput = 0;
    #endif
	float timeBeingInvincible = 1.5f;
	bool facingRight = false, isBumped = false, isInvincible = false;
	Rigidbody2D rb;
	Animator animator;
	Transform checkGroundTop, checkGroundBottom, attackLocation, landingSmokeLocation;
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
        landingSmokeLocation = GameObject.Find(this.name + "/LandingSmokeLocation").transform;
        life = ApplicationController.ac.playerData.max_life;
	}

	void Update ()
	{
#if !UNITY_ANDROID && !UNITY_IPHONE && !UNITY_BLACKBERRY && !UNITY_WINRT || UNITY_EDITOR
        if(life>0 && Time.timeScale > 0f && !freeze){
            if (Input.GetButtonDown ("Jump"))
			    Jump ();
		    else if (Input.GetButtonUp ("Jump"))
			    StopJump ();
		    if (Input.GetButtonDown ("Attack"))
			    Attack ();
        }
#endif
	}

	void FixedUpdate ()
	{
#if !UNITY_ANDROID && !UNITY_IPHONE && !UNITY_BLACKBERRY && !UNITY_WINRT || UNITY_EDITOR
        if (life > 0 && !freeze)
            Move(Input.GetAxisRaw("Horizontal"));
        else
            Move(0f);
#else
		Move (hInput);
#endif

        CheckGround();
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

    void CheckGround()
    {
        bool newIsGrounded = Physics2D.OverlapArea(checkGroundTop.position, checkGroundBottom.position, playerMask);
        
        // Pop some smoke when landing for juicy effect
        if (this.isGrounded==false && newIsGrounded == true) { 
            Transform smoke = (Transform)Instantiate(landingSmokePrefab, landingSmokeLocation.position, Quaternion.identity);
            Destroy(smoke.gameObject, 1f);
        }

        isGrounded = newIsGrounded;
        animator.SetBool("isGrounded", isGrounded);
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
        #if UNITY_ANDROID || UNITY_IOS
		hInput = horizonalInput;
        #endif
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
		if (life <= 0) // don't take damge when dead
			return;
		if (!isInvincible || attacker.name == "Boundary") {
			if (bumpVelocity != Vector2.zero) {
				if ((transform.position.x - attacker.transform.position.x) > 0) // if attacker come from the left, bump to right
				bumpVelocity.x *= -1;
				animator.SetTrigger ("jump");
				rb.velocity = bumpVelocity;
				StartCoroutine (BeingBump (bumpTime));
			}
			GetInjured (damage);
			if (life > 0)
				StartCoroutine (BeingInvincible ());
		}
	}

	IEnumerator BeingBump (float timeBeingBumped)
	{
		isBumped = true;
		yield return new WaitForSeconds (timeBeingBumped);
		isBumped = false;
	}

	IEnumerator BeingInvincible ()
	{
		isInvincible = true;
		float t = 0f;
		float lastBlink = 0f;
		while (t < timeBeingInvincible) {
			t += Time.deltaTime;
			if (lastBlink > 0.05f) { //blinking speed
				lastBlink = 0f;
				foreach (SpriteRenderer sprite in transform.GetComponentsInChildren<SpriteRenderer>()) {
					if (sprite.gameObject.name != "Mouth")
						sprite.enabled = !sprite.enabled;
				}
			} else {
				lastBlink += Time.deltaTime;
			}
			yield return null;
		}
		foreach (SpriteRenderer sprite in transform.GetComponentsInChildren<SpriteRenderer>()) {
			if (sprite.gameObject.name != "Mouth")
				sprite.enabled = true;
		}
		isInvincible = false;
	}

	void GetInjured (int dmg)
	{
		dmg = Mathf.Clamp (dmg, 0, life);
		this.life -= dmg;
		GameController.gc.PlayerInjured (dmg);
		if (this.life <= 0) {
			Die ();
		} else
			mouth.Meowing ("hit");
	}

	void Die ()
	{
		mouth.Meowing ("die");
		animator.SetBool ("isDead", true);
		animator.SetTrigger ("die"); // trigger + bool to prevent animator to play death multiple times
		GameController.gc.GameOver ();
		StartMoving (0f);
	}

	public void CollectKittyz (int amount = 1)
	{
		GameController.gc.CollectKittyz (amount);
	}

	public Vector3 GetHangGliderPosition ()
	{
		return this.hangGliderPosition;
	}
}