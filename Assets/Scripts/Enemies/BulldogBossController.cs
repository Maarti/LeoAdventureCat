using UnityEngine;

public class BulldogBossController : MonoBehaviour, IDefendable
{
    public int life = 100, damageOnCollision =1;
    public Vector2 bumpOnCollision = new Vector2(-2, 2);
    public float speed = 1f;
    public bool isFacingRight = true;
    Animator animator;
    AudioSource audioSource;
    int state = 0;          // 0 = attack rat / 1 = attack cat
    GameObject cat;
    

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        cat = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        // chase cat
        if (state == 1)
        {
            Vector3 target = new Vector3(cat.transform.position.x, this.transform.position.y, this.transform.position.z);
            this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed/100);
            if ((target.x < this.transform.position.x && isFacingRight) ||
                (target.x > this.transform.position.x && !isFacingRight))
                Flip();
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.transform.tag == "Player")
            other.gameObject.GetComponent<PlayerController>().Defend(this.gameObject, damageOnCollision, bumpOnCollision, 0.5f);
    }

    public virtual void Defend(GameObject attacker, int damage, Vector2 bumpVelocity, float bumpTime)
    {
        this.life -= damage;
        if (life <= 0)
            Die();
        else
            animator.SetTrigger("hit");
    }

    // Called when life = 0, set transparent and launch the dying animation
    public virtual void Die()
    {
        animator.SetTrigger("die");
        this.gameObject.layer = LayerMask.NameToLayer("Transparent");
        audioSource.Stop();
        //audioSource.PlayOneShot(dyingSound);
        //GameObject.Destroy(this.gameObject, 5f);
    }

    public void ChaseCat()
    {
        this.state = 1;
    }

    public void StopChaseCat()
    {
        this.state = 0;
    }

    public void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = new Vector3(this.transform.localScale.x * -1, this.transform.localScale.y, this.transform.localScale.z);
        this.transform.localScale = theScale;
    }
}
