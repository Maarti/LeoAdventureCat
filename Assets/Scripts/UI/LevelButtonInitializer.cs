using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonInitializer : MonoBehaviour
{
	public LevelEnum levelEnum = LevelEnum.level_1_01;

	Level level;
	GameObject uiDifficulty, uiLockImage, uiScore;
	Text uiName;
	Button button;
	bool isStarted = false;

	// Use this for initialization
	void Start ()
	{
		Debug.Log (this.name + " = " + Time.time + " " + ApplicationController.ac.levels [levelEnum].name);
		//level = ApplicationController.ac.levels [levelEnum];
		uiName = transform.Find ("Name").gameObject.GetComponent<Text> ();
		uiLockImage = transform.Find ("LockImg").gameObject;
		uiDifficulty = transform.Find ("Difficulty").gameObject;
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
		}
		uiName.text = LocalizationManager.Instance.GetText ("LEVEL") + " " + level.name;
	}

	void OnEnable ()
	{
		if (isStarted)
			Start ();
	}

}
