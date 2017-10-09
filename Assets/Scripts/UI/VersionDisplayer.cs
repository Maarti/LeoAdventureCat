using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VersionDisplayer : MonoBehaviour
{
	void Start ()
	{
		GetComponent<Text> ().text = "Version " + Application.version.ToString ();	
	}
}
