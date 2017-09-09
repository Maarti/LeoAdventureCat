using UnityEngine;
using System.Collections;

public abstract class AbstractEnemy : MonoBehaviour, IAttackable, ISentinel, IPatroller, IDefendable
{

	public LayerMask groundLayer, playerLayer;
	public float speed = 1, lineOfSight = 2, damageOnCollision = 1, life = 2;
	// 0 = no bump taken
	public float selfBumpMultiplier = 0f;
	public Vector2 bumpOnCollision = new Vector2 (-2, 3);
	public bool isAtacking = false;

	protected bool facingRight = false;
	protected Rigidbody2D rb;
	protected Transform groundCheck, startLoS, wallCheckTop, wallCheckBottom;
	protected bool isMoving = true, isBumped = false;


	// Use this for initialization
	protected virtual void Start ()
	{
		rb = GetComponent<Rigidbody2D> ();
		groundCheck = transform.Find ("groundCheck").transform;
		startLoS = transform.Find ("lineOfSight").transform;
		wallCheckTop = transform.Find ("wallCheckTop").transform;
		wallCheckBottom = transform.Find ("wallCheckBottom").transform;
	}

	// Update is called once per frame
	protected virtual void Update ()
	{
		if (isMoving && !isBumped)
			Patrol ();
		if (!isAtacking && CheckLoS ())
			Attack ();
	}

	public GameObject CheckLoS ()
	{
		// Detecting player in line of sight
		if (lineOfSight > 0) {
			Vector2 direction = (facingRight) ? Vector2.right : Vector2.left;
			RaycastHit2D ray = Physics2D.Raycast (startLoS.position, direction, lineOfSight, playerLayer);
			Debug.DrawRay (startLoS.position, direction * lineOfSight, Color.red);
			if (ray)
				return ray.collider.gameObject;
		}
		return null;
	}

	public virtual void Attack ()
	{
		isMoving = false;
		isAtacking = true;
	}

	public void Patrol ()
	{
		// Detecting Ground
		//if (!Physics2D.Raycast (transform.position, groundCheck.position - transform.position, 1, groundLayer))
		if (!detectGround () || detectWall ())
			Flip ();
		Debug.DrawLine (transform.position, groundCheck.position, Color.black);

		// Moving
		Vector2 moveVel = rb.velocity;
		moveVel.x = (facingRight) ? speed : -speed;
		rb.velocity = moveVel;
	}

	protected void Flip ()
	{
		if (rb.velocity.y == 0) { // don't flip if in the air
			facingRight = !facingRight;
			Vector3 theScale = new Vector3 (transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
			transform.localScale = theScale;
			rb.velocity = Vector2.zero;
		}
	}

	// Bump player if touched
	void OnCollisionEnter2D (Collision2D other)
	{
		if (other.transform.tag == "Player")
			other.gameObject.GetComponent<PlayerController> ().Defend (this.gameObject, damageOnCollision, bumpOnCollision, 0.5f);
		else if (other.gameObject.name == "Scratch")
			Debug.Log (this.name + " OnCollis with " + other.gameObject.name);
	}

	RaycastHit2D detectGround ()
	{
		return Physics2D.Linecast (transform.position, groundCheck.position, groundLayer);
	}

	RaycastHit2D detectWall ()
	{
		RaycastHit2D ray;
		if (ray = Physics2D.Linecast (transform.position, wallCheckTop.position, groundLayer))
			return ray;
		else if (ray = Physics2D.Linecast (transform.position, wallCheckBottom.position, groundLayer))
			return ray;
		else
			return default(RaycastHit2D);
	}

	public virtual void Defend (GameObject attacker, float damage, Vector2 bumpVelocity, float bumpTime)
	{
		Debug.Log (gameObject.name + " hit for " + damage + " dmg !");
		this.life -= damage;
		if (life <= 0) {
			GameObject.Destroy (this.gameObject);
			return;
		}
		if (selfBumpMultiplier > 0 && bumpVelocity != Vector2.zero) {
			if ((transform.position.x - attacker.transform.position.x) > 0) // if attacker come from the left, bump to right
				bumpVelocity.x *= -1;
			bumpVelocity *= this.selfBumpMultiplier;
			//animator.SetTrigger ("hit");
			rb.velocity = bumpVelocity;
			StartCoroutine (BeingBump (bumpTime * selfBumpMultiplier));
		}
	}

	IEnumerator BeingBump (float timeBeingBumped)
	{
		isBumped = true;
		yield return new WaitForSeconds (timeBeingBumped);
		isBumped = false;
	}



}
