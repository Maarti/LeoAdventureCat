using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlagInitializer : MonoBehaviour
{
	public int currentLang = 0;

	// Use this for initialization
	void Start ()
	{
		//currentLang = LocalizationManager.Instance.currentLanguageID;
		currentLang = ApplicationController.ac.playerData.lang_id;
		GetComponent<Dropdown> ().value = currentLang;
	}

}
