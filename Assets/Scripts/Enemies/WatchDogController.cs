using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchDogController : MonoBehaviour
{
    GameObject theBall, theDog;
	Rigidbody2D ballRb;
	Transform mouthBall, spawn, dogTarget, ballAreaTop, ballAreaBottom;
    bool ballIsCaught = false, isResetting = false;


	void Start ()
	{
		theDog = transform.Find ("WatchDog").gameObject;
		theBall = transform.Find ("BouncingBall").gameObject;
		ballRb = theBall.GetComponent<Rigidbody2D> ();
		mouthBall = theDog.transform.Find ("Body/Head/Mouth/MouthBall");
		spawn = transform.Find ("BallSpawn");
		dogTarget = transform.Find ("DogTarget");
		ballAreaTop = transform.Find ("BallAreaTop");
		ballAreaBottom = transform.Find ("BallAreaBottom");
	}

	void Update ()
	{
        if (!isResetting)
        {
            if (!ballIsCaught && (ballRb.IsSleeping() || !IsBallInArea()))
                ResetBall();
        }
        else
        {
            theBall.transform.position = Vector3.Lerp(theBall.transform.position, spawn.position, 0.1f);
            float diff = Mathf.Abs(spawn.position.x - theBall.transform.position.x) +
                            Mathf.Abs(spawn.position.y - theBall.transform.position.y);

            //if(spawn.position == theBall.transform.position)
            if (diff < 0.01f)
            {
                isResetting = false;
                ballRb.isKinematic = false;
                ballRb.velocity = Vector2.zero;
                ballRb.angularVelocity = 0f;
                ballRb.rotation = 0f;
                theBall.layer = LayerMask.NameToLayer("Destructible");
                ballRb.transform.position = spawn.position;
            }
        }
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject == theBall) {		
			ballIsCaught = true;	
			StartCoroutine (BallMoveToDog ());
			theDog.GetComponent<Animator> ().SetTrigger ("catchBall");
		}
	}

	public void ResetBall ()
	{
		if (!ballIsCaught) {
            isResetting = true;
			ballRb.isKinematic = true;
			ballRb.velocity = Vector2.zero;
			ballRb.angularVelocity = 0f;
			ballRb.rotation = 0f;
            theBall.layer = LayerMask.NameToLayer("Transparent");
			//ballRb.transform.position = spawn.position;

		}
	}

	IEnumerator BallMoveToDog ()
	{
		float moveSpeed = 2f;
		ballRb.isKinematic = true;
		ballRb.velocity = Vector2.zero;
		float totalTime = 0f;
		while (totalTime < 0.25f) {
			theBall.transform.position = Vector3.MoveTowards (theBall.transform.position, mouthBall.position, Time.deltaTime * moveSpeed);
			totalTime += Time.deltaTime;
			yield return null;
		}
		theBall.SetActive (false);
		StartCoroutine (DogMoveToTarget ());
	}

	IEnumerator DogMoveToTarget ()
	{
		float moveSpeed = 1.5f;
		while (theDog.transform.position != dogTarget.position) {
			theDog.transform.position = Vector3.MoveTowards (theDog.transform.position, dogTarget.position, Time.deltaTime * moveSpeed);
			yield return null;
		}
		theDog.GetComponent<Animator> ().SetTrigger ("sit");
	}

	bool IsBallInArea ()
	{
		return (
		    theBall.transform.position.x >= ballAreaTop.position.x
		    && theBall.transform.position.x <= ballAreaBottom.position.x
		    && theBall.transform.position.y < ballAreaTop.position.y
		    && theBall.transform.position.y > ballAreaBottom.position.y);
	}

}
