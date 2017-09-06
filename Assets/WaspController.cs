using UnityEngine;
using System.Collections;

public class WaspController : AbstractEnemy
{
	Animator animator;

	void Awake ()
	{
		animator = GetComponent<Animator> ();
	}

	// Use this for initialization
	new void Start ()
	{
		base.Start ();
	}

	// Update is called once per frame
	new void Update ()
	{
		base.Update ();
	}

	public override void Attack ()
	{
		Debug.Log ("Wasp Attack");

	}
}
