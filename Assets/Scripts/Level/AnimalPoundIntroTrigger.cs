using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalPoundIntroTrigger : MonoBehaviour
{

	public GameObject rat, weight;
	public Transform rat_to_cheese;
	public float ratSpeed = 2f;
	public HingeJoint2D lastRopeJoint;
	bool isTriggered = false, ratFacingRight = true, waitingTimeIsSet = false;
	bool ratStarted = false, ratArrivedAtCheese = false, ratIsEating = false, trapTriggered = false;
	Animator ratAnim;
	float startWaitingTime;
	Rigidbody2D ratRb;

	// Use this for initialization
	void Start ()
	{
		ratAnim = rat.GetComponent<Animator> ();
		ratRb = rat.GetComponent<Rigidbody2D> ();
	}

	void FixedUpdate ()
	{
		if (isTriggered = true) {

			// Rat run to cheese
			if (!ratArrivedAtCheese) {
				if (ratFacingRight)
					RatFlip ();			
				if (rat.transform.position != rat_to_cheese.position) {
					rat.transform.position = Vector3.MoveTowards (rat.transform.position, rat_to_cheese.position, Time.deltaTime * ratSpeed);
					ratAnim.SetFloat ("speed", ratSpeed);
				} else {
					ratArrivedAtCheese = true;
					ratAnim.SetFloat ("speed", 0f);
				}
			} 
			if (ratArrivedAtCheese && !trapTriggered) {

				// Rat eat cheese
				ratAnim.SetBool ("isEating", ratIsEating);

				// Wait 4 sec
				if (!waitingTimeIsSet) {
					startWaitingTime = Time.time;
					ratIsEating = true;
					waitingTimeIsSet = true;
				}
				Debug.Log ("startWaitingTime=" + startWaitingTime + " time=" + Time.time);
				if ((Time.time - startWaitingTime) < 4f)
					return;
				else
					ratIsEating = false;

				// Trap triggered
				if (!trapTriggered) {
					lastRopeJoint.connectedBody = rat.GetComponent<Rigidbody2D> ();
					weight.GetComponent<Rigidbody2D> ().mass = 40f;
					ratRb.isKinematic = false;
					ratRb.mass = 10f;
					trapTriggered = true;
				}
			}
		}

	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player") {
			isTriggered = true;
		}
	}



	void RatFlip ()
	{
		ratFacingRight = !ratFacingRight;
		Vector3 theScale = new Vector3 (rat.transform.localScale.x * -1, rat.transform.localScale.y, rat.transform.localScale.z);
		rat.transform.localScale = theScale;		
	}

	IEnumerator RatMove (Transform target)
	{
		while (rat.transform.position != target.position) {
			rat.transform.position = Vector3.MoveTowards (rat.transform.position, target.position, Time.deltaTime * ratSpeed);
			ratAnim.SetFloat ("speed", ratSpeed);
			yield return null;
		}
		ratAnim.SetFloat ("speed", 0f);
	}

}