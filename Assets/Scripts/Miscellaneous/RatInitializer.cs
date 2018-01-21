using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatInitializer : MonoBehaviour {

    public bool isEating = false;
    Animator anim;
	
	void Start () {
        anim = GetComponent<Animator>();
        anim.SetBool("isEating", isEating);
	}
	
}
