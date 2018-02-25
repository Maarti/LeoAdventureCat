using UnityEngine;

public class ObstacleController : MonoBehaviour {
   
    public int damage = 1;

    bool hit = false;
    Animator anim;
    Transform player;
    AudioSource audioS;
 
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        audioS = GetComponent<AudioSource>();
    }

    private void Update() {
        if (anim)
            anim.SetFloat("player.x", player.position.x - transform.position.x);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(hit || other.tag != "Player")
            return;
       
        Hit(other.gameObject);
    }
   
    void Hit(GameObject other){
        other.GetComponent<IDefendable> ().Defend (this.gameObject, damage, Vector3.zero, 0f);
        hit = true;
        if (anim)
            anim.SetTrigger("hit");
        if (audioS)
            audioS.Play();
        foreach(EyesTracker eye in GetComponentsInChildren<EyesTracker>()) {
            eye.hit = true;
        }
    }
}
