// https://www.youtube.com/watch?v=QkisHNmcK7Y

using UnityEngine;
using System.Collections;

public class ScollingBackground : MonoBehaviour
{
	public float backgroundSize, paralaxSpeed;

	Transform cameraTransform;
	Transform[] layers;
	float viewZone = 4, lastCameraX;
	int leftIndex, rightIndex;

	// Use this for initialization
	void Start ()
	{
		cameraTransform = Camera.main.transform;
		lastCameraX = cameraTransform.position.x;
		layers = new Transform[transform.childCount];
		for (int i = 0; i < transform.childCount; i++)
			layers [i] = transform.GetChild (i);

		leftIndex = 0;
		rightIndex = layers.Length - 1;
	}

	private void Update ()
	{
		float deltaX = cameraTransform.position.x - lastCameraX;
		transform.position += Vector3.right * (deltaX * paralaxSpeed);
		lastCameraX = cameraTransform.position.x;

		//Debug.Log ("camera = " + cameraTransform.position.x + " limit = " + (layers [leftIndex].transform.position.x + viewZone));
		if (cameraTransform.position.x < (layers [leftIndex].transform.position.x + viewZone))
			ScrollLeft ();

		if (cameraTransform.position.x > (layers [rightIndex].transform.position.x - viewZone))
			ScrollRight ();
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
