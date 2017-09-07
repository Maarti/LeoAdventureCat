using UnityEngine;
using System.Collections;

public class ProjectileController : MonoBehaviour
{
	public float speed = 1, bumpTime = 0.5f, timeToLive = 3, damage = 1;
	public Vector2 bumpVelocity = new Vector2 (-2, 3);
	public bool destroyOnCollision = true;

	GameObject player;
	IDefendable pc;

	// Use this for initialization
	void Start ()
	{
		player = GameObject.Find ("Cat");
		pc = player.GetComponent<IDefendable> ();
		GameObject.Destroy (this.gameObject, timeToLive);
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		transform.position = new Vector3 (transform.position.x + speed / 10, transform.position.y, transform.position.z);
	}

	void OnCollisionEnter2D (Collision2D other)
	{
		if (other.transform.tag == "Player") {
			pc.Defend (this.gameObject, damage, bumpVelocity, bumpTime);
			if (destroyOnCollision)
				GameObject.Destroy (this.gameObject);
		}
	}
}
