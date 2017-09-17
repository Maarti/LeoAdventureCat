using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Text))]
public class LocalizationUIText : MonoBehaviour
{
	public string key;

	void Start ()
	{
		// Get the string value from localization manager from key & set text component text value to the returned string value
		GetComponent<Text> ().text = LocalizationManager.Instance.GetText (key);
	}
}