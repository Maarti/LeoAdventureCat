using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
	public Sprite imgOn, imgOff;
	Image soundImg;

	public void Start ()
	{
		soundImg = GetComponent<Image> ();
		if (ApplicationController.ac.playerData.isMute)
			soundImg.sprite = imgOff;
		else
			soundImg.sprite = imgOn;
	}

	public void ToggleSound ()
	{
		bool isMute = !ApplicationController.ac.playerData.isMute;
		Debug.Log ("mute = " + isMute.ToString ());
		ApplicationController.ac.playerData.isMute = isMute;
		AudioListener.volume = isMute ? 0f : 1f;
		if (isMute)
			soundImg.sprite = imgOff;
		else
			soundImg.sprite = imgOn;
	}
}
