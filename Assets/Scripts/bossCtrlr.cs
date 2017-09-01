using UnityEngine;
using System.Collections;

public class bossCtrlr : MonoBehaviour
{
	private Animator animator;

	void Awake ()
	{


		// Get the animator
		animator = GetComponent<Animator> ();

	}

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnTriggerEnter2D (Collider2D otherCollider2D)
	{

		// Change animation
		animator.SetTrigger ("Hit");

	}
}
