using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DestroyByBoundary : MonoBehaviour
{
	void OnTriggerExit2D (Collider2D other)
	{
		GameObject obj = other.gameObject;
		if (obj.tag == "Player") {
			PlayerController pc = other.GetComponent<PlayerController> ();
			pc.Defend (gameObject, pc.life, Vector2.zero, 0f);
		} else if (obj.tag == "AoE")
			return;
		else
			Destroy (other.gameObject);
	}

}