using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUIController : MonoBehaviour
{
	public GameObject pausePanel, mobileController, dialogPanel;
	public bool gamePaused = false;
	public Transform heartPrefab;
	public AudioClip scoreSound;
	GameController gc;
	Text kittyzTxt, timeTxt, lifeTxt, scoreTxt, targetKittyzTxt, targetTimeTxt, targetLifeTxt, scoreLabelTxt;
	bool /*isStarted = false,*/ targetsInited = false;
	GameObject lifeBar, buttons_1, buttonNext, buttonResume, pauseTitle;
	RectTransform blocScore;
	Dictionary<DialogEnum,Dialog> dialogDico;
	Level level;
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
		blocScore = GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/Scores").GetComponent<RectTransform> ();
		buttons_1 = GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/Buttons_1");
		buttonNext = GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/Buttons_2/NextLevelButton");
		buttonResume = GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/Buttons_2/ResumeButton");
		pauseTitle = GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/Title");
		kittyzTxt = GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/Scores/ScoreKittyz/Score").GetComponent<Text> ();
		timeTxt = GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/Scores/ScoreTime/Score").GetComponent<Text> ();
		lifeTxt = GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/Scores/ScoreLife/Score").GetComponent<Text> ();
		scoreTxt = GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/TotalScore").GetComponent<Text> ();
		scoreLabelTxt = GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/LabelScore").GetComponent<Text> ();
		//TopUI
		lifeBar = GameObject.Find ("Canvas/" + this.name + "/TopUI/LifeBar");
		//Targets
		targetKittyzTxt = GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/Scores/ScoreKittyz/Target").GetComponent<Text> ();
		targetTimeTxt = GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/Scores/ScoreTime/Target").GetComponent<Text> ();
		targetLifeTxt = GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/Scores/ScoreLife/Target").GetComponent<Text> ();
		//LevelTitle
		GameObject.Find ("Canvas/" + this.name + "/LevelTitle").GetComponent<Text> ().text = level.GetFullName ();

		InstantiateDialogs ();
		//isStarted = true;
	}

	void InitScores (bool gameFinished = false)
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
		} else {
			scoreTxt.enabled = false;
			scoreLabelTxt.enabled = false;
			buttonNext.SetActive (false);
			buttonResume.SetActive (true);
		}
		/*GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/Scores/ScoreKittyz/Target").GetComponent<Text> ().text = "/" + gc.targetKittyz.ToString ();
		GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/Scores/ScoreTime/Target").GetComponent<Text> ().text = "/" + gc.targetTime.ToString () + "s";
		GameObject.Find ("Canvas/" + this.name + "/PauseMenuPanel/Scores/ScoreLife/Target").GetComponent<Text> ().text = "(" + gc.targetLife.ToString () + "max)";*/
		if (!targetsInited) {
			targetKittyzTxt.text = "/" + gc.targetKittyz.ToString ();
			targetTimeTxt.text = "/" + gc.targetTime.ToString () + "s";
			targetLifeTxt.text = "(" + gc.targetLife.ToString () + " max)";
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

	public void EndGame ()
	{
		gc.EndGame ();
		GameObject.Find ("Canvas/" + this.name + "/TopUI").SetActive (false);
		buttons_1.SetActive (false);
		blocScore.offsetMax = new Vector2 (blocScore.offsetMax.x, -100);
		pausePanel.SetActive (true);
		InitScores (true);
		if (mobileController)
			mobileController.SetActive (false);
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
		GetComponent<AudioSource> ().PlayOneShot (scoreSound);
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
		Time.timeScale = 1f;
		Level nextLevel = gc.level.GetNextUnlockedLevel ();
		SceneManager.LoadScene (nextLevel.id.ToString ());
	}

	// display a line of a Dialog. Return false if the dialog is finished.
	public void DisplayDialog (DialogEnum dialogEnum)
	{	
		Dialog dialog = dialogDico [dialogEnum];
		gc.DisplayDialog (true); // pause the game
		dialogPanel.SetActive (true);
		dialogPanel.GetComponent<DialogController> ().dialog = dialog;
		dialogPanel.GetComponent<DialogController> ().DisplayDialog ();

		/*if (dialog.isFinished) {
			gc.DisplayDialog (false);
			dialogPanel.SetActive (false);
			return false;
		} else {
			gc.DisplayDialog (true); // pause the game
			DialogLine dl = dialog.ReadLine ();
			dialogPanel.SetActive (true);
			dialName.text = LocalizationManager.Instance.GetText (dl.nameStringId);
			dialText.text = LocalizationManager.Instance.GetText (dl.textStringId);
			dialPortrait.sprite = dl.portrait;
			return true;
		}*/

	}

	public void FinishDialog ()
	{	
		gc.DisplayDialog (false);
		dialogPanel.SetActive (false);	
	}

	// Manage the dialogs that have to be loaded for each level
	void InstantiateDialogs ()
	{
		dialogDico = new Dictionary<DialogEnum,Dialog> ();
		switch (level.id) {
		case LevelEnum.level_1_story:
			Sprite portraitLeo = Resources.Load ("Portraits/leo", typeof(Sprite)) as Sprite;
			List<DialogLine> dl = new List<DialogLine> () {
				new DialogLine ("TUTO_JUMP", "LEO", portraitLeo)
			};
			dialogDico.Add (DialogEnum.tuto_jump, new Dialog (dl));
			dl = new List<DialogLine> () {
				new DialogLine ("TUTO_ATTACK_1", "LEO", portraitLeo),
				new DialogLine ("TUTO_ATTACK_2", "LEO", portraitLeo),
				new DialogLine ("TUTO_ATTACK_3", "LEO", portraitLeo)
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
			/* to delete*/
			dl = new List<DialogLine> () {
				new DialogLine ("IN_PROGRESS", "LEO", portraitLeo)
			};
			dialogDico.Add (DialogEnum.in_progress, new Dialog (dl));
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
				new DialogLine ("DOG_CATCHER_START", "LEO", portraitLeo)
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
