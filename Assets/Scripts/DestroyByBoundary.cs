using UnityEngine;
using System.Collections;

public class DestroyByBoundary : MonoBehaviour
{

	void OnTriggerExit2D (Collider2D other)
	{
		GameObject obj = other.gameObject;
		if (obj.tag == "Player")
			obj.transform.position = new Vector3 (0.0f, 0.0f, 0.0f);
		else
			Destroy (other.gameObject);
	}

}