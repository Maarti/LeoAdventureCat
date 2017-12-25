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
			this.temporaryKittyzDestroyed = new List<string> (this.savedKittyzDestroyed);
			LevelSpecialLoad ();
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
        // we keep the player Z axis
		player.transform.position = new Vector3(checkpoint.transform.position.x, checkpoint.transform.position.y, player.transform.position.z);
        // also teleport camera to player
        Camera.main.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, Camera.main.transform.position.z);
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

	void LevelSpecialLoad ()
	{
		switch (levelEnum) {
		case LevelEnum.level_2_story:
			GameObject.Destroy (GameObject.Find ("Rope"));
			GameObject.Destroy (GameObject.Find ("Triggers/intro_trigger"));
			GameObject.Destroy (GameObject.Find ("Triggers/liberation_trigger"));
			GameObject.Destroy (GameObject.Find ("Triggers/stop_trigger"));
            // rat on cat
            if (!checkpointName.Equals("Checkpoint_end"))
            {
                GameObject rat = GameObject.FindGameObjectWithTag("Rat");
                rat.GetComponent<Collider2D>().enabled = false;
                Vector3 ratScale = rat.transform.localScale;
                Transform ratLocation = GameObject.Find("Cat/Body/RatLocation").transform;
                rat.transform.position = ratLocation.position;
                rat.transform.parent = ratLocation;
                rat.transform.localScale = ratScale;
                rat.GetComponent<Animator>().SetBool("gotMouthCheese", false);
                rat.GetComponent<Animator>().SetBool("isEating", true);
            }
            // rat at the end
            else
            {
                GameObject rat = GameObject.FindGameObjectWithTag("Rat");
                rat.transform.position = GameObject.Find("BossManager/RatUpPosition").transform.position;
                Vector3 theScale = new Vector3(Mathf.Abs(rat.transform.localScale.x) * -1, rat.transform.localScale.y, rat.transform.localScale.z);
                rat.transform.localScale = theScale;
            }
			break;
		default:
			break;
		}
	}

}
