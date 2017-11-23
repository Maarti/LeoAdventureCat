using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowerController : MonoBehaviour {

    public GameObject effectArea;
    public GameObject smoke;
    public float waitingTime=2f, blowingTime = 3f;
    public int damage = 1;
    public Vector2 bumpOnCollision = new Vector2(-1f, -1f);
    Animator animator;
    float startWaitingTime, startBlowingTime;
    bool isWaiting = true, isBlowing=false;
    ParticleSystem prtcle;
    AreaEffector2D aoe;
    AudioSource audio;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        startWaitingTime = Time.fixedTime;
        startBlowingTime = Time.fixedTime;
        prtcle = smoke.GetComponent<ParticleSystem>();
        aoe = effectArea.GetComponent<AreaEffector2D>();
        audio = transform.Find("Audio").gameObject.GetComponent<AudioSource>();
        StopBlowing();
    }
	
	// Update is called once per frame
	void Update () {
        if (isWaiting)
        {
            if (Time.fixedTime - startWaitingTime >= waitingTime)
            {
                isWaiting = false;
                Trigger();
            }
        }
        else if(isBlowing)
        {
            if(Time.fixedTime - startBlowingTime >= blowingTime)
            {
                isBlowing = false;
                isWaiting = true;
                StopBlowing();
            }
        }
	}

    void Trigger()
    {
        animator.SetTrigger("trigger");
    }

    // Called by animator
    public void StartBlowing()
    {
        startBlowingTime = Time.fixedTime;
        isBlowing = true;
        //aoe.SetActive(true);
        prtcle.Play();
        aoe.enabled = true;
        audio.Play();
    }

    void StopBlowing()
    {
        isBlowing = false;
        //aoe.SetActive(false);
        prtcle.Stop();
        aoe.enabled = false;
        startWaitingTime = Time.fixedTime;
        audio.Stop();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.transform.tag == "Player")
            collision.gameObject.GetComponent<PlayerController>().Defend(this.gameObject, damage, bumpOnCollision, 0.5f);
    }
}
