using UnityEngine;

public class SpikeSpinningShelf : MonoBehaviour {

    public float waitingTime = 3f;
    public bool isSpikeDown = true;

    float lastSpin;
    Animator anim;
    
	void Start () {
        anim = GetComponent<Animator>();
        anim.SetBool("isSpikeDown", isSpikeDown);
        lastSpin = Time.time;
	}
	
	void Update () {
        if (Time.time >= lastSpin + waitingTime)
            Spin();
	}

    void Spin()
    {
        isSpikeDown = !isSpikeDown;
        anim.SetBool("isSpikeDown", isSpikeDown);
        lastSpin = Time.time;
    }
}
