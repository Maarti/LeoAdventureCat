using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;
using UnityEngine.Analytics;

public class GameUIController : MonoBehaviour
{
	public GameObject pausePanel, mobileController, dialogPanel, gameoverPanel, tipPanel, tipButton;
	public bool gamePaused = false, gameFinished = false;
	public Transform heartPrefab;
	public AudioClip scoreSound, pauseSound, winSound;
	GameController gc;
	Text kittyzTxt, timeTxt, lifeTxt, scoreTxt, targetKittyzTxt, targetTimeTxt, targetLifeTxt, scoreLabelTxt, totalKittyzText, checkpointCountdown;
	bool targetsInited = false, interstitialWatched = false, isGameOver = false;
	GameObject lifeBar, buttons_1, buttonNext, buttonResume, pauseTitle, topUI, checkpointController, shoplist;
	Dictionary<DialogEnum,Dialog> dialogDico;
	Level level;
	AudioSource audioSource;
	InterstitialAd interstitial;
	ActionEnum actionOnEnd = ActionEnum.main_menu;
	/*Text dialName, dialText;
	Image dialPortrait;*/


	void Start ()
	{
		gc = GameObject.Find ("GameController").GetComponent<GameController> ();
		// Get Level from Scene name
		string sceneName = SceneManager.GetActiveScene ().name;
		LevelEnum lvlEnum = (LevelEnum)Enum.Parse (typeof(LevelEnum), sceneName);
		level = ApplicationController.ac.levels [lvlEnum];
		//PausePanel
		//blocScore = GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/Scores").GetComponent<RectTransform> ();
		shoplist = GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/ShopList");
		buttons_1 = GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/Buttons_1");
		totalKittyzText = GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/Buttons_1/KittyzPanel/KittyzText").GetComponent<Text> ();
		buttonNext = GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/Buttons_2/NextLevelButton");
		buttonResume = GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/Buttons_2/ResumeButton");
		pauseTitle = GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/Title");
		kittyzTxt = GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/Scores/ScoreKittyz/Score").GetComponent<Text> ();
		timeTxt = GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/Scores/ScoreTime/Score").GetComponent<Text> ();
		lifeTxt = GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/Scores/ScoreLife/Score").GetComponent<Text> ();
		scoreTxt = GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/TotalScore").GetComponent<Text> ();
		scoreLabelTxt = GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/LabelScore").GetComponent<Text> ();
		//GameOverPanel
		checkpointCountdown = GameObject.Find ("Canvas/" + this.name + "/GameOverPanel/Countdown/CheckpointCountdown").GetComponent<Text> ();
		//TopUI
		topUI = GameObject.Find ("Canvas/" + this.name + "/TopUI");
		lifeBar = GameObject.Find ("Canvas/" + this.name + "/TopUI/LifeBar");
		//Targets
		targetKittyzTxt = GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/Scores/ScoreKittyz/Target").GetComponent<Text> ();
		targetTimeTxt = GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/Scores/ScoreTime/Target").GetComponent<Text> ();
		targetLifeTxt = GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/Scores/ScoreLife/Target").GetComponent<Text> ();
        //LevelTitle and difficulty
        GameObject levelTitle = GameObject.Find("Canvas/" + this.name + "/LevelTitle");
        GameObject levelDifficultyLabel = levelTitle.transform.Find("LevelDifficulty").gameObject;
        levelTitle.GetComponent<Text> ().text = level.GetFullName ();
        if (level.isStory)
            levelDifficultyLabel.SetActive(false);
        else
            levelDifficultyLabel.GetComponent<Text>().text = LocalizationManager.Instance.GetText(level.difficulty.ToString());
        //Checkpoints
        checkpointController = GameObject.Find ("CheckPointController");
        //TipUI
        if (ApplicationController.ac.IsTipAlreadyConsulted(TipEnum.BUY_LIFE)) {
            Destroy(tipButton);
            Destroy(tipPanel);
        }

		InstantiateDialogs ();
		audioSource = GetComponent<AudioSource> ();
		CreateInterstitial ();
	}

	void InitScores (bool gameFinished = false, bool gameOver = false)
	{
		kittyzTxt.text = gc.kittyzCollected.ToString ();
		timeTxt.text = Mathf.CeilToInt (gc.levelTimer).ToString ();
		lifeTxt.text = gc.lifeLost.ToString ();
		if (gameFinished) {
			pauseTitle.GetComponent<LocalizationUIText> ().enabled = false;
			pauseTitle.GetComponent<Text> ().text = level.GetFullName () + " " + LocalizationManager.Instance.GetText ("COMPLETED");
			scoreLabelTxt.enabled = true;
			scoreTxt.enabled = true;
			buttonNext.SetActive (true);
			buttonResume.SetActive (false);
			Level nextLevel = gc.level.GetNextUnlockedLevel ();
			buttonNext.GetComponent<Button> ().interactable = (nextLevel.id != gc.level.id) ? true : false;
			int score = Mathf.FloorToInt (gc.CalculateScore ());
			StartCoroutine ("animScore", score);
			/*} else if (gameOver) {
			pauseTitle.GetComponent<LocalizationUIText> ().key = "GAME_OVER";
			scoreTxt.enabled = false;
			scoreLabelTxt.enabled = false;
			buttonNext.SetActive (false);
			buttonResume.SetActive (false);*/
		} else {
			scoreTxt.enabled = false;
			scoreLabelTxt.enabled = false;
			buttonNext.SetActive (false);
			buttonResume.SetActive (true);
		}
		if (!targetsInited) {
			targetKittyzTxt.text = "/" + gc.targetKittyz.ToString ();
			targetTimeTxt.text = "/" + gc.targetTime.ToString () + "s";
			targetLifeTxt.text = "(" + gc.targetLife.ToString () + " max)";
		}
		totalKittyzText.text = ApplicationController.ac.playerData.kittyz.ToString (); //init total kittyz
	}

	public void TooglePause ()
	{
		PauseGame (!gamePaused);
		audioSource.PlayOneShot (pauseSound);
	}

	public void PauseGame (bool pause = true)
	{		
		// Pause the UI
		gamePaused = pause;
		pausePanel.SetActive (pause);
		if (pause)
			InitScores ();
		//if (mobileController)
		//mobileController.SetActive (!pause);
		DisplayMobileController (!pause);

		// Pause the game if not yet paused
		if (gc.gamePaused != pause)
			gc.PauseGame (pause);

        // Hide the tips if unpaused
        if (!pause)
            DisplayTip(false);
	}

	public void BuyItem (int item)
	{
		switch (item) {
		case 0: // LIFE
			if (ApplicationController.ac.playerData.kittyz >= 5 && ApplicationController.ac.playerData.max_life > gc.pc.life) { //item price hardcoded
				gc.PlayerInjured (-1);
				ApplicationController.ac.playerData.updateKittys (-5, totalKittyzText, true);
                AnalyticsResult ar = Analytics.CustomEvent("LifeBought_" + this.level.name, new Dictionary<string, object> {
                    {"current_life", gc.pc.life},
                    {"life_lost",gc.lifeLost },
                    {"total_kittys",ApplicationController.ac.playerData.kittyz},
                    {"position",gc.pc.transform.position}
                });
                Debug.Log("Analytics LifeBought :" + ar);
            }
			break;
		case 1: // CHECKPOINT
			if (ApplicationController.ac.playerData.kittyz >= 15) { //item price hardcoded
				ApplicationController.ac.playerData.updateKittys (-15, totalKittyzText, true);
				ReloadScene (true);
			}
			break;
		default:
			break;
		}
	}

	public void DisplayMobileController (bool setActive = true, bool blockRayCasts = true)
	{
		if (mobileController) {
			mobileController.SetActive (setActive);
			mobileController.GetComponent<CanvasGroup> ().blocksRaycasts = blockRayCasts;
		}
	}

	public void DisplayTopUI (bool setActive = true)
	{
		topUI.SetActive (setActive);
	}

	public void EndGame ()
	{
		gameFinished = true;
		gc.EndGame ();
		DisplayTopUI (false);
		buttons_1.SetActive (false);
		shoplist.SetActive (false);
		//blocScore.offsetMax = new Vector2 (blocScore.offsetMax.x, -100);
		pausePanel.SetActive (true);
		InitScores (true);
		audioSource.PlayOneShot (winSound, 0.5f);
		if (mobileController)
			mobileController.SetActive (false);
	}

	public void GameOver ()
	{
		DisplayTopUI (false);
		if (mobileController)
			mobileController.SetActive (false);
		StartCoroutine (DisplayGameOverMenu ());
	}

	IEnumerator DisplayGameOverMenu ()
	{
		isGameOver = true;
		yield return new WaitForSeconds (1f);
		gameoverPanel.SetActive (true);
		//countdon before reload from checkpoint
		for (int i = 5; i > 0; i--) {
			checkpointCountdown.text = "(" + i.ToString () + ")";
			yield return new WaitForSeconds (1f);
		}
		checkpointCountdown.text = "(0)";
		ReloadScene (true);
	}

	public void ReloadScene (bool fromCheckpoint = false)
	{
		StopCoroutine (DisplayGameOverMenu ());
		if (gameFinished && !interstitialWatched) {
			EndGameAction (ActionEnum.restart_level);
		} else if (isGameOver && fromCheckpoint && !interstitialWatched) {
			EndGameAction (ActionEnum.restart_from_checkpoint);
		} else {
			PauseGame (false);
			if (!fromCheckpoint)
				checkpointController.GetComponent<CheckPointController> ().Reset (this.level.id);
			//SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
			SceneLoader.LoadSceneWithLoadingScreen (SceneManager.GetActiveScene ().name);
		}
	}

	public void LoadMainMenu ()
	{
		StopCoroutine (DisplayGameOverMenu ());
		if (gameFinished && !interstitialWatched) {
			EndGameAction (ActionEnum.main_menu);
		} else {
			PauseGame (false);
			Destroy (checkpointController);
			//SceneManager.LoadScene ("main_menu");
			SceneLoader.LoadSceneWithLoadingScreen ("main_menu");
		}
	}

	public void DrawLifebar (int nbHearts)
	{
		// Remove all hearts
		foreach (Transform child in lifeBar.transform) {
			GameObject.Destroy (child.gameObject);
		}
		// Draw hearts
		if (nbHearts > 0) {
			for (int i = 1; i <= nbHearts; i++) {
				Instantiate (heartPrefab, lifeBar.transform);
			}
		}
	}

	IEnumerator animScore (int score)
	{
		float animDuration = scoreSound.length;
		int start = 0;
		int currentAnimScore = 0;
		audioSource.PlayOneShot (scoreSound);
		for (float timer = 0; timer < animDuration; timer += Time.fixedDeltaTime) {
			float progress = timer / animDuration;
			currentAnimScore = (int)Mathf.Lerp (start, score, progress);
			scoreTxt.text = currentAnimScore.ToString () + "%";
			yield return null;
		}
		scoreTxt.text = score.ToString () + "%";
		if (score >= 100) {
			scoreTxt.color = Color.green;
			scoreLabelTxt.color = Color.green;
		}
	}

	public void LoadNextScene ()
	{
		if (gameFinished && !interstitialWatched) {
			EndGameAction (ActionEnum.next_level);
		} else {
			Time.timeScale = 1f;
			Level nextLevel = gc.level.GetNextUnlockedLevel ();
			//SceneManager.LoadScene (nextLevel.id.ToString ());
			SceneLoader.LoadSceneWithLoadingScreen (nextLevel.id.ToString ());
		}
	}

    public void DisplayTipButton()
    {
        if(tipButton!=null)
            tipButton.SetActive(true);
    }

    public void DisplayTip(bool display)
    {        
        if (display) {
            if (tipPanel != null)
                tipPanel.SetActive(true);
            ApplicationController.ac.ConsultTip(TipEnum.BUY_LIFE);
            if(tipButton!=null)
                Destroy(tipButton);
        }else{
            if (tipPanel != null && tipPanel.activeSelf)
                Destroy(tipPanel);
        }
    }

	/*****************************************************************************/
	/*							ADS												 */
	/*****************************************************************************/

	// On game end, play interstitial then do action button
	void EndGameAction (ActionEnum action)
	{
		this.actionOnEnd = action;
		ShowInterstitial ();
	}

	void CreateInterstitial ()
	{
		string adUnitIdInterstitial = Config.adUnitIdInterstitial;
		//string adUnitIdInterstitial = "ca-app-pub-3940256099942544/1033173712"; // test ad
		this.interstitial = new InterstitialAd (adUnitIdInterstitial);

		interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
		interstitial.OnAdClosed += HandleOnAdFinished;
		interstitial.OnAdLeavingApplication += HandleOnAdFinished;
		RequestInterstitial ();
	}

	void RequestInterstitial ()
	{
		if (this.interstitial == null || !this.interstitial.IsLoaded ()) {
			AdRequest request = new AdRequest.Builder ()	
				//.AddTestDevice (AdRequest.TestDeviceSimulator)
				//.AddTestDevice (Config.myTestDevice1) 
				//.AddTestDevice (Config.myTestDevice1Caps) 
			    .AddTestDevice (Config.myTestDevice2)
                .Build ();
			interstitial.LoadAd (request);
		}
	}

	void ShowInterstitial ()
	{	
		#if !UNITY_ANDROID && !UNITY_IPHONE && !UNITY_BLACKBERRY && !UNITY_WINRT || UNITY_EDITOR
		HandleOnAdFinished (this, null);
		#else
		if (this.interstitial.IsLoaded ()) {
			interstitial.Show ();
		} else {
			HandleOnAdFinished (this, null);
		}
		#endif
	}

	void HandleOnAdFailedToLoad (object sender, EventArgs args)
	{
		RequestInterstitial ();
	}

	void HandleOnAdFinished (object sender, EventArgs args)
	{
		this.interstitial.Destroy ();
		this.interstitial = null;
		interstitialWatched = true;
		switch (actionOnEnd) {
		case ActionEnum.main_menu:
			LoadMainMenu ();
			break;
		case ActionEnum.restart_level:
			ReloadScene (false);
			break;
		case ActionEnum.restart_from_checkpoint:
			ReloadScene (true);
			break;
		case ActionEnum.next_level:
			LoadNextScene ();
			break;
		default:
			LoadMainMenu ();
			break;
		}

	}

	public enum ActionEnum
	{
		main_menu,
		restart_level,
		next_level,
		restart_from_checkpoint
	}



	/*****************************************************************************/
	/*							DIALOGS											 */
	/*****************************************************************************/

	// display a line of a Dialog. Return false if the dialog is finished.
	public void DisplayDialog (DialogEnum dialogEnum)
	{	
		Dialog dialog = dialogDico [dialogEnum];
		gc.DisplayDialog (true); // pause the game
		dialogPanel.SetActive (true);
		DisplayTopUI (false);
		DisplayMobileController (true, false);
		dialogPanel.GetComponent<DialogController> ().dialog = dialog;
		dialogPanel.GetComponent<DialogController> ().DisplayDialog ();
	}

	public void FinishDialog ()
	{	
		gc.DisplayDialog (false);
		dialogPanel.SetActive (false);	
		DisplayTopUI ();
		DisplayMobileController ();
	}

	// Get the dialogs that have to be loaded for this level
	void InstantiateDialogs ()
	{
		dialogDico = Dialog.InstantiateDialogs (level.id);
	}
		
}
