using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Text))]
public class LocalizationUIText : MonoBehaviour
{
	public string key;
	public int lang;
	public float reloadInSec = 0f;
	private bool isStarted = false;

	void Start ()
	{
		//lang = LocalizationManager.Instance.currentLanguageID;
		lang = ApplicationController.ac.playerData.lang_id;
		// Get the string value from localization manager from key & set text component text value to the returned string value
		Refresh ();

		// Refresh for the 1rst scene, to ba sure that the language has been loaded
		if (reloadInSec > 0)
			Invoke ("Refresh", reloadInSec);

		// Is started, so Enable() can be called
		isStarted = true;
	}


	public void Refresh ()
	{
		GetComponent<Text> ().text = LocalizationManager.Instance.GetText (key);
	}

	void OnEnable ()
	{
		if (isStarted)
			Refresh ();
	}
		
}