using UnityEngine;
using System.Collections;

public abstract class AbstractEnemy : MonoBehaviour, IAttackable, ISentinel, IPatroller
{

	public LayerMask groundLayer, playerLayer;
	public float speed = 1, lineOfSight = 0;

	bool facingRight = false;
	Rigidbody2D rb;
	Transform groundCheck, startLoS;

	// Use this for initialization
	protected void Start ()
	{
		rb = GetComponent<Rigidbody2D> ();
		groundCheck = GameObject.Find (this.name + "/groundCheck").transform;
		startLoS = GameObject.Find (this.name + "/lineOfSight").transform;
	}

	// Update is called once per frame
	protected void Update ()
	{
		Patrol ();
		if (CheckLoS ())
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
		Debug.Log ("Abstract Attack");
	}

	public void Patrol ()
	{
		// Detecting Ground
		if (!Physics2D.Raycast (transform.position, groundCheck.position - transform.position, 1, groundLayer))
			Flip ();
		Debug.DrawRay (transform.position, groundCheck.position - transform.position, Color.black);

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
}
