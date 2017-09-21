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
	GameUIController guic;
	Level level;
	GameObject player;

	void Awake ()
	{
		if (gc != this)
			gc = this;
	}

	void Start ()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		guic = GameObject.Find ("Canvas/GameUI").GetComponent<GameUIController> ();

		// Get Level from Scene name
		string sceneName = SceneManager.GetActiveScene ().name;
		LevelEnum lvlEnum = (LevelEnum)Enum.Parse (typeof(LevelEnum), sceneName); 
		this.level = ApplicationController.ac.levels [lvlEnum];	

		targetKittyz = level.targetKittyz;
		targetTime = level.targetTime;
		targetLife = level.targetLife;
	}

	void Update ()
	{
		if (!gamePaused && !gameFinished)
			levelTimer += Time.deltaTime;
	}

	public void PauseGame (bool pause = true)
	{
		// Pause the game
		gamePaused = pause;
		Time.timeScale = (pause) ? 0f : 1f;
		if (pause)
			player.GetComponent<PlayerController> ().StartMoving (0f);

		// Pause the UI if not yet paused
		if (guic.gamePaused != pause)
			guic.PauseGame (pause);		
	}

	public void PlayerInjured (int dmg)
	{
		this.lifeLost += dmg;	
	}

	public void CollectKittyz (int amount = 1)
	{
		kittyzCollected += amount;	
		ApplicationController.ac.playerData.updateKittys (1);	
	}

	public float CalculateScore ()
	{
		// Kittyz score
		float kittyzScore = kittyzCollected / targetKittyz;

		// Time score
		const float timeFlexibility = 2;
		float timeScore = 0f;
		if (levelTimer <= targetTime)
			timeScore = 1f;
		else if (levelTimer >= targetTime * timeFlexibility)
			timeScore = 0f;
		else
			timeScore = (targetTime * timeFlexibility - levelTimer) / (targetTime * timeFlexibility - targetTime);

		// Life score
		const float lifeFlexibility = 3;
		float targetLife = level.targetLife;
		float lifeScore = 0f;
		if (lifeLost <= targetLife)
			lifeScore = 1f;
		else if (lifeLost >= targetLife * lifeFlexibility)
			lifeScore = 0f;
		else
			lifeScore = (targetLife * lifeFlexibility - lifeLost) / (targetLife * lifeFlexibility - targetLife);
		
		float finalScore = (kittyzScore / 3 + timeScore / 3 + lifeScore / 3) * 100;
		Debug.Log ("Score = " + finalScore + " KS = " + kittyzScore + " TS = " + timeScore + " LS = " + lifeScore);
		return finalScore;
	}

	public void ReloadScene ()
	{
		guic.ReloadScene ();
	}

	public void GameOver ()
	{
		ReloadScene ();
	}

}

