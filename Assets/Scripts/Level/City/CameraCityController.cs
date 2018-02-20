using UnityEngine;

public class CameraCityController : MonoBehaviour
{
	public float xMin = 6/*, xMax = 999yMin = 1.5f, yMax = 6.8f*/;
	public Vector3 offset = new Vector3 (5f, 0f, -10f);
    public GameObject player;

    float smoothingInterpolateTime = 0.1f;
	
	// LateUpdate is called after Update
	void LateUpdate ()
	{
		// Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
		Vector3 newPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z) + offset;
		float yPlayer = player.transform.position.y;
        

        // Borders managing
		if (newPosition.x < xMin)
			newPosition.x = xMin;
        /*else if (newPosition.x > xMax)
			newPosition.x = xMax;
        if (newPosition.y < yMin)
            newPosition.y = yMin;
        else if (newPosition.y > yMax)
            newPosition.y = yMax;*/

        // Move camera smoothly
        //this.transform.position = Vector3.Lerp(this.transform.position, newPosition, smoothingInterpolateTime);
        this.transform.position = newPosition;

        // Move the camera instantly if it is out of scope
        /* bool instantMove = false;
         if (this.transform.position.x < xMin || this.transform.position.x > xMax ||
             this.transform.position.y < yMin || this.transform.position.y > yMax)
             instantMove = true;
         if (instantMove)
             this.transform.position = newPosition;*/
    }

}