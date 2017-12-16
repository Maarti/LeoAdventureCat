// https://www.youtube.com/watch?v=QkisHNmcK7Y

using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    // General public parameters
    public float backgroundSize, paralaxSpeed;
    public bool horizontalScroll = true, verticalScroll = false;

    // Vertical public parameters
    public float minCamY, maxCamY, minBkgY, maxBkgY;

    // General private parameters
	Transform cameraTransform;

    // Horizontal private parameters
	Transform[] layers;
	float viewZone = 4, lastCameraX;
	int leftIndex, rightIndex;
    

	void Start ()
	{
		cameraTransform = Camera.main.transform;
		lastCameraX = cameraTransform.position.x;
        if (horizontalScroll)
        {
            layers = new Transform[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
                layers[i] = transform.GetChild(i);

            leftIndex = 0;
            rightIndex = layers.Length - 1;
        }
	}

	private void Update ()
	{
        if (horizontalScroll)
        {
            float deltaX = cameraTransform.position.x - lastCameraX;
            transform.position += Vector3.right * (deltaX * paralaxSpeed);
            lastCameraX = cameraTransform.position.x;

            //Debug.Log ("camera = " + cameraTransform.position.x + " limit = " + (layers [leftIndex].transform.position.x + viewZone));
            if (cameraTransform.position.x < (layers[leftIndex].transform.position.x + viewZone))
                ScrollLeft();

            if (cameraTransform.position.x > (layers[rightIndex].transform.position.x - viewZone))
                ScrollRight();
        }

        if (verticalScroll)
        {
            // Compute the camera Y ratio
            float ratio = (cameraTransform.position.y - minCamY) / (maxCamY - minCamY);

            // Apply the ratio to the background Y
            float newBkgY = (maxBkgY - minBkgY) * ratio + minBkgY;
            transform.position = new Vector3(transform.position.x, newBkgY, transform.position.z);
        }
	}

	private void ScrollLeft ()
	{
		Vector3 right = layers [rightIndex].position;
		layers [rightIndex].position = new Vector3 ((layers [leftIndex].position.x - backgroundSize), right.y, right.z);
		leftIndex = rightIndex;
		rightIndex--;
		if (rightIndex < 0)
			rightIndex = layers.Length - 1;
	}

	private void ScrollRight ()
	{
		Vector3 left = layers [leftIndex].position;
		layers [leftIndex].position = new Vector3 ((layers [rightIndex].position.x + backgroundSize), left.y, left.z);
		rightIndex = leftIndex;
		leftIndex++;
		if (leftIndex == layers.Length)
			leftIndex = 0;
	}
}