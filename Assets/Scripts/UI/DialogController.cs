using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
	public Dialog dialog;
	GameUIController guic;
	Text dialName, dialText;
	Image dialPortrait;
	bool isStarted = false;
	AudioSource audioSource;

	void Start ()
	{
		if (!isStarted) {
			guic = GameObject.Find ("Canvas/GameUI").GetComponent<GameUIController> ();
			dialName = transform.Find ("Name").gameObject.GetComponent<Text> ();// GameObject.Find ("Canvas/" + this.name + "/DialogPanel/Name").GetComponent<Text> ();
			dialPortrait = transform.Find ("Portrait").gameObject.GetComponent <Image> ();// GameObject.Find("Canvas/" + this.name + "/DialogPanel/Portrait").GetComponent<Image> ();
			dialText = transform.Find ("TextScroll/Viewport/Content/Text").gameObject.GetComponent <Text> (); //GameObject.Find ("Canvas/" + this.name + "/DialogPanel/TextScroll/Viewport/Content/Text").GetComponent<Text> ();
			audioSource = GetComponent<AudioSource> ();
			isStarted = true;
		}
	}

	public void DisplayDialog ()
	{
		if (!isStarted)
			Start ();
		if (dialog.isFinished) {
			guic.FinishDialog ();
		} else {
			DialogLine dl = dialog.ReadLine ();
			dialName.text = LocalizationManager.Instance.GetText (dl.nameStringId);
			dialText.text = LocalizationManager.Instance.GetText (dl.textStringId);
			dialPortrait.sprite = dl.portrait;
			if (dl.audio != null)
				this.audioSource.PlayOneShot (dl.audio);
		}
	}
}

