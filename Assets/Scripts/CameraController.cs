using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

	public GameObject player;
	public float yMin = 1;
	//Public variable to store a reference to the player game object
	private Vector3 offset;
	//Private variable to store the offset distance between the player and camera

	// Use this for initialization
	void Start ()
	{
		//Calculate and store the offset value by getting the distance between the player's position and camera's position.
		offset = transform.position - player.transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate ()
	{
		// Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
		Vector3 newPosition = player.transform.position + offset;
		if (newPosition.y < yMin)
			newPosition.y = yMin;
		transform.position = newPosition;

	}
}