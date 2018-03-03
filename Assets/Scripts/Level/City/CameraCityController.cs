using UnityEngine;

public class CameraCityController : MonoBehaviour
{
	public float xMin = 6;
	public Vector3 offset = new Vector3 (5f, 0f, -10f);
    public GameObject player;
    	
	// LateUpdate is called after Update
	void LateUpdate ()
	{
		// Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
		Vector3 newPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z) + offset;
  
        // Borders managing
		if (newPosition.x < xMin)
			newPosition.x = xMin;

        // Move camera smoothly
        //this.transform.position = Vector3.Lerp(this.transform.position, newPosition, smoothingInterpolateTime);
        this.transform.position = newPosition;

    }

}