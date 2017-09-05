using UnityEngine;
using System.Collections;

public class WaspController : MonoBehaviour
{

	public float range = 3;
	public LayerMask playerLayer;

	Animator animator;
	Transform player;
	bool facingRight = false;

	void Awake ()
	{
		animator = GetComponent<Animator> ();
	}

	// Use this for initialization
	void Start ()
	{
		player = GameObject.Find ("Cat").transform;
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 direction = (facingRight) ? Vector3.right * range : Vector3.left * range;
		if (Physics2D.Raycast (transform.position, direction, playerLayer).collider)
			Debug.Log ("LINE OF SIGHT");
		Debug.DrawRay (transform.position, direction, Color.red);
		
		//Debug.DrawRay (transform.position, Vector3.left, Color.red);
	}
}
