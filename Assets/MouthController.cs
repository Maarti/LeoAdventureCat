using UnityEngine;
using System.Collections;

public class MouthController : MonoBehaviour
{

	public AudioClip jumpAudio, biteAudio;

	AudioSource audioSource;
	SpriteRenderer sprite;

	void Start ()
	{
		audioSource = GetComponent<AudioSource> ();
		sprite = GetComponent<SpriteRenderer> ();
		sprite.enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void Meowing (string audioName)
	{
		AudioClip audio;
		sprite.enabled = true;	// open mouth

		// Select audio
		switch (audioName) {
		case "jump":
			audio = jumpAudio;
			break;
		default:
			return;
		}

		audioSource.PlayOneShot (audio);			//play audio
		StartCoroutine (CloseMouthIn (audio.length));//close mouth when finished
	}

	IEnumerator CloseMouthIn (float timeInSeconds)
	{
		yield return new WaitForSeconds (timeInSeconds);
		sprite.enabled = false;
	}
}
