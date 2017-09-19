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
		if (ApplicationController.ac.playerData.isMute) {
			EnableSound (false);
			soundImg.sprite = imgOff;
		} else {
			EnableSound ();
			soundImg.sprite = imgOn;
		}
	}

	public void ToggleSound ()
	{
		bool isMute = !ApplicationController.ac.playerData.isMute;
		Debug.Log ("mute = " + isMute.ToString ());
		ApplicationController.ac.playerData.isMute = isMute;
		EnableSound (!isMute);
		if (isMute)
			soundImg.sprite = imgOff;
		else
			soundImg.sprite = imgOn;
	}

	void EnableSound (bool enable = true)
	{
		AudioListener.volume = enable ? 1f : 0f;
	}
}
