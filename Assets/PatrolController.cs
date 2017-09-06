using UnityEngine;
using System.Collections;

public class PatrolController : MonoBehaviour
{
	public LayerMask layerMask, playerMask;
	public float speed = 1, lineOfSight = 0;

	Rigidbody2D rb;
	bool facingRight = false;
	Transform groundCheck, startLoS;


	// Use this for initialization
	void Start ()
	{
		rb = GetComponent<Rigidbody2D> ();
		groundCheck = GameObject.Find (this.name + "/ground_check").transform;
		startLoS = GameObject.Find (this.name + "/lineOfSight").transform;
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Detecting Ground
		if (!Physics2D.Raycast (transform.position, groundCheck.position - transform.position, 1, layerMask))
			Flip ();
		Debug.DrawRay (transform.position, groundCheck.position - transform.position, Color.black);

		// Moving
		Vector2 moveVel = rb.velocity;
		moveVel.x = (facingRight) ? speed : -speed;
		rb.velocity = moveVel;

		// Detecting player in line of sight
		if (lineOfSight > 0) {
			Vector2 direction = (facingRight) ? Vector2.right : Vector2.left;
			if (Physics2D.Raycast (startLoS.position, direction, lineOfSight, playerMask))
				Debug.Log ("attack");
			Debug.DrawRay (startLoS.position, direction * lineOfSight, Color.red);
		}
		
	}

	void Flip ()
	{
		if (rb.velocity.y == 0) { // don't flip if in the air
			facingRight = !facingRight;
			Vector3 theScale = new Vector3 (transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
			transform.localScale = theScale;
			rb.velocity = Vector2.zero;
		}
	}
}
