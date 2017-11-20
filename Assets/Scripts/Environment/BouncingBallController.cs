using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingBallController : MonoBehaviour, IDefendable
{

	public Vector2 bumpVelocity = new Vector2 (0, 4);
    public float xDecelaration = 1f;

	Rigidbody2D rb;
    Vector2 velo;

    // Use this for initialization
    void Start ()
	{
		rb = GetComponent<Rigidbody2D> ();
	}

    void Update()
    {
        velo = rb.velocity;
        if (velo.x > 0)
            velo.x = Mathf.Clamp(velo.x - (xDecelaration * Time.deltaTime), 0f, 5f);
        else
            velo.x = Mathf.Clamp(velo.x + (xDecelaration * Time.deltaTime), -5f, 0f);
        rb.velocity = velo;
    }

	public void Defend (GameObject attacker, int damage, Vector2 attackerBumpVelocity, float bumpTime)
	{
		if (bumpVelocity != Vector2.zero && !rb.isKinematic) {
			if ((transform.position.x - attacker.transform.position.x) > 0) // if attacker come from the left, bump to right
					bumpVelocity.x *= -1;
			rb.velocity = bumpVelocity;
		}			
	}

	public void Reset ()
	{
		transform.parent.gameObject.GetComponent<WatchDogController> ().ResetBall ();
	}
}
