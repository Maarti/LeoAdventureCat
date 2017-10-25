using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointController : MonoBehaviour
{

	public static CheckPointController cc;
	public float levelTimer = 0f;
	public string checkpointName = "null";
	public LevelEnum levelEnum;
	public int lifeLost = 0, kittyzCollected = 0;
	public List<string> savedKittyzDestroyed = new List<string> ();
	public List<string> temporaryKittyzDestroyed = new List<string> ();

	void Awake ()
	{
		Debug.Log ("cp awake name=" + checkpointName);
		if (cc == null) {
			DontDestroyOnLoad (gameObject);
			cc = this;
		} else if (cc != this) {
			Destroy (gameObject);
		}
	}

	public void Load ()
	{
		Debug.Log ("cp load name=" + checkpointName);
		if (checkpointName != "null") {			
			DestroyKittyz ();
			LoadScores ();
			TeleportPlayerToCheckpoint ();
			this.temporaryKittyzDestroyed = new List<string> (this.savedKittyzDestroyed);
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
		foreach (string kittyzName in savedKittyzDestroyed) {
			GameObject kittyz = GameObject.Find (kittyzName);
			if (kittyz)
				Destroy (kittyz);
		}
	}

	void TeleportPlayerToCheckpoint ()
	{
		GameObject player = GameObject.FindWithTag ("Player");
		GameObject checkpoint = GameObject.Find (checkpointName + "/Spawn");
		player.transform.position = checkpoint.transform.position;
	}

	public void Check (string checkpointName)
	{
		this.checkpointName = checkpointName;
		this.lifeLost = GameController.gc.lifeLost;
		this.kittyzCollected = GameController.gc.kittyzCollected;
		this.levelTimer = GameController.gc.levelTimer;
		// copy list by value, not reference
		this.savedKittyzDestroyed = new List<string> (this.temporaryKittyzDestroyed);
	}

	public void KittyzCollected (string kittyzObjectName)
	{
		this.temporaryKittyzDestroyed.Add (kittyzObjectName);
	}

	public void Reset (LevelEnum levelEnum)
	{
		this.levelEnum = levelEnum;
		this.levelTimer = 0f;
		this.checkpointName = "null";
		this.lifeLost = 0;
		this.kittyzCollected = 0;
		this.savedKittyzDestroyed = new List<string> ();
		this.temporaryKittyzDestroyed = new List<string> ();
	}

}
