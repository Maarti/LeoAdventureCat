// https://www.youtube.com/watch?v=HMZnSZswTmU

using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour
{

	public GameObject platform;
	public float moveSpeed;
	public Transform[] points;
	public int pointSelection;

	Transform currentPoint;

	// Use this for initialization
	void Start ()
	{
		currentPoint = points [pointSelection];
	}
	
	// Update is called once per frame
	void Update ()
	{
		platform.transform.position = Vector3.MoveTowards (platform.transform.position, currentPoint.position, Time.deltaTime * moveSpeed);
		if (platform.transform.position == currentPoint.position) {
			pointSelection++;
			if (pointSelection >= points.Length)
				pointSelection = 0;			
		}
		currentPoint = points [pointSelection];
	}

}
