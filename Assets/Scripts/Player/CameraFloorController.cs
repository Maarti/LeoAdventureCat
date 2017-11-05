using UnityEngine;
using System.Collections;
using System;

public class CameraFloorController : MonoBehaviour
{
	public float yMin = 1.5f, yMax = 6.8f, xMin = 6, xMax = 90;
	public Vector3 offset = new Vector3 (0f, 0f, -10.5f);
	public Floor[] floors;

	float smoothingInterpolateTime = 0.1f;
	Floor currentFloor;
	//bool isMovingSmoothly = false;
	GameObject player;

	// Use this for initialization
	void Start ()
	{
		player = GameObject.FindWithTag ("Player");
	}
	
	// LateUpdate is called after Update
	void LateUpdate ()
	{
		// Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
		Vector3 newPosition = player.transform.position + offset;
		float yPlayer = player.transform.position.y;
		foreach (Floor floor in floors) {
			if (yPlayer < floor.playerYMax) {
				if (currentFloor != floor) {
					currentFloor = floor;
					//StartCoroutine (MoveSmoothly (1f));
				}
				break;
			}
		}

		if (newPosition.y < currentFloor.yMin)
			newPosition.y = currentFloor.yMin;
		else if (newPosition.y > currentFloor.yMax)
			newPosition.y = currentFloor.yMax;
		if (newPosition.x < xMin)
			newPosition.x = xMin;
		else if (newPosition.x > xMax)
			newPosition.x = xMax;
		//if (isMovingSmoothly)
		this.transform.position = Vector3.Lerp (this.transform.position, newPosition, smoothingInterpolateTime);
		//else
		//	this.transform.position = newPosition;

	}

	/*IEnumerator MoveSmoothly (float time)
	{
		isMovingSmoothly = true;
		yield return new WaitForSeconds (time);
		isMovingSmoothly = false;
	}*/

	[Serializable]
	public class Floor
	{
		// playerYMax = max Y position of the player for this floor
		// yMin = min Y position of the camera for this floor
		// yMAx= max Y position of the camera for this floor
		public float playerYMax, yMin, yMax;
	}
}