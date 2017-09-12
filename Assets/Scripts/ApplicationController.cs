using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationController : MonoBehaviour
{

	public static ApplicationController ac;

	void Awake ()
	{
		if (ac == null) {
			DontDestroyOnLoad (gameObject);
			ac = this;
		} else if (ac != this) {
			Destroy (gameObject);
		}
	}
}
