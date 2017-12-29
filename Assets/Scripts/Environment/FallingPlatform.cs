using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{

	public float timeBeforeFall = 1f;
    public bool fallOnCollision = true, fadeToBlack = false;
    float startTime;
    
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
        if (fadeToBlack)
        {
            startTime = Time.time;
            StartCoroutine(FadeToBlack());
        }
    }

	void Fall ()
	{
        if(fallOnCollision)
            transform.parent.GetComponent<Animator>().enabled = false; 
        else
            GetComponent<Animator>().enabled = false;
        GetComponent<Rigidbody2D> ().isKinematic = false;
        Destroy(this.gameObject, 4f);
	}

    IEnumerator FadeToBlack()
    {
        SpriteRenderer sprite = GetComponentInChildren<SpriteRenderer>();
        Color color = sprite.color;
        float coeff = 0f;
        while (coeff < 1f)
        {
            color.r = color.g = color.b = Mathf.Lerp(1f, 0.25f, coeff); 
            sprite.color = color;
            coeff = Mathf.Clamp((Time.time - startTime) / timeBeforeFall, 0f, 1f);
            yield return null;
        }
    }
}
