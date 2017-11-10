using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchDogController : MonoBehaviour
{
	public GameObject theBall;
	public GameObject theDog;
	public Transform spawn;
	public Transform dogTarget;

	Rigidbody2D ballRb;
	Transform mouthBall;
	bool ballIsCaught = false;


	void Start ()
	{
		ballRb = theBall.GetComponent<Rigidbody2D> ();
		mouthBall = theDog.transform.Find ("Body/Head/Mouth/MouthBall");
	}

	void Update ()
	{
		if (!ballIsCaught && ballRb.IsSleeping ())
			ResetBall ();


	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject == theBall) {		
			ballIsCaught = true;	
			StartCoroutine (BallMoveToDog ());
			theDog.GetComponent<Animator> ().SetTrigger ("catchBall");


		}
	}

	void ResetBall ()
	{
		if (!ballIsCaught) {
			ballRb.isKinematic = false;
			ballRb.transform.position = spawn.position;
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

}
