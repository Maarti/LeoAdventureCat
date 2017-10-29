using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalPoundLiberationTrigger : MonoBehaviour
{
	public GameObject rat, rope, cage;
	public Transform chairDown, chairUp, frontCage, runOut;
	public float ratSpeed;
	bool ratLiberated = false, inFrontOfCage = false, ratFacingRight = false, isCageOpen = false;
	bool waitingTimeIsSet = false;
	int state = 0;
	float startWaitingTime;
	Animator ratAnim;
	GameUIController guic;

	// Use this for initialization
	void Start ()
	{
		ratAnim = rat.GetComponent<Animator> ();
		guic = GameObject.Find ("Canvas/GameUI").GetComponent<GameUIController> ();
	}

	void FixedUpdate ()
	{
		if (ratLiberated) {
			// Rat run to chair
			if (state == 0) {
				if (rat.transform.position != chairDown.position) {
					rat.transform.position = Vector3.MoveTowards (rat.transform.position, chairDown.position, Time.deltaTime * ratSpeed);
					ratAnim.SetFloat ("speed", ratSpeed);
				} else {
					rat.transform.Rotate (Vector3.forward * 90);
					state++;
				}
			}

			// Rat run to key
			else if (state == 1) {
				if (rat.transform.position != chairUp.position) {
					rat.transform.position = Vector3.MoveTowards (rat.transform.position, chairUp.position, Time.deltaTime * ratSpeed);
					ratAnim.SetFloat ("speed", ratSpeed);
				} else {
					ratAnim.SetBool ("gotKey", true);
					RatFlip ();
					state++;
				}
			} 

			// Rat run to chair
			else if (state == 2) {
				if (rat.transform.position != chairDown.position) {
					rat.transform.position = Vector3.MoveTowards (rat.transform.position, chairDown.position, Time.deltaTime * ratSpeed);
					ratAnim.SetFloat ("speed", ratSpeed);
				} else {
					rat.transform.Rotate (Vector3.back * 90);
					state++;
				}
			} 

			// Rat run to cage
			else if (state == 3) {
				if (rat.transform.position != frontCage.position) {
					rat.transform.position = Vector3.MoveTowards (rat.transform.position, frontCage.position, Time.deltaTime * ratSpeed);
					ratAnim.SetFloat ("speed", ratSpeed);
				} else {
					ratAnim.SetFloat ("speed", 0f);
					state++;
				}
			}

			// Waiting 2s
			else if (state == 4) {
				if (!waitingTimeIsSet) {
					startWaitingTime = Time.time;
					waitingTimeIsSet = true;
				} else {
					if ((Time.time - startWaitingTime) < 2f)
						return;
					else {
						ratAnim.SetBool ("gotKey", false);
						state++;					
					}
				}
			}

			// Rat open cage
			else if (state == 5) {
				if (!isCageOpen) {
					cage.GetComponent<Animator> ().SetBool ("isOpened", true);
					isCageOpen = true;
				} else {
					guic.DisplayDialog (DialogEnum.rat_asks_follow);
					RatFlip ();
					ratSpeed = 2.5f;
					state++;
				}
			}

			// Rat run out
			else if (state == 6) {
				if (rat.transform.position != runOut.position) {
					rat.transform.position = Vector3.MoveTowards (rat.transform.position, runOut.position, Time.deltaTime * ratSpeed);
					ratAnim.SetFloat ("speed", ratSpeed);
				} else {
					ratAnim.SetFloat ("speed", 0f);
					state++;
				}
			} 

			// Destroy
			else if (state == 7)
				Destroy (this.gameObject);

		}
	}


	void OnTriggerExit2D (Collider2D other)
	{
		GameObject obj = other.gameObject;
		if (obj.name == "Rat") {
			ratAnim.SetBool ("isHanging", false);
			StartCoroutine (DestroyRope ());
		}
	}

	IEnumerator DestroyRope ()
	{
		SpriteRenderer[] sprites = rope.GetComponentsInChildren<SpriteRenderer> ();
		Color newColor;

		for (float t = 0.0f; t <= 1.0f; t += Time.deltaTime / 1f) {

			foreach (SpriteRenderer sprite in sprites) {
				newColor = sprite.color;
				newColor.a = Mathf.Lerp (1, 0, t);
				sprite.color = newColor;
			}
			yield return null;
		}
		Destroy (rope.gameObject);
		yield return new WaitForSeconds (1f);
		guic.DisplayDialog (DialogEnum.rat_thanks_cat);
		yield return new WaitForSeconds (.5f);
		RatFlip ();
		rat.GetComponent<Rigidbody2D> ().isKinematic = true;
		ratLiberated = true;
	}


	void RatFlip ()
	{
		ratFacingRight = !ratFacingRight;
		Vector3 theScale = new Vector3 (rat.transform.localScale.x * -1, rat.transform.localScale.y, rat.transform.localScale.z);
		rat.transform.localScale = theScale;		
	}
}
