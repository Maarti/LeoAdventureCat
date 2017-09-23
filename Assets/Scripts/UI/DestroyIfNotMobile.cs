using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DestroyIfNotMobile : MonoBehaviour
{
	public float fadeTo = 0.25f, waitBeforeFade = 6f, speedFade = 0.2f;

	#if !UNITY_ANDROID && !UNITY_IPHONE && !UNITY_BLACKBERRY && !UNITY_WINRT || UNITY_EDITOR
	void Start ()
	{
		Destroy (this.gameObject);
	}
	#else
	void Start ()
	{
		if (SceneManager.GetActiveScene ().name != "level_1_story")
			FadeOutUI ();
	}
	#endif

	public void FadeOutUI ()
	{
		CanvasGroup cg = GetComponent<CanvasGroup> ();
		StartCoroutine (FadeOut (cg));
	}

	IEnumerator FadeOut (CanvasGroup cg)
	{
		yield return new WaitForSeconds (waitBeforeFade);
		float alpha = 1f;
		while (alpha >= fadeTo) {
			alpha -= Time.deltaTime * speedFade;
			cg.alpha = alpha;
			yield return null;
		}
		cg.alpha = fadeTo;
	}
}
