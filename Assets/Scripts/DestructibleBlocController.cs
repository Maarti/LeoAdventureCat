using UnityEngine;
using System.Collections;

public class DestructibleBlocController : MonoBehaviour
{

	public float life = 2.0f;
	public Transform explodePrefab;

	Animator animator;

	// Use this for initialization
	void Start ()
	{
	
	}

	void Awake ()
	{
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void Hit (float damage)
	{
		animator.SetTrigger ("hit");
		life -= damage;
		if (life <= 0) {
			Transform explosion = (Transform)Instantiate (explodePrefab, this.gameObject.transform.position, Quaternion.identity);
			Destroy (this.gameObject);
			Destroy (explosion.gameObject, 0.5f);
		}
	}
}
