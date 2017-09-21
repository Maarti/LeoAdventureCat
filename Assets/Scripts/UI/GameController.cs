using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	public static GameController gc;
	public int lifeLost = 0, kittyzCollected = 0;
	public float levelTimer = 0f;
	public bool gamePaused = false, gameFinished = false;
	GameUIController guic;
	Level level;

	void Awake ()
	{
		if (gc != this)
			gc = this;
	}

	void Start ()
	{
		guic = GameObject.Find ("Canvas/GameUI").GetComponent<GameUIController> ();

		// Get Level from Scene name
		string sceneName = SceneManager.GetActiveScene ().name;
		LevelEnum lvlEnum = (LevelEnum)Enum.Parse (typeof(LevelEnum), sceneName); 
		this.level = ApplicationController.ac.levels [lvlEnum];		
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

		// Pause the UI if not yet paused
		if (guic.gamePaused != pause)
			guic.PauseGame (pause);		
	}

	void PlayerInjured (int dmg)
	{
		this.lifeLost += dmg;	
	}

	void CollectKittyz (int amount = 1)
	{
		kittyzCollected += amount;	
		ApplicationController.ac.playerData.updateKittys (1);	
	}

	public float CalculateScore ()
	{
		// Kittyz score
		float kittyzScore = kittyzCollected / level.targetKittyz;

		// Time score
		const float timeFlexibility = 2;
		float targetTime = level.targetTime;
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

		float finalScore = (1 / 3 * kittyzScore + 1 / 3 * timeScore + 1 / 3 * lifeScore) * 100;
		return finalScore;
	}

}

