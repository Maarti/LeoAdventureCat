using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeController : MonoBehaviour
{
	public int damageOnCollision = 1;
	public Vector2 bumpOnCollision = new Vector2 (-2, 1);
    public bool collidWithRat = false;

    void OnCollisionStay2D (Collision2D other)
	{
		if (other.transform.tag == "Player")
			other.gameObject.GetComponent<PlayerController> ().Defend (this.gameObject, damageOnCollision, bumpOnCollision, 0.5f);

        else if (collidWithRat && other.transform.tag == "Rat")
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().Defend(this.gameObject, damageOnCollision, Vector2.zero, 0f);
    }
}
