using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	public float yMin = 1, yMax = 6.8f, xMin = 2, xMax = 90;

	Vector3 offset;
	GameObject player;

	// Use this for initialization
	void Start ()
	{
		player = GameObject.FindWithTag ("Player");
		//offset = transform.position - player.transform.position;
		offset = new Vector3 (0.16f, -0.36f, -10.5f);
	}
	
	// LateUpdate is called after Update
	void LateUpdate ()
	{
		// Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
		Vector3 newPosition = player.transform.position + offset;
		if (newPosition.y < yMin)
			newPosition.y = yMin;
		else if (newPosition.y > yMax)
			newPosition.y = yMax;
		if (newPosition.x < xMin)
			newPosition.x = xMin;
		else if (newPosition.x > xMax)
			newPosition.x = xMax;
		transform.position = newPosition;

	}
}