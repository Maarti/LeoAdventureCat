using UnityEngine;
using System.Collections;

public class ProjectileController : MonoBehaviour
{
	public float bumpVelocity = 0;
	public bool destroyOnCollision = true;

	GameObject player;
	IDefendable pc;

	// Use this for initialization
	void Start ()
	{
		player = GameObject.Find ("Cat");
		pc = player.GetComponent<IDefendable> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnCollisionEnter2D (Collision2D other)
	{
		Debug.Log ("Proj collision with " + other.transform.name);
		if (other.transform.tag == "Player") {
			pc.Defend (1, bumpVelocity);
			if (destroyOnCollision)
				GameObject.Destroy (this.gameObject);
		}
	}
}
