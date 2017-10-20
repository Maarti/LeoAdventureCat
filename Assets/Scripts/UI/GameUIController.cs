using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;

public class GameUIController : MonoBehaviour
{
	public GameObject pausePanel, mobileController, dialogPanel;
	public bool gamePaused = false, gameFinished = false;
	public Transform heartPrefab;
	public AudioClip scoreSound, pauseSound, winSound;
	GameController gc;
	Text kittyzTxt, timeTxt, lifeTxt, scoreTxt, targetKittyzTxt, targetTimeTxt, targetLifeTxt, scoreLabelTxt, totalKittyzText;
	bool targetsInited = false, interstitialWatched = false;
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
		//TopUI
		topUI = GameObject.Find ("Canvas/" + this.name + "/TopUI");
		lifeBar = GameObject.Find ("Canvas/" + this.name + "/TopUI/LifeBar");
		//Targets
		targetKittyzTxt = GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/Scores/ScoreKittyz/Target").GetComponent<Text> ();
		targetTimeTxt = GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/Scores/ScoreTime/Target").GetComponent<Text> ();
		targetLifeTxt = GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/Scores/ScoreLife/Target").GetComponent<Text> ();
		//LevelTitle
		GameObject.Find ("Canvas/" + this.name + "/LevelTitle").GetComponent<Text> ().text = level.GetFullName ();
		//Checkpoints
		checkpointController = GameObject.Find ("CheckPointController");

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
		} else if (gameOver) {
			pauseTitle.GetComponent<LocalizationUIText> ().key = "GAME_OVER";/* enabled = false;
			pauseTitle.GetComponent<Text> ().text = LocalizationManager.Instance.GetText ("COMPLETED");*/
			scoreTxt.enabled = false;
			scoreLabelTxt.enabled = false;
			buttonNext.SetActive (false);
			buttonResume.SetActive (false);
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
	}

	public void BuyItem (int item)
	{
		switch (item) {
		case 0: // LIFE
			if (ApplicationController.ac.playerData.kittyz >= 5 && ApplicationController.ac.playerData.max_life > gc.pc.life) { //item price hardcoded
				gc.PlayerInjured (-1);
				ApplicationController.ac.playerData.updateKittys (-5, totalKittyzText, true);
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
		GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/ShopList/LifeItemPanel/BuyButton").GetComponent<Button> ().interactable = false;
		yield return new WaitForSeconds (2f);
		pausePanel.SetActive (true);
		InitScores (false, true);
	}

	public void ReloadScene (bool fromCheckpoint = false)
	{
		if (gameFinished && !interstitialWatched) {
			EndGameAction (ActionEnum.restart_level);
		} else {
			PauseGame (false);
			if (!fromCheckpoint)
				Destroy (checkpointController);
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		}
	}

	public void LoadMainMenu ()
	{
		if (gameFinished && !interstitialWatched) {
			EndGameAction (ActionEnum.main_menu);
		} else {
			PauseGame (false);
			Destroy (checkpointController);
			SceneManager.LoadScene ("main_menu");
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
			SceneManager.LoadScene (nextLevel.id.ToString ());
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
		//string adUnitIdInterstitial = Config.adUnitIdInterstitial;
		string adUnitIdInterstitial = "ca-app-pub-3940256099942544/1033173712"; // test ad
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
				.AddTestDevice (AdRequest.TestDeviceSimulator)
				.AddTestDevice (Config.myTestDevice1) 
				.AddTestDevice (Config.myTestDevice1Caps) 
			                    //.AddTestDevice (Config.myTestDevice2) 
				.Build ();
			interstitial.LoadAd (request);
		}
	}

	void ShowInterstitial ()
	{		
		if (this.interstitial.IsLoaded ()) {
			interstitial.Show ();
		} else {
			HandleOnAdFinished (this, null);
		}
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
			ReloadScene ();
			break;
		case ActionEnum.next_level:
			LoadNextScene ();
			break;
		default:
			LoadMainMenu ();
			break;
		}

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

	// Manage the dialogs that have to be loaded for each level
	void InstantiateDialogs ()
	{
		dialogDico = new Dictionary<DialogEnum,Dialog> ();
		switch (level.id) {
		case LevelEnum.level_1_story:
			Sprite portraitLeo = Resources.Load ("Portraits/leo", typeof(Sprite)) as Sprite;
			Sprite portraitDogCatcher = Resources.Load ("Portraits/dogcatcher", typeof(Sprite)) as Sprite;
			List<DialogLine> dl = new List<DialogLine> () {
				new DialogLine ("TUTO_JUMP", "LEO", portraitLeo)
			};
			dialogDico.Add (DialogEnum.tuto_jump, new Dialog (dl));
			dl = new List<DialogLine> () {
				new DialogLine ("TUTO_ATTACK", "LEO", portraitLeo)
			};
			dialogDico.Add (DialogEnum.tuto_attack, new Dialog (dl));
			dl = new List<DialogLine> () {
				new DialogLine ("TUTO_KITTYZ", "LEO", portraitLeo)
			};
			dialogDico.Add (DialogEnum.tuto_kittyz, new Dialog (dl));
			dl = new List<DialogLine> () {
				new DialogLine ("TUTO_ENNEMY_1", "LEO", portraitLeo),
				new DialogLine ("TUTO_ENNEMY_2", "LEO", portraitLeo)
			};
			dialogDico.Add (DialogEnum.tuto_ennemy, new Dialog (dl));
			dl = new List<DialogLine> () {
				new DialogLine ("TUTO_CHECKPOINT", "LEO", portraitLeo)
			};
			dialogDico.Add (DialogEnum.tuto_checkpoint, new Dialog (dl));
			dl = new List<DialogLine> () {
				new DialogLine ("FIRST_HEDGEHOG_1", "LEO", portraitLeo)
			};
			dialogDico.Add (DialogEnum.first_hedgehog_1, new Dialog (dl));
			dl = new List<DialogLine> () {
				new DialogLine ("FIRST_HEDGEHOG_2", "LEO", portraitLeo)
			};
			dialogDico.Add (DialogEnum.first_hedgehog_2, new Dialog (dl));
			dl = new List<DialogLine> () {
				new DialogLine ("FIRST_SQUIRREL", "LEO", portraitLeo)
			};
			dialogDico.Add (DialogEnum.first_squirrel, new Dialog (dl));
			dl = new List<DialogLine> () {
				new DialogLine ("DOG_CATCHER_START", "DOG_CATCHER", portraitDogCatcher)
			};
			dialogDico.Add (DialogEnum.dog_catcher_start, new Dialog (dl));
			break;
		default:
			break;
		}
	}
		
}


/*****************************************************************************/
/*							DIALOG CLASSES									 */
/*****************************************************************************/
public class DialogLine
{
	public Sprite portrait;
	public string nameStringId, textStringId;

	public DialogLine (string textStringId, string nameStringId, Sprite portrait)
	{
		this.textStringId = textStringId;
		this.nameStringId = nameStringId;
		this.portrait = portrait;
	}
}

public class Dialog
{

	public List<DialogLine> lines;
	public int currentLine = 0;
	public bool isFinished = false;

	public Dialog (List<DialogLine> dl)
	{
		this.lines = dl;
	}

	public DialogLine ReadLine ()
	{
		int ret = 0;
		if (currentLine < lines.Count) {
			ret = currentLine;
			if (currentLine == 0 && isFinished == true)
				isFinished = false;
			if (currentLine == lines.Count - 1) {
				isFinished = true;
				currentLine = 0;
			} else
				currentLine++;
			return lines [ret];
		} else {
			currentLine = 0;
			return lines [0];			
		}
	}

}

public enum DialogEnum
{
	tuto_jump,
	tuto_attack,
	tuto_kittyz,
	tuto_ennemy,
	tuto_checkpoint,
	in_progress,
	first_hedgehog_1,
	first_hedgehog_2,
	first_squirrel,
	dog_catcher_start
}

public enum ActionEnum
{
	main_menu,
	restart_level,
	next_level
}

