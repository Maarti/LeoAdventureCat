using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointController : MonoBehaviour
{

	public static CheckPointController cc;
	public float levelTimer;
	public string checkpointName = "null";
	public LevelEnum levelEnum;
	public int lifeLost = 0, kittyzCollected = 0;
	public List<string> kittyzDestroyed = new List<string> ();

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
		if (checkpointName != "null") {
			DestroyKittyz ();
			LoadScores ();
			TeleportPlayerToCheckpoint ();
		}
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
			GameObject kittyz = GameObject.Find (kittyzName);
			if (kittyz)
				Destroy (kittyz);
		}
	}

	void TeleportPlayerToCheckpoint ()
	{
		GameObject player = GameObject.FindWithTag ("Player");
		GameObject checkpoint = GameObject.Find (checkpointName);
		player.transform.position = checkpoint.transform.position;
	}

	public void Check (string checkpointName)
	{
		this.checkpointName = checkpointName;
		this.lifeLost = GameController.gc.lifeLost;
		this.kittyzCollected = GameController.gc.kittyzCollected;
		this.levelTimer = GameController.gc.levelTimer;
	}

	public void KittyzCollected (string kittyzObjectName)
	{
		this.kittyzDestroyed.Add (kittyzObjectName);
	}

}
