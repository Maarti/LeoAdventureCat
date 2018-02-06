using UnityEngine;

public class ThornController : MonoBehaviour
{
	public int damageOnCollision = 1;
	public Vector2 bumpOnCollision = new Vector2 (-2, 3);
	public float animSpeed;

	// Use this for initialization
	void Start ()
	{
        if(animSpeed!=0f)
		    GetComponent<Animator> ().speed = animSpeed;
	}

	// Bump player if touched
	void OnCollisionEnter2D (Collision2D other)
	{
		if (other.transform.tag == "Player")
			other.gameObject.GetComponent<PlayerController> ().Defend (this.gameObject, damageOnCollision, bumpOnCollision, 0.5f);
	}
    
}