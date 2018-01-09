using UnityEngine;

// https://www.youtube.com/watch?v=HMZnSZswTmU

public class MovingPlatform : MonoBehaviour
{
	public GameObject platform;
    public float moveSpeed;
    public Transform[] points;
	public int pointSelection;
    public float smoothVal = 4f;    // smoothVal = divisor coefficient
    public float smoothDist = .8f;  // distance from which to slow down

    Transform currentPoint;

	// Use this for initialization
	void Start ()
	{
		currentPoint = points [pointSelection];
	}
	
	// Update is called once per frame
	void Update ()
	{
        float speed = moveSpeed;
        if (smoothVal > 0f && smoothDist > 0f)
        {
            float distNext = Vector3.Distance(platform.transform.position, currentPoint.position);
            // slow down when arriving to point
            if (distNext < smoothDist)
                speed = Mathf.Lerp(moveSpeed / smoothVal, moveSpeed, distNext / smoothDist);
            // smoothly reaccelerate
            else
            {
                int previous = (pointSelection - 1 < 0) ? points.Length - 1 : pointSelection - 1;
                float distPrevious = Vector3.Distance(platform.transform.position, points[previous].position);
                if (distPrevious< smoothDist)
                    speed = Mathf.Lerp(moveSpeed / smoothVal, moveSpeed, distPrevious / smoothDist);
            }
            
        }
        platform.transform.position = Vector3.MoveTowards (platform.transform.position, currentPoint.position, Time.deltaTime * speed);

		if (platform.transform.position == currentPoint.position) {
			pointSelection++;
			if (pointSelection >= points.Length)
				pointSelection = 0;			
		}
		currentPoint = points [pointSelection];
	}

}
