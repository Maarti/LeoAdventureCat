using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	public static GameController gc;
	public int lifeLost = 0, kittyzCollected = 0, targetLife, targetKittyz, targetTime;
	public float levelTimer = 0f;
	public bool gamePaused = false, gameFinished = false;
	public Level level;
	public PlayerController pc;
	GameUIController guic;
	bool lifeBarInit = false;

	void Awake ()
	{
		if (gc != this)
			gc = this;
	}

	void Start ()
	{
		Time.timeScale = 1f;
		guic = GameObject.Find ("Canvas/GameUI").GetComponent<GameUIController> ();
		pc = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();

		// Get Level from Scene name
		string sceneName = SceneManager.GetActiveScene ().name;
		LevelEnum lvlEnum = (LevelEnum)Enum.Parse (typeof(LevelEnum), sceneName); 
		this.level = ApplicationController.ac.levels [lvlEnum];	

		// Init target
		targetKittyz = level.targetKittyz;
		targetTime = level.targetTime;
		targetLife = level.targetLife;

		// Load checkpoint if exists for this level
		GameObject checkpointControllerObject = GameObject.Find ("CheckPointController");
		if (checkpointControllerObject) {
			CheckPointController checkpointController = checkpointControllerObject.GetComponent<CheckPointController> ();
			if (checkpointController.levelEnum == lvlEnum)
				checkpointController.Load ();
			else
				checkpointController.Reset (lvlEnum);
		}
	}

	void Update ()
	{
		if (!gamePaused && !gameFinished)
			levelTimer += Time.deltaTime;
	}

	void FixedUpdate ()
	{
		if (!lifeBarInit) {
			guic.DrawLifebar (pc.life);
			lifeBarInit = true;
		}
	}

	public void PauseGame (bool pause = true)
	{
		// Pause the game
		gamePaused = pause;
		Time.timeScale = (pause) ? 0f : 1f;
		if (pause)
			pc.StartMoving (0f);

		// Pause the UI if not yet paused
		if (guic.gamePaused != pause)
			guic.PauseGame (pause);		
	}

	public void EndGame ()
	{
		// Pause the game
		gameFinished = true;
		gamePaused = true;
		Time.timeScale = 0f;
		pc.StartMoving (0f);
		ApplicationController.ac.FinishLevel (this.level.id, Mathf.FloorToInt (CalculateScore ()));
	}

	public void PlayerInjured (int dmg)
	{
		if (dmg > 0)
			this.lifeLost += dmg;
		else
			this.pc.life -= dmg;
		guic.DrawLifebar (pc.life);
	}

	public void CollectKittyz (int amount = 1)
	{
		kittyzCollected += amount;	
		ApplicationController.ac.playerData.updateKittys (1);	
	}

	public float CalculateScore ()
	{
		// Kittyz score
		float kittyzScore = (float)kittyzCollected / targetKittyz;

		// Time score
		const float timeFlexibility = 1.5f;
		float timeScore = 0f;
		if (levelTimer <= targetTime)
			timeScore = 1f;
		else if (levelTimer >= targetTime * timeFlexibility)
			timeScore = 0f;
		else
			timeScore = (targetTime * timeFlexibility - levelTimer) / (targetTime * timeFlexibility - targetTime);

		// Life score
		const float lifeFlexibility = 2f;
		float lifeScore = 0f;
		if (lifeLost <= targetLife)
			lifeScore = 1f;
		else if (lifeLost >= targetLife * lifeFlexibility)
			lifeScore = 0f;
		else
			lifeScore = (float)(targetLife * lifeFlexibility - lifeLost) / (targetLife * lifeFlexibility - targetLife);
		
		float finalScore = (kittyzScore / 3 + timeScore / 3 + lifeScore / 3) * 100;
		Debug.Log ("Score = " + finalScore + " KS = " + kittyzScore + " TS = " + timeScore + " LS = " + lifeScore);
		return finalScore;
	}

	public void ReloadScene (bool fromCheckpoint = false)
	{
		guic.ReloadScene (fromCheckpoint);
	}

	public void GameOver ()
	{
		guic.GameOver ();
	}


	public void DisplayDialog (bool pauseGame = true)
	{
		if (pauseGame) {
			Time.timeScale = 0f;
			pc.StartMoving (0f);
		} else
			Time.timeScale = 1f;
	}

}

