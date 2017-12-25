using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalPoundBulldogTrigger : MonoBehaviour
{
	public GameObject dog, ball, boundary;
	public Transform ratDown, ratUp, ratDownPipe, ratUpPipe, dogDown, dogUp;
    public float dogSpeed = 1f;
    GameObject rat;
	bool ratFacingRight = false, dogFacingRight = true;
	bool waitingTimeIsSet = false;
	int state = -1;
	float startWaitingTime;
	Animator ratAnim, dogAnim;
	GameUIController guic;

	// Use this for initialization
	void Start ()
	{
        rat = GameObject.FindGameObjectWithTag("Rat");
        ratAnim = rat.GetComponent<Animator> ();
        dogAnim = dog.GetComponent<Animator>();
		guic = GameObject.Find ("Canvas/GameUI").GetComponent<GameUIController> ();
	}

	void FixedUpdate ()
	{
		if (state>0) {
			// Rat run to chair
			/*if (state == 0) {
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
*/
		}
	}


	void OnTriggerEnter2D (Collider2D other)
	{
		if (state==-1 && other.gameObject.tag == "Player") {
            state++;
			StartCoroutine (StartAnimation ());
		}
	}

    IEnumerator StartAnimation()
    {
        // disable UI        
        guic.DisplayMobileController(false);
        guic.DisplayTopUI(false);

        // throw ball
        ball.GetComponent<Rigidbody2D>().isKinematic = false;
        ball.GetComponent<Rigidbody2D>().velocity = new Vector2(-2, 5); ;
        yield return new WaitForSeconds(1.5f);

        // activate boundary
        boundary.SetActive(true);

        // rat dialog
        guic.DisplayDialog(DialogEnum.rat_start_bulldog);
        yield return null;

        // display ui
        guic.DisplayMobileController();
        guic.DisplayTopUI();

        // dog run to rat AND rat run into pipe
        dogAnim.SetFloat("x.velocity", dogSpeed);
        ratAnim.SetFloat("speed", 1.5f);
        RatFlip();
        while (dog.transform.position != dogUp.position)
        {
            rat.transform.position = Vector3.MoveTowards(rat.transform.position, ratUpPipe.position, Time.deltaTime *1.5f);
            dog.transform.position = Vector3.MoveTowards(dog.transform.position, dogUp.position, Time.deltaTime * dogSpeed);
            yield return null;
        }
        rat.transform.position = ratDownPipe.position;
        ratAnim.SetFloat("speed", 0f);
        dogAnim.SetFloat("x.velocity", 0f);
        dogAnim.SetBool("goUp", true);

        state++;
        yield return null;
    }

    void DogFlip()
    {
    }

    void RatFlip ()
	{
		ratFacingRight = !ratFacingRight;
		Vector3 theScale = new Vector3 (rat.transform.localScale.x * -1, rat.transform.localScale.y, rat.transform.localScale.z);
		rat.transform.localScale = theScale;		
	}
}
