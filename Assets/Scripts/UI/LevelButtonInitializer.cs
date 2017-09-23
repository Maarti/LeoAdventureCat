using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelButtonInitializer : MonoBehaviour
{
	public LevelEnum levelEnum = LevelEnum.level_1_01;

	Level level;
	GameObject uiLockImage, uiScore;
	Text uiDifficulty, uiName;
	Button button;
	bool isStarted = false;

	// Use this for initialization
	void Start ()
	{
		level = ApplicationController.ac.levels [levelEnum];
		uiName = transform.Find ("Name").gameObject.GetComponent<Text> ();
		uiLockImage = transform.Find ("LockImg").gameObject;
		if (!level.isStory)
			uiDifficulty = transform.Find ("Difficulty").gameObject.GetComponent<Text> ();
		uiScore = transform.Find ("Score").gameObject;
		button = GetComponent<Button> ();
		InitButton ();
		// When object is Started, OnEnable() can be called
		isStarted = true;
	}

	void InitButton ()
	{
		level = ApplicationController.ac.levels [levelEnum];
		if (level.isLocked) {
			// TODO : change the onclick to shop menu if level locked
			button.interactable = false;
			uiLockImage.SetActive (true);
			uiScore.SetActive (false);
		} else {
			button.interactable = true;
			uiLockImage.SetActive (false);
			uiScore.SetActive (true);
			uiScore.GetComponent<Text> ().text = level.score.ToString () + "%";
			if (level.score >= 100) {
				uiScore.GetComponent<Text> ().color = Color.green; //1EFF00FF
				uiScore.GetComponent<Text> ().resizeTextForBestFit = true;
			}
		}
		if (!level.isStory)
			uiDifficulty.text = LocalizationManager.Instance.GetText (level.difficulty.ToString ());
		uiName.text = level.GetFullName ();
	}

	void OnEnable ()
	{
		if (isStarted)
			Start ();
	}

	public void LoadThisLevel ()
	{
		SceneManager.LoadScene (levelEnum.ToString ());
	}

}
