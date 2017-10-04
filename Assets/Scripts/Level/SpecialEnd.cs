using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpecialEnd : EndSign
{

	GameObject boss;
	Animator bossAnim;

	protected override void Start ()
	{
		base.Start ();
		boss = GameObject.Find ("Boss");
		bossAnim = boss.GetComponent<Animator> ();
	}

	protected override void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player") {
			GameController.gc.gameFinished = true;
			guic.DisplayMobileController (false);
			bossAnim.SetTrigger ("wakeUp");
		}
	}
}
