using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointObjectController : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player") {
			CheckPointController.cc.Check (this.gameObject.name);
		}
	}
}
