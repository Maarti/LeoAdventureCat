using UnityEngine;

public class BulldogBossController : MonoBehaviour, IDefendable
{
    public int life = 100, damageOnCollision =1;
    public Vector2 bumpOnCollision = new Vector2(-2, 2);
    Animator animator;
    AudioSource audioSource;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
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
}
