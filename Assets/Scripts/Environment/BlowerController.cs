using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowerController : MonoBehaviour {

    public GameObject aoe;
    public float waitingTime=3f, blowingTime = 1.5f;
    Animator animator;
    float startWaitingTime, startBlowingTime;
    bool isWaiting = true, isBlowing=false;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        startWaitingTime = Time.fixedTime;
        startBlowingTime = Time.fixedTime;
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
        aoe.SetActive(true);
    }

    void StopBlowing()
    {
        isBlowing = false;
        aoe.SetActive(false);
        startWaitingTime = Time.fixedTime;
    }
}
