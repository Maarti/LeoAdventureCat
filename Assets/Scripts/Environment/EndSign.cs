using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSign : MonoBehaviour
{

	public string levelToLoad = "level_1_01";

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void OnTriggerExit2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player")
			SceneManager.LoadScene (levelToLoad);
	}
}
