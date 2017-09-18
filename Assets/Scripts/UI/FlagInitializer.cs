using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlagInitializer : MonoBehaviour
{
	public int currentLang = 0;
	public Text[] textToRefresh;

	// Use this for initialization
	void Start ()
	{
		currentLang = ApplicationController.ac.playerData.lang_id;
		GetComponent<Dropdown> ().value = currentLang;
	}

	public void SetLanguage (int id)
	{
		LocalizationManager.Instance.SetLanguage (id, textToRefresh);
	}

}
