using UnityEngine;

public class KittyzController : MonoBehaviour
{
	public int value = 1;
	public float speedOnCollect = 8f;
	AudioSource audioSource;
	bool isCollected = false;

	void Start ()
	{
		audioSource = GetComponent<AudioSource> ();
	}

	void Update ()
	{
		if (isCollected)
			transform.position = new Vector3 (transform.position.x, transform.position.y + Time.deltaTime * speedOnCollect, transform.position.z);
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (!isCollected && other.transform.tag == "Player") {
			other.gameObject.GetComponent<IKittyzCollecter> ().CollectKittyz (this.value);
			GetCollected ();
		}
	}

	void GetCollected ()
	{
		isCollected = true;
		GetComponent<Collider2D> ().enabled = false;
		transform.parent.gameObject.GetComponent<Animator> ().SetTrigger ("collected");
		CheckPointController.cc.KittyzCollected (this.transform.parent.name);
		audioSource.PlayOneShot (audioSource.clip);
		Destroy (gameObject.transform.parent.gameObject, 1f);
	}
}

