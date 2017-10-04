using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ForestSpecialEnd : EndSign
{

	GameObject boss, player, van;
	Transform doorPosition, drivingPosition;
	Animator bossAnim, vanAnim;
	DogCatcherController bossCtrlr;

	protected override void Start ()
	{
		base.Start ();
		player = GameObject.FindWithTag ("Player");
		boss = GameObject.Find ("Boss");
		bossAnim = boss.GetComponent<Animator> ();
		bossCtrlr = boss.GetComponent<DogCatcherController> ();
		doorPosition = GameObject.Find ("Van/BossDoorPosition").transform;
		drivingPosition = GameObject.Find ("Van/BossDrivingPosition").transform;
		van = GameObject.Find ("Van");
		vanAnim = van.GetComponent<Animator> ();
	}

	protected override void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player") {
			GameController.gc.gameFinished = true;
			guic.DisplayMobileController (false);
			Physics2D.IgnoreCollision (boss.GetComponent<Collider2D> (), player.GetComponent<Collider2D> (), true);
			StartCoroutine (EndingAnimation ());
		}
	}

	IEnumerator EndingAnimation ()
	{
		// Boss wake up and flip to player
		bossAnim.SetTrigger ("wakeUp");
		yield return new WaitForSeconds (2f);

		// Boss start absorbing
		bossAnim.SetTrigger ("absorb");
		yield return new WaitForSeconds (2f);

		// Cat "disapear"
		SpriteRenderer[] playerSprites = player.GetComponentsInChildren<SpriteRenderer> ();
		foreach (SpriteRenderer sprite in playerSprites) {
			sprite.enabled = false;
		}

		// Boss stop absorbing and flip
		bossCtrlr.StopAbsorb ();
		yield return new WaitForSeconds (1f);
		bossCtrlr.Flip ();

		// Boss run to car
		bossCtrlr.Run ();
		while (boss.transform.position != doorPosition.transform.position) {
			boss.transform.position = Vector3.MoveTowards (boss.transform.position, doorPosition.position, Time.deltaTime * 3f);
			player.transform.position = Vector3.MoveTowards (player.transform.position, doorPosition.position, Time.deltaTime * 3f);
			yield return null;
		}
		bossCtrlr.Run (false);

		// Boss open rear door and drop stuff
		vanAnim.SetTrigger ("openDoor");
		yield return new WaitForSeconds (0.8f);
		boss.transform.Find ("Body/Weapon").gameObject.SetActive (false);
		boss.transform.Find ("Body/Bag").gameObject.SetActive (false);
		yield return new WaitForSeconds (1f);

		// Boss go to drive seat
		while (boss.transform.position != drivingPosition.position) {
			boss.transform.position = Vector3.MoveTowards (boss.transform.position, drivingPosition.position, Time.deltaTime * 1.5f);
			yield return null;
		}
		boss.transform.Find ("Body/UpLeg").gameObject.SetActive (false);
		boss.transform.Find ("Body/UpLeg2").gameObject.SetActive (false);

		// Van start
		vanAnim.SetTrigger ("startRolling");

	}
}
