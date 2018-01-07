
using UnityEngine;

public class MainMenuController : MonoBehaviour
{

	void Update ()
	{
		// Exit if Android back button is pressed
		if (Input.GetKeyDown (KeyCode.Escape))
			Application.Quit (); 
	}
}
