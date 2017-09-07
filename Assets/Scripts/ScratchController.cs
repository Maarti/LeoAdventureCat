// http://answers.unity3d.com/questions/654836/unity2d-sprite-fade-in-and-out.html

using UnityEngine;
using System.Collections;

public class ScratchController : MonoBehaviour
{

	SpriteRenderer sprite;
	private float startTime;
	public float duration = 0.5f, minScale = 0.07f, maxScale = 0.14f;

	// Use this for initialization
	void Start ()
	{
		startTime = Time.time;
		sprite = gameObject.GetComponent<SpriteRenderer> (); 
		gameObject.transform.localScale = new Vector3 (Random.Range (minScale, maxScale), Random.Range (minScale, maxScale), gameObject.transform.localScale.z);
		sprite.flipX = (Random.value > 0.5f); // random boolean
		sprite.flipY = (Random.value > 0.5f); 
	}
	
	// Update is called once per frame
	void Update ()
	{
		float t = (Time.time - startTime) / duration;
		sprite.color = new Color (1f, 1f, 1f, Mathf.SmoothStep (1f, 0.0f, t));
		if (Time.time - startTime >= duration)
			Destroy (this.gameObject);
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		GameObject obj = other.gameObject;
		if (obj.layer == LayerMask.NameToLayer ("Destructible"))
			other.GetComponent<DestructibleBlocController> ().Hit (1.0f);
		else if (obj.layer == LayerMask.NameToLayer ("Enemy"))
			GameObject.Destroy (other.gameObject);
	}
}
