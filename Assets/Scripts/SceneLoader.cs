using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
	public bool isOnLoadingScene = false;
	public GameObject[] characters;
	public Text progressText;

	void Start ()
	{
		if (isOnLoadingScene) {
			// Load the next scene
			string sceneName = ApplicationController.ac.nextSceneToLoad;
			progressText.text = "0%";
			if (sceneName != "")
				//LoadScene (sceneName);
				StartCoroutine (LoadAsyncScene (sceneName));
			else
				LoadScene ("main_menu");

			// Activate one character to display
			GameObject character = characters [Random.Range (0, characters.Length)];
			character.SetActive (true);

			// Animate character
			AnimateCharacter (character);
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


	IEnumerator LoadAsyncScene (string sceneName)
	{
		// The Application loads the Scene in the background at the same time as the current Scene.
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync (sceneName);

		//Wait until the last operation fully loads to return anything
		while (!asyncLoad.isDone) {
			// I multiply the progress by 110 (and not 100) because the progress is stuck at 0.9 (=> 90%), so now it's 99%.
			int progress = Mathf.Clamp (Mathf.RoundToInt (asyncLoad.progress * 110), 0, 100);
			progressText.text = progress.ToString () + "%";
			yield return null;
		}
	}

	void AnimateCharacter (GameObject character)
	{
        Debug.Log("char=" + character.name);
        switch (character.name) {
		case "Cat":
			character.GetComponent<Animator> ().SetFloat ("speed", 1f);
			break;
		case "BarkingDog":
			character.GetComponent<Animator> ().SetFloat ("x.velocity", 1f);
			break;
        case "WatchDog":
                Debug.Log("entered" );
                character.GetComponent<Animator>().SetBool("isPlaying", true);
            break;
        default:
			break;
		}
	}
}
