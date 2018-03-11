using System.Collections;
using UnityEngine;

public class ForestSpecialEnd : EndSign
{

	public AudioClip carDoorSound, carIdleSound, scaryBossAwake;
	GameObject boss, player, van;
	Transform doorPosition, drivingPosition, vanLeftPosition, vanRightPosition;
	Animator bossAnim, vanAnim;
	DogCatcherController bossCtrlr;
	bool isTriggered = false;
	AudioSource audioSource;

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
		vanLeftPosition = GameObject.Find ("Triggers/VanLeftPosition").transform;
		vanRightPosition = GameObject.Find ("Triggers/VanRightPosition").transform;
		audioSource = GetComponent<AudioSource> ();
	}

	protected override void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player" && !isTriggered) {
			isTriggered = true;
			GameController.gc.gameFinished = true;
			guic.DisplayMobileController (false);
			guic.DisplayTopUI (false);
            player.GetComponent<PlayerController>().freeze=true;
            player.GetComponent<PlayerController> ().StartMoving (0f);
			Physics2D.IgnoreCollision (boss.GetComponent<Collider2D> (), player.GetComponent<Collider2D> (), true);
			foreach (Collider2D col in van.GetComponentsInChildren<Collider2D> ()) {
				Physics2D.IgnoreCollision (col, player.GetComponent<Collider2D> (), true);
			}
			StartCoroutine (EndingAnimation ());
		}
	}

	IEnumerator EndingAnimation ()
	{
		// Boss wake up and flip to player
		PlaySound (scaryBossAwake);
		bossAnim.SetTrigger ("wakeUp");
		yield return new WaitForSeconds (2f);

		// Boss start absorbing
		bossAnim.SetTrigger ("absorb");
		yield return new WaitForSeconds (2.5f);

		// Cat "disapear"
		SpriteRenderer[] playerSprites = player.GetComponentsInChildren<SpriteRenderer> ();
		foreach (SpriteRenderer sprite in playerSprites) {
			sprite.enabled = false;
		}
        TrailRenderer tr = player.GetComponentInChildren<TrailRenderer>();
        if (tr) tr.enabled = false;

		// Boss stop absorbing and flip
		bossCtrlr.StopAbsorb ();
		bossCtrlr.StopAudio ();
		yield return new WaitForSeconds (1f);
		bossCtrlr.Flip ();

		// Boss run to car
		bossCtrlr.Run ();
		while (boss.transform.position != doorPosition.transform.position) {
			boss.transform.position = Vector3.MoveTowards (boss.transform.position, doorPosition.position, Time.deltaTime * 3f);
			player.transform.position = Vector3.MoveTowards (player.transform.position, doorPosition.position, Time.deltaTime * 3.5f);
			yield return null;
		}
		bossCtrlr.Run (false);

		// Boss open rear door and drop stuff
		vanAnim.SetTrigger ("openDoor");
		PlaySound (carDoorSound);
		yield return new WaitForSeconds (1f);
		boss.transform.Find ("Body/Weapon").gameObject.SetActive (false);
		boss.transform.Find ("Body/Bag").gameObject.SetActive (false);
		yield return new WaitForSeconds (1.6f);
		PlaySound (carDoorSound);

		// Boss go to drive seat
		bossCtrlr.Run ();
		while (boss.transform.position != drivingPosition.position) {
			boss.transform.position = Vector3.MoveTowards (boss.transform.position, drivingPosition.position, Time.deltaTime * 1.5f);
			yield return null;
		}
		bossCtrlr.Run (false);
		boss.transform.Find ("Body/UpLeg").gameObject.SetActive (false);
		boss.transform.Find ("Body/UpLeg2").gameObject.SetActive (false);
		PlaySound (carDoorSound);
		boss.transform.parent = van.transform;
		yield return new WaitForSeconds (1f);

		// Van start
		vanAnim.SetTrigger ("startRolling");
		PlaySound (carIdleSound, true);

		// Van go to left
		while (van.transform.position != vanLeftPosition.position) {
			van.transform.position = Vector3.MoveTowards (van.transform.position, vanLeftPosition.position, Time.deltaTime * 1.5f);
			yield return null;
		}

		// Van flip
		Vector3 newScale = new Vector3 (van.transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
		van.transform.localScale = newScale;

		// Van go to right faster
		PlaySound (carIdleSound, true, 1.2f);
		while (van.transform.position != vanRightPosition.position) {
			van.transform.position = Vector3.MoveTowards (van.transform.position, vanRightPosition.position, Time.deltaTime * 7f);
			yield return null;
		}

		// Score panel
		audioSource.Stop ();
		guic.EndGame ();
	}

	void PlaySound (AudioClip audioClip, bool loop = false, float pitch = 1f, float volume = 1f)
	{
		audioSource.Stop ();
		audioSource.clip = audioClip;
		audioSource.loop = loop;
		audioSource.pitch = pitch;
		audioSource.volume = volume;
		audioSource.Play ();
	}
}
