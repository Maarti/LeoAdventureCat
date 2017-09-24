using UnityEngine;
using System.Collections;

public class WaspController : AbstractEnemy
{
	public float respawnDartTime = 1.5f;
	public Transform dartPrefab;

	//Animator animator;
	SpriteRenderer dartSprite;

	/*void Awake ()
	{
		animator = GetComponent<Animator> ();
	}*/

	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();
		dartSprite = GameObject.Find (this.name + "/Body/Abdomen/Dart").GetComponent<SpriteRenderer> ();
	}


	public override void Attack ()
	{
		base.Attack ();
		rb.velocity = Vector2.zero;
		animator.SetTrigger ("attack");
	}

	// Called by "wasp_attack" animation
	void ThrowDart ()
	{
		Transform dartProjectile = (Transform)Instantiate (dartPrefab, dartSprite.transform.position, dartSprite.transform.rotation);
		dartSprite.color = new Color (1f, 1f, 1f, 0.0f); // hiding wasp dart
		StartCoroutine (RespawnDart (respawnDartTime));
		isMoving = true;
		ProjectileController dart = dartProjectile.GetComponent<ProjectileController> ();
		dart.timeToLive = 3;
		if (!facingRight)
			dart.speed *= -1;
		else {
			Vector3 scale = dartProjectile.localScale;
			scale.x *= -1;
			dartProjectile.localScale = scale;
		}
	}

	IEnumerator RespawnDart (float time)
	{
		yield return new WaitForSeconds (0.5f); // waiting time before respawning dart
		Color newColor = dartSprite.color;
		for (float t = 0.0f; t <= 1.0f; t += Time.deltaTime / time) {			
			newColor.a = Mathf.Lerp (0, 1, t);
			dartSprite.color = newColor;
			yield return null;
		}
		newColor.a = 1;
		dartSprite.color = newColor;
		isAtacking = false; // ready to attack again
	}

	// to delete when all enemies's animator have "hit" trigger
	/*public override void Defend (GameObject attacker, int damage, Vector2 bumpVelocity, float bumpTime)
	{
		animator.SetTrigger ("hit");
		base.Defend (attacker, damage, bumpVelocity, bumpTime);
	}*/
}
