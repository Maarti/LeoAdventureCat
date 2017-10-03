using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{

	public Transform moveToPosition;

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.transform.tag == "Player") {
			other.transform.position = moveToPosition.position;
		}
	}
}
