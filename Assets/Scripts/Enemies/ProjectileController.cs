using UnityEngine;
using System.Collections;

public class ProjectileController : MonoBehaviour
{
	public float speed = 1f, bumpTime = 0.5f, timeToLive = 3f;
	public int damage = 1;
	public Vector2 bumpVelocity = new Vector2 (-2, 3);
	public bool destroyOnCollision = true, hasRigidBody = false, fade = false;

	bool isFading = false;
	SpriteRenderer sprite;

	void Start ()
	{
		if (fade)
			StartCoroutine (FadeOut ());
		else
			GameObject.Destroy (this.gameObject, timeToLive);
		sprite = GetComponent<SpriteRenderer> ();
	}

	void FixedUpdate ()
	{
		if (!hasRigidBody)
			transform.position = new Vector3 (transform.position.x + speed / 10, transform.position.y, transform.position.z);
	}

	void OnCollisionEnter2D (Collision2D other)
	{
		if (!isFading) {
			if (other.transform.tag == "Player") {
				other.gameObject.GetComponent<IDefendable> ().Defend (this.gameObject, damage, bumpVelocity, bumpTime);
				if (destroyOnCollision)
					GameObject.Destroy (this.gameObject);
			}
		}
	}

	// FadeOut then destroys
	IEnumerator FadeOut ()
	{
		yield return new WaitForSeconds (timeToLive);
		isFading = true; //no damage when fading away
		Color newColor = sprite.color;
		for (float t = 0.0f; t <= 0.5f; t += Time.deltaTime / 1f) {			
			newColor.a = Mathf.Lerp (1, 0, t*2);
			sprite.color = newColor;
			yield return null;
		}
		GameObject.Destroy (this.gameObject);
	}
}
