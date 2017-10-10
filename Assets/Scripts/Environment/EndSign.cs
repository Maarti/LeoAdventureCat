using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSign : MonoBehaviour
{
	//public LevelEnum currentLevel = LevelEnum.level_1_01;
	protected GameUIController guic;

	protected virtual void Start ()
	{
		guic = GameObject.Find ("Canvas/GameUI").GetComponent<GameUIController> ();
	}

	protected virtual void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player") {
			//ApplicationController.ac.FinishLevel (currentLevel, Mathf.FloorToInt (GameController.gc.CalculateScore ()));
			guic.EndGame ();
			/*
			if (levelToLoad == LevelEnum.main_menu || levelToLoad == LevelEnum.none || ApplicationController.ac.levels [levelToLoad].isLocked)
				SceneManager.LoadScene ("main_menu");
			else
				SceneManager.LoadScene (levelToLoad.ToString ());
				*/
		}
	}
}
