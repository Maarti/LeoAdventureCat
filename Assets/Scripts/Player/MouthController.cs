using UnityEngine;
using System.Collections;

public class MouthController : MonoBehaviour
{

	public AudioClip jumpAudio, biteAudio, hitAudio;

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
		case "hit":
			audio = hitAudio;
			break;
		default:
			return;
		}

		if (audioSource.isPlaying)
			audioSource.Stop ();						//stop playing current sound
		audioSource.PlayOneShot (audio);			//play audio
		StartCoroutine (CloseMouthIn (audio.length));//close mouth when finished
	}

	IEnumerator CloseMouthIn (float timeInSeconds)
	{
		yield return new WaitForSeconds (timeInSeconds);
		sprite.enabled = false;
	}
}
