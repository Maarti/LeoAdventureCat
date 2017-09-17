using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Text))]
public class LocalizationUIText : MonoBehaviour
{
	public string key;
	public int lang;
	public float reloadInSec = 0f;

	void Start ()
	{
		lang = LocalizationManager.Instance.currentLanguageID;
		// Get the string value from localization manager from key & set text component text value to the returned string value
		Refresh ();
		if (reloadInSec > 0)
			Invoke ("Refresh", reloadInSec);
	}


	public void Refresh ()
	{
		GetComponent<Text> ().text = LocalizationManager.Instance.GetText (key);
	}
		
}