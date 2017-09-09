using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	public float yMin = 1, xMin = 2;

	Vector3 offset;
	GameObject player;

	// Use this for initialization
	void Start ()
	{
		player = GameObject.FindWithTag ("Player");
		offset = transform.position - player.transform.position;
	}
	
	// LateUpdate is called after Update
	void LateUpdate ()
	{
		// Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
		Vector3 newPosition = player.transform.position + offset;
		if (newPosition.y < yMin)
			newPosition.y = yMin;
		if (newPosition.x < xMin)
			newPosition.x = xMin;
		transform.position = newPosition;

	}
}