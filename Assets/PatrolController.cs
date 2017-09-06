using UnityEngine;
using System.Collections;

public class PatrolController : MonoBehaviour
{
	public LayerMask layerMask;
	public float speed=1, lineOfSight = 0;

	Rigidbody2D rb;
	bool facingRight = false;
	Transform groundCheck;

	// Use this for initialization
	void Start ()
	{
		rb = GetComponent<Rigidbody2D> ();
		groundCheck = GameObject.Find (this.name + "/ground_check").transform;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!Physics2D.Raycast (transform.position, groundCheck.position - transform.position, 1, layerMask);)
			Flip ();
		Debug.DrawRay (transform.position, groundCheck.position - transform.position, Color.black);

		Vector2 moveVel = rb.velocity;
		moveVel.x = (facingRight) ? speed : -speed;
		rb.velocity = moveVel;
		
	}

	void Flip ()
	{
		facingRight = !facingRight;
		Vector3 theScale = new Vector3 (transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
		transform.localScale = theScale;
		rb.velocity = Vector2.zero;
	}
}
