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
	GameObject player;

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
        
        // Floors managing
        if (floors.Length > 0) { 
            foreach (Floor floor in floors) {
			    if (yPlayer < floor.playerYMax) {
				    if (currentFloor != floor) {
					    currentFloor = floor;
				    }
				    break;
			    }
		    }
		    if (newPosition.y < currentFloor.yMin)
			    newPosition.y = currentFloor.yMin;
		    else if (newPosition.y > currentFloor.yMax)
			    newPosition.y = currentFloor.yMax;
        }

        // Borders managing
		if (newPosition.x < xMin)
			newPosition.x = xMin;
		else if (newPosition.x > xMax)
			newPosition.x = xMax;
        if (newPosition.y < yMin)
            newPosition.y = yMin;
        else if (newPosition.y > yMax)
            newPosition.y = yMax;
        
        // Move camera smoothly
        this.transform.position = Vector3.Lerp(this.transform.position, newPosition, smoothingInterpolateTime);

        // Move the camera instantly if it is out of scope
        bool instantMove = false;
        if (this.transform.position.x < xMin || this.transform.position.x > xMax ||
            this.transform.position.y < yMin || this.transform.position.y > yMax)
            instantMove = true;
        if (instantMove)
            this.transform.position = newPosition;
    }


	[Serializable]
	public class Floor
	{
		// playerYMax = max Y position of the player for this floor
		// yMin = min Y position of the camera for this floor
		// yMAx= max Y position of the camera for this floor
		public float playerYMax, yMin, yMax;
	}
}