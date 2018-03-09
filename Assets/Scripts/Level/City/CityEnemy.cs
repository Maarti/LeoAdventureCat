using UnityEngine;

public class CityEnemy : MonoBehaviour {

    public Transform dartPrefab;
    public GameObject dart;

    Animator anim;
    
    void Start () {
        anim = GetComponent<Animator>();
    }


    void ReadyIn() {
        Invoke("IsReady", 2f);
    }

    void IsReady() {
        anim.SetTrigger("attack");
    }

    void Attack() {
        Transform dartProjectile = (Transform)Instantiate(dartPrefab, dart.transform.position, dart.transform.rotation);
        ProjectileController proj = dartProjectile.GetComponent<ProjectileController>();
        proj.timeToLive = 3;
        proj.speed *= -1.75f;        
        dartProjectile.parent = this.transform.parent;
        Destroy(dart);
        anim.SetTrigger("outro");
        Destroy(this.gameObject, 5f);
    }
}
