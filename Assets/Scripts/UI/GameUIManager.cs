using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
	public GameObject pausePanel, mobileController;

	bool isPaused = false;

	public void TooglePause ()
	{
		PauseGame (!isPaused);
	}

	public void PauseGame (bool pause = true)
	{		
		if (pause) {
			Time.timeScale = 0f;
			isPaused = true;
			pausePanel.SetActive (true);
			if (mobileController)
				mobileController.SetActive (false);
		} else {
			Time.timeScale = 1f;
			isPaused = false;
			pausePanel.SetActive (false);
			if (mobileController)
				mobileController.SetActive (true);
		}
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
