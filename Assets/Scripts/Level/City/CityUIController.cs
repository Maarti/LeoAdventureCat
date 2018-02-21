using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;
using UnityEngine.Analytics;

public class CityUIController : MonoBehaviour
{
	public GameObject pausePanel, mobileController, gameoverPanel, tipPanel, tipButton, obsGenObj;
	public bool gamePaused = false, gameFinished = false;
	public Transform heartPrefab;
	public AudioClip scoreSound, pauseSound, winSound;
    public Slider distanceBar, speedBar;
	CityGameController gc;
	Text kittyzTxt, timeTxt, lifeTxt, scoreTxt, targetKittyzTxt, targetTimeTxt, targetLifeTxt, scoreLabelTxt, totalKittyzText, checkpointCountdown;
	bool targetsInited = false, interstitialWatched = false, isGameOver = false, progressBarsInit=false;
	GameObject lifeBar, buttons_1, buttonNext, buttonResume, pauseTitle, topUI, checkpointController, shoplist;
	Level level;
	AudioSource audioSource;
	InterstitialAd interstitial;
    ObstaclesGenerator obsGen;
	ActionEnum actionOnEnd = ActionEnum.main_menu;
    ConnectionTesterStatus connectionTestResult = ConnectionTesterStatus.Undetermined;


    void Start ()
	{
		gc = GameObject.Find ("CityGameController").GetComponent<CityGameController> ();
		// Get Level from Scene name
		string sceneName = SceneManager.GetActiveScene ().name;
		LevelEnum lvlEnum = (LevelEnum)Enum.Parse (typeof(LevelEnum), sceneName);
		level = ApplicationController.ac.levels [lvlEnum];
		//PausePanel
		shoplist = transform.Find("PauseMenuPanel/ShopList").gameObject;
		buttons_1 = transform.Find("PauseMenuPanel/Buttons_1").gameObject;
		totalKittyzText = transform.Find("PauseMenuPanel/Buttons_1/KittyzPanel/KittyzText").gameObject.GetComponent<Text> ();
        buttonNext = transform.Find("PauseMenuPanel/Buttons_2/NextLevelButton").gameObject;
		buttonResume = transform.Find("PauseMenuPanel/Buttons_2/ResumeButton").gameObject;
		pauseTitle = transform.Find("PauseMenuPanel/Title").gameObject;
		kittyzTxt = transform.Find("PauseMenuPanel/Scores/ScoreKittyz/Score").gameObject.GetComponent<Text> ();
		timeTxt = transform.Find("PauseMenuPanel/Scores/ScoreTime/Score").gameObject.GetComponent<Text> ();
		lifeTxt = transform.Find("PauseMenuPanel/Scores/ScoreLife/Score").gameObject.GetComponent<Text> ();
		scoreTxt = transform.Find("PauseMenuPanel/TotalScore").gameObject.GetComponent<Text> ();
		scoreLabelTxt = transform.Find("PauseMenuPanel/LabelScore").gameObject.GetComponent<Text> ();
		//GameOverPanel
		checkpointCountdown = transform.Find("GameOverPanel/Countdown/CheckpointCountdown").gameObject.GetComponent<Text> ();
		//TopUI
		topUI = transform.Find("TopUI").gameObject;
		lifeBar = transform.Find("TopUI/LifeBar").gameObject;
		//Targets
		targetKittyzTxt = transform.Find("PauseMenuPanel/Scores/ScoreKittyz/Target").gameObject.GetComponent<Text> ();
		targetTimeTxt = transform.Find("PauseMenuPanel/Scores/ScoreTime/Target").gameObject.GetComponent<Text> ();
		targetLifeTxt = transform.Find("PauseMenuPanel/Scores/ScoreLife/Target").gameObject.GetComponent<Text> ();
        //LevelTitle and difficulty
        GameObject levelTitle = transform.Find("LevelTitle").gameObject;
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
        //Obstacles
        obsGen = obsGenObj.GetComponent<ObstaclesGenerator>();

		audioSource = GetComponent<AudioSource> ();
		CreateInterstitial ();
	}

    private void Update() {
        if (!progressBarsInit)
            InitProgressBars();
        distanceBar.value = gc.pc.transform.position.x;
        speedBar.value = gc.pc.speed;
    }

    public void InitProgressBars() {
        distanceBar.minValue = 0f;
        distanceBar.maxValue = obsGen.distanceToTravel;
        distanceBar.value = gc.pc.transform.position.x;
        speedBar.minValue = 1f;
        speedBar.maxValue = gc.pc.maxSpeed;
        speedBar.value = gc.pc.speed;
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
			StartCoroutine ("AnimScore", score);
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

	IEnumerator AnimScore (int score)
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
        Debug.Log("CreateInterstitial=" + this.interstitial);
        RequestInterstitial ();
	}

	void RequestInterstitial ()
	{
        Debug.Log("Should RequestInterstitial ? interstitial="+interstitial);
        if (this.interstitial != null && !this.interstitial.IsLoaded ()) {
            Debug.Log("Yes ! RequestInterstitial() interstitialIsLoaded=" + this.interstitial.IsLoaded());
            AdRequest request = new AdRequest.Builder ()
			    .AddTestDevice(Config.myTestDevice2)
                .AddTestDevice(Config.myTestDevice3)
                .Build ();
			interstitial.LoadAd (request);
		}        
	}

	void ShowInterstitial ()
	{	
		#if !UNITY_ANDROID && !UNITY_IPHONE && !UNITY_BLACKBERRY && !UNITY_WINRT || UNITY_EDITOR
		HandleOnAdFinished (this, null);
#else
        Debug.Log("ShowInterstitial()="+this.interstitial);
        Debug.Log("internetReachability = " + Application.internetReachability.ToString());
		if (this.interstitial.IsLoaded ()) {
			Debug.Log("interstitial IsLoaded, let's show it");
            interstitial.Show ();
            Debug.Log("interstitial has been shown");
		} else {
            Debug.Log("interstitial is not loaded");
			HandleOnAdFinished (this, null);
		}
#endif
    }

    void HandleOnAdFailedToLoad (object sender, EventArgs args)
	{
        connectionTestResult = Network.TestConnection();        
        Debug.Log("FailedToLoad testConnection=" + connectionTestResult+ " and internetReachability=" + Application.internetReachability);
        if (Application.internetReachability == NetworkReachability.NotReachable)
            StartCoroutine(RequestInterstialAfterSeconds(10f));
        else
            StartCoroutine(RequestInterstialAfterSeconds(2f));
    }

    IEnumerator RequestInterstialAfterSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        RequestInterstitial();
    }

	void HandleOnAdFinished (object sender, EventArgs args)
	{
        Debug.Log("HandleOnAdFinished action="+actionOnEnd);
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
	
}
