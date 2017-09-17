using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonInitializer : MonoBehaviour
{
	public LevelEnum levelEnum = LevelEnum.level_1_01;

	Level level;
	GameObject /*uiName, uiDifficulty,*/ uiLockImage;
	Button button;

	// Use this for initialization
	void Start ()
	{
		level = ApplicationController.ac.levels [levelEnum];
		//uiName = transform.Find ("Name").gameObject;
		uiLockImage = transform.Find ("LockImg").gameObject;
		//uiDifficulty = transform.Find ("Difficulty").gameObject;
		button = GetComponent<Button> ();
		InitButton ();
	}

	void InitButton ()
	{
		if (level.isLocked) {
			button.interactable = false;
			uiLockImage.GetComponent<Image> ().color = Color.red;
		} else {
			button.interactable = true;
			uiLockImage.GetComponent<Image> ().color = Color.green;
		}
	}

	void OnEnable ()
	{
		Start ();
	}

}
