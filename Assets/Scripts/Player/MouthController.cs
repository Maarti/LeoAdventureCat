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
		audioSource.pitch = 1f;

		// Select audio
		switch (audioName) {
		case "jump":
			audioSource.pitch = Random.Range (0.95f, 1.2f);
			audio = jumpAudio;
			break;
		case "hit":
			audioSource.pitch = Random.Range (0.9f, 1.1f);
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
