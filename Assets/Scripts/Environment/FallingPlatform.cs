using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{

	public float timeBeforeFall = 1f;
    public bool fallOnCollision = true;
    
	void OnCollisionEnter2D (Collision2D other)
	{
		if (fallOnCollision && other.transform.tag == "Player") {
            Trigger();
		}
	}

    public void Trigger()
    {
        if(fallOnCollision)
            transform.parent.GetComponent<Animator>().SetTrigger("collision"); 
        else
            GetComponent<Animator>().SetTrigger("collision");
        Invoke("Fall", timeBeforeFall);
    }

	void Fall ()
	{
        if(fallOnCollision)
            transform.parent.GetComponent<Animator>().enabled = false; 
        else
            GetComponent<Animator>().enabled = false;
        GetComponent<Rigidbody2D> ().isKinematic = false;
	}
}
