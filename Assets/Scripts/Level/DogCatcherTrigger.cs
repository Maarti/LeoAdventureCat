﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogCatcherTrigger : MonoBehaviour
{

	public DialogEnum dialog;
	public float moveSpeed;
	public Transform endBossPosition;
	GameUIController guic;
	bool arrived = false, isBossInitialized = false, isTriggered = false, isRunning = false;
	GameObject player, boss;
	DogCatcherController bossCtrlr;


	void Start ()
	{
		guic = GameObject.Find ("Canvas/GameUI").GetComponent<GameUIController> ();
		player = GameObject.FindWithTag ("Player");
		boss = GameObject.Find ("Boss");
		bossCtrlr = boss.GetComponent<DogCatcherController> ();
		Physics2D.IgnoreCollision (boss.GetComponent<Collider2D> (), player.GetComponent<Collider2D> ());
		boss.SetActive (false);
	}

	// Update is called once per frame
	void Update ()
	{
		if (isTriggered) {	
			//player.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
			if (!arrived) {
				Physics2D.IgnoreCollision (boss.GetComponent<Collider2D> (), player.GetComponent<Collider2D> (), true);
				RunToRight ();
				boss.transform.position = Vector3.MoveTowards (boss.transform.position, endBossPosition.position, Time.deltaTime * moveSpeed);
				if (boss.transform.position == endBossPosition.position) {
					arrived = true;
					bossCtrlr.Flip ();
					bossCtrlr.Run (false);
				}
			} else if (arrived && !isBossInitialized) {
				Physics2D.IgnoreCollision (boss.GetComponent<Collider2D> (), player.GetComponent<Collider2D> (), false);
				isBossInitialized = true;
			} else if (arrived && isBossInitialized) {
				//Time.timeScale = 1f;
				Destroy (this.gameObject);
			}
		}
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player") {
			isTriggered = true;
			guic.DisplayDialog (dialog);
			boss.SetActive (true);
		}
	}

	void OnDestroy ()
	{
		Time.timeScale = 1f;
	}

	void RunToRight ()
	{
		if (!isRunning) {
			bossCtrlr.Flip ();
			bossCtrlr.Run ();
			isRunning = true;
		}
	}
}