using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalPoundRatStopTrigger : MonoBehaviour
{
	public GameObject rat;
	public Transform chairDown;
	public float ratSpeed;
	GameObject cat;
	bool ratFacingRight = true, isTrigered = false;
	int state = 0;
	Animator ratAnim;
	GameUIController guic;
	Transform ratLocation;

	// Use this for initialization
	void Start ()
	{
		ratAnim = rat.GetComponent<Animator> ();
		guic = GameObject.Find ("Canvas/GameUI").GetComponent<GameUIController> ();
		cat = GameObject.FindGameObjectWithTag ("Player");
		ratLocation = cat.transform.Find ("Body/RatLocation");
	}

	void FixedUpdate ()
	{
		if (state == 0)
			return;
		
		// Rat say stop
		else if (state == 1) {
			guic.DisplayDialog (DialogEnum.rat_asks_wait);
			cat.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
			RatFlip ();
			state++;
		} 

		// Rat run to chair / Disable UI
		else if (state == 2) {		
			guic.DisplayMobileController (false);
			guic.DisplayTopUI (false);
			state++;
		}

		// Rat run to chair / Disable UI
		else if (state == 3) {			
			if (rat.transform.position != chairDown.position) {
				rat.transform.position = Vector3.MoveTowards (rat.transform.position, chairDown.position, Time.deltaTime * ratSpeed);
				ratAnim.SetFloat ("speed", ratSpeed);
			} else {
				RatFlip ();
				ratAnim.SetBool ("gotMouthCheese", true);
				state++;
			}
		} 

		// Rat run to cat
		else if (state == 4) {
			if (rat.transform.position.x != ratLocation.position.x) {
				Vector3 target = new Vector3 (ratLocation.position.x, rat.transform.position.y, rat.transform.position.z);
				rat.transform.position = Vector3.MoveTowards (rat.transform.position, target, Time.deltaTime * ratSpeed);
				ratAnim.SetFloat ("speed", ratSpeed);
			} else {
				ratAnim.SetFloat ("speed", 0f);
				guic.DisplayDialog (DialogEnum.rat_ready);
				state++;
			}
		}

		// Rat jump on cat and eat cheese
		else if (state == 5) {
			rat.GetComponent<Collider2D> ().enabled = false;
			Vector3 ratScale = rat.transform.localScale;
			rat.transform.position = ratLocation.position;
			rat.transform.parent = ratLocation;
			rat.transform.localScale = ratScale;
			ratAnim.SetBool ("gotMouthCheese", false);
			ratAnim.SetBool ("isEating", true);
			//guic.DisplayDialog (DialogEnum.rat_ready);
			guic.DisplayMobileController ();
			guic.DisplayTopUI ();
			state++;
		}

		// Destroy
		else if (state == 6)
			Destroy (this.gameObject);

		
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (!isTrigered && other.gameObject.tag == "Player") {
			isTrigered = true;
			state++;
		}
	}

	void RatFlip ()
	{
		ratFacingRight = !ratFacingRight;
		Vector3 theScale = new Vector3 (rat.transform.localScale.x * -1, rat.transform.localScale.y, rat.transform.localScale.z);
		rat.transform.localScale = theScale;		
	}
}
