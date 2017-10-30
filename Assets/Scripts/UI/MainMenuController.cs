using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Exit if Android back button is pressed
		if (Input.GetKeyDown (KeyCode.Escape))
			Application.Quit (); 
	}
}
