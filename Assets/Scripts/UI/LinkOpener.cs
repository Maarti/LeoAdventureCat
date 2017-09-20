using UnityEngine;
using System.Collections;

public class LinkOpener : MonoBehaviour
{

	public void OpenLink (string localUrlId)
	{
		string url = LocalizationManager.Instance.GetText (localUrlId);
		Debug.Log ("opening url " + url);
		Application.OpenURL (url);
	}
}