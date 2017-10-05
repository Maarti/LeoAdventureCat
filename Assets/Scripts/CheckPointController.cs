using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointController : MonoBehaviour
{

	public static CheckPointController cc;
	public float levelTimer;
	public string checkointName;
	public LevelEnum levelEnum;
	public int lifeLost = 0, kittyzCollected = 0;
	public string[] kittyzDestroyed;

	void Awake ()
	{
		if (cc == null) {
			DontDestroyOnLoad (gameObject);
			cc = this;
		} else if (cc != this) {
			Destroy (gameObject);
		}
	}

	public void Load ()
	{
		DestroyKittyz ();
		LoadScores ();
		TeleportPlayerToCheckpoint ();
	}

	void LoadScores ()
	{
		GameController.gc.lifeLost = this.lifeLost;
		GameController.gc.kittyzCollected = this.kittyzCollected;
		GameController.gc.levelTimer = this.levelTimer;
	}

	void DestroyKittyz ()
	{
		foreach (string kittyzName in kittyzDestroyed) {
			Destroy (GameObject.Find (kittyzName));
		}
	}

	void TeleportPlayerToCheckpoint ()
	{
		GameObject player = GameObject.FindWithTag ("Player");
		GameObject checkpoint = GameObject.Find (checkointName);
		player.transform.position = checkpoint.transform.position;
	}

}
