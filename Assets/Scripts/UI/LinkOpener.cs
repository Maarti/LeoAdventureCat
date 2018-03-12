using UnityEngine;
using System.Collections;

public class LinkOpener : MonoBehaviour
{

	public void OpenLink (string localUrlId)
	{
#if UNITY_ANDROID
        if(localUrlId=="URL_PLAY_STORE")
            localUrlId = "URL_PLAY_STORE_ANDROID";
#endif
        string url = LocalizationManager.Instance.GetText (localUrlId);
		Debug.Log ("opening url " + url);
		Application.OpenURL (url);
	}
}