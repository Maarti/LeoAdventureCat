using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUIController : MonoBehaviour
{
	public GameObject pausePanel, mobileController;
	public bool gamePaused = false;
	GameController gc;
	Text kittyzTxt, timeTxt, lifeTxt, scoreTxt;
	bool isStarted = false;

	void Start ()
	{
		gc = GameObject.Find ("GameController").GetComponent<GameController> ();
		kittyzTxt = GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/Scores/ScoreKittyz/Score").GetComponent<Text> ();
		timeTxt = GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/Scores/ScoreTime/Score").GetComponent<Text> ();
		lifeTxt = GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/Scores/ScoreLife/Score").GetComponent<Text> ();
		scoreTxt = GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/TotalScore").GetComponent<Text> ();
		isStarted = true;
	}

	void InitScores ()
	{
		kittyzTxt.text = gc.kittyzCollected.ToString ();
		timeTxt.text = Mathf.RoundToInt (gc.levelTimer).ToString ();
		lifeTxt.text = gc.lifeLost.ToString ();
		scoreTxt.text = Mathf.FloorToInt (gc.CalculateScore ()).ToString () + "%";
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
