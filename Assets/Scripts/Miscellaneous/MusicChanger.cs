using UnityEngine;

public class MusicChanger : MonoBehaviour {

    public AudioClip newAudio;
    GameObject gameCtrlr;

    void Start () {
        gameCtrlr = GameObject.Find("GameController");
	}

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.tag == "Player") {
            AudioSource audioSource = gameCtrlr.GetComponent<AudioSource>();
            audioSource.Stop();
            audioSource.clip = newAudio;
            audioSource.Play();
            Destroy(this.gameObject);
        }
    }

}
