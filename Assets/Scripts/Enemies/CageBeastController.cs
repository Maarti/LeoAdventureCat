using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageBeastController : MonoBehaviour
{
	public int damageOnCollision = 1;
	public Vector2 bumpOnCollision = new Vector2 (-2, 3);

	bool readyToAttack = false;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void OnTriggerStay2D (Collider2D other)
	{
		if (readyToAttack && other.transform.tag == "Player")
			other.gameObject.GetComponent<PlayerController> ().Defend (this.gameObject, damageOnCollision, bumpOnCollision, 0.5f);
	}

	public void isReadyToAttack ()
	{
		this.readyToAttack = true;
	}

	public void isNotReadyToAttack ()
	{
		this.readyToAttack = false;
	}
}
