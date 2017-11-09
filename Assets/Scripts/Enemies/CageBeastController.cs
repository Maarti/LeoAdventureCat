using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageBeastController : MonoBehaviour
{
	public int damageOnCollision = 1;
	public Vector2 bumpOnCollision = new Vector2 (-2, 3);
	public float speed = 1f;

	bool readyToAttack = false, isAttacking = false;
	Animator animator;
	GameObject slash;
	AudioSource audioSource;

	// Use this for initialization
	void Start ()
	{
		animator = GetComponent<Animator> ();
		animator.SetFloat ("speed", speed);
		slash = transform.Find ("Slash").gameObject;
		audioSource = GetComponent<AudioSource> ();
	}

	void OnTriggerStay2D (Collider2D other)
	{
		if (readyToAttack && !isAttacking && other.transform.tag == "Player")
			Attack (other.gameObject);		
	}

	void Attack (GameObject target)
	{
		isAttacking = true;
		target.GetComponent<PlayerController> ().Defend (this.gameObject, damageOnCollision, bumpOnCollision, 0.5f);
		animator.SetTrigger ("attack");
		audioSource.Play ();

		//display slash
		slash.transform.position = target.transform.position;
		Debug.Log ("ready to attack = " + readyToAttack.ToString ());
		Color slashColor = slash.GetComponent<SpriteRenderer> ().color;
		slashColor.a = 1f;
		slash.GetComponent<SpriteRenderer> ().color = slashColor;
		StartCoroutine (SlashFadeOut ());
	}

	public void isReadyToAttack ()
	{
		this.readyToAttack = true;
	}

	public void isNotReadyToAttack ()
	{
		this.readyToAttack = false;
		this.isAttacking = false;
	}

	IEnumerator SlashFadeOut ()
	{
		Color newColor = slash.GetComponent<SpriteRenderer> ().color;
		for (float t = 0.0f; t <= 1.0f; t += Time.deltaTime / 1f) {			
			newColor.a = Mathf.Lerp (1, 0, t);
			slash.GetComponent<SpriteRenderer> ().color = newColor;
			yield return null;
		}
		newColor.a = 0f;
		slash.GetComponent<SpriteRenderer> ().color = newColor;
	}

}
