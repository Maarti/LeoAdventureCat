using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInitializer : MonoBehaviour
{
	public LevelEnum levelEnum = LevelEnum.level_1_01;

	Level level;
	GameObject /*uiName, uiDifficulty,*/ uiLockImage;

	// Use this for initialization
	void Start ()
	{
		level = ApplicationController.ac.levels [levelEnum];
		//uiName = transform.Find ("Name").gameObject;
		uiLockImage = transform.Find ("LockImg").gameObject;
		//uiDifficulty = transform.Find ("Difficulty").gameObject;
		InitButton ();
	}

	void InitButton ()
	{
		if (level.isLocked) {
			GetComponent<Button> ().interactable = false;
			uiLockImage.GetComponent<Image> ().color = Color.red;
		} else if (level.isCompleted)
			uiLockImage.GetComponent<Image> ().color = Color.green;
		else
			uiLockImage.GetComponent<Image> ().color = Color.yellow;
	}

}
