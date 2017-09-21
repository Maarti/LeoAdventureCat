﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUIController : MonoBehaviour
{
	public GameObject pausePanel, mobileController;
	public bool gamePaused = false;
	GameController gc;
	Text kittyzTxt, timeTxt, lifeTxt, scoreTxt, targetKittyzTxt, targetTimeTxt, targetLifeTxt;
	bool isStarted = false, targetsInited = false;

	void Start ()
	{
		gc = GameObject.Find ("GameController").GetComponent<GameController> ();
		kittyzTxt = GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/Scores/ScoreKittyz/Score").GetComponent<Text> ();
		timeTxt = GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/Scores/ScoreTime/Score").GetComponent<Text> ();
		lifeTxt = GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/Scores/ScoreLife/Score").GetComponent<Text> ();
		scoreTxt = GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/TotalScore").GetComponent<Text> ();
		//Targets
		targetKittyzTxt = GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/Scores/ScoreKittyz/Target").GetComponent<Text> ();
		targetTimeTxt = GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/Scores/ScoreTime/Target").GetComponent<Text> ();
		targetLifeTxt = GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/Scores/ScoreLife/Target").GetComponent<Text> ();

		isStarted = true;
	}

	void InitScores ()
	{
		kittyzTxt.text = gc.kittyzCollected.ToString ();
		timeTxt.text = Mathf.RoundToInt (gc.levelTimer).ToString ();
		lifeTxt.text = gc.lifeLost.ToString ();
		scoreTxt.text = Mathf.FloorToInt (gc.CalculateScore ()).ToString () + "%";
		GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/Scores/ScoreKittyz/Target").GetComponent<Text> ().text = "/" + gc.targetKittyz.ToString ();
		GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/Scores/ScoreTime/Target").GetComponent<Text> ().text = "/" + gc.targetTime.ToString () + "s";
		GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/Scores/ScoreLife/Target").GetComponent<Text> ().text = "(" + gc.targetLife.ToString () + "max)";
		if (!targetsInited) {
			targetKittyzTxt.text = "/" + gc.targetKittyz.ToString ();
			targetTimeTxt.text = "/" + gc.targetTime.ToString () + "s";
			targetLifeTxt.text = "(" + gc.targetLife.ToString () + "max)";
		}
	}

	public void TooglePause ()
	{
		PauseGame (!gamePaused);
	}

	public void PauseGame (bool pause = true)
	{		
		// Pause the UI
		gamePaused = pause;
		pausePanel.SetActive (pause);
		if (pause)
			InitScores ();
		if (mobileController)
			mobileController.SetActive (!pause);

		// Pause the game if not yet paused
		if (gc.gamePaused != pause)
			gc.PauseGame (pause);
	}

	public void ReloadScene ()
	{
		PauseGame (false);
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}

	public void LoadMainMenu ()
	{
		PauseGame (false);
		SceneManager.LoadScene ("main_menu");
	}

}
