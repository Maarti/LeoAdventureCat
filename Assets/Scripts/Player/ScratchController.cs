// http://answers.unity3d.com/questions/654836/unity2d-sprite-fade-in-and-out.html

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ScratchController : MonoBehaviour
{
	public float duration = 0.5f, minScale = 0.07f, maxScale = 0.14f,
		damage = 1f, bumpDuration = 0f;
	public Vector2 bumpVelocity = Vector2.zero;

	SpriteRenderer sprite;
	float startTime;
	bool enableCollider = true;
	Collider2D col;
	List<string> ennemiesHit = new List<string> ();

	// Use this for initialization
	void Start ()
	{
		startTime = Time.time;
		this.sprite = gameObject.GetComponent<SpriteRenderer> (); 
		this.col = gameObject.GetComponent<Collider2D> ();
		gameObject.transform.localScale = new Vector3 (Random.Range (minScale, maxScale), Random.Range (minScale, maxScale), gameObject.transform.localScale.z);
		sprite.flipX = (Random.value > 0.5f); // random boolean
		sprite.flipY = (Random.value > 0.5f); 
	}

	public void Init (float damage, Vector2 bumpVelocity, float bumpDuration)
	{
		this.damage = damage;
		this.bumpVelocity = bumpVelocity;
		this.bumpDuration = bumpDuration;
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Trigger the collider only on the first frame
		if (!enableCollider)
			this.col.enabled = false;
		else
			enableCollider = false;

		// Fade out
		float t = (Time.time - startTime) / duration;
		sprite.color = new Color (1f, 1f, 1f, Mathf.SmoothStep (1f, 0.0f, t));
		if (Time.time - startTime >= duration)
			Destroy (this.gameObject);
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		string layer = LayerMask.LayerToName (other.gameObject.layer);
		// if the ennemy has not been already hit by this attack
		if ((layer == "Destructible" || layer == "Enemy") && !ennemiesHit.Contains (other.name)) {
			ennemiesHit.Add (other.name);
			other.gameObject.GetComponent<IDefendable> ().Defend (this.gameObject, damage, bumpVelocity, bumpDuration);
		}

	}
}
