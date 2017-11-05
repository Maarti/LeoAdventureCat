using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	public bool isOnLoadingScene = false;

	void Start ()
	{
		if (isOnLoadingScene) {
			string sceneName = ApplicationController.ac.nextSceneToLoad;
			if (sceneName != "")
				LoadScene (sceneName);
			else
				LoadScene ("main_menu");
		}
	}

	public void LoadScene (string sceneName)
	{
		SceneManager.LoadScene (sceneName);
	}

	public static void LoadSceneWithLoadingScreen (string sceneName)
	{
		ApplicationController.ac.nextSceneToLoad = sceneName;
		SceneManager.LoadScene ("loading_scene");
	}


}
