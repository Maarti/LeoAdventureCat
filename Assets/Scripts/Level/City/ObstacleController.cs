using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour {
   
    public int damage = 1;
    bool hit = false;
    Animator anim;
 
    void Start () {
        //anim = GetComponent<Animator>();
    }
 
    void OnTriggerEnter2D(Collider2D other) {
        if(hit || other.tag != "Player")
            return;
       
        Hit(other.gameObject);
    }
   
    void Hit(GameObject other){
        other.GetComponent<IDefendable> ().Defend (this.gameObject, damage, Vector3.zero, 0f);
        hit = true;
        //anim.setTrigger("hit");      
    }
}
