using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DestroyByBoundary : MonoBehaviour
{
	public string levelToLoad = "level_1_01";

	void OnTriggerExit2D (Collider2D other)
	{
		GameObject obj = other.gameObject;
		if (obj.tag == "Player")
			//obj.transform.position = new Vector3 (0.0f, 0.0f, 0.0f);
			SceneManager.LoadScene (levelToLoad);
		else
			Destroy (other.gameObject);
	}

}