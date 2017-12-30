using UnityEngine;

public class BulldogBossController : MonoBehaviour, IDefendable
{
    public int life = 100, damageOnCollision =1;
    public Vector2 bumpOnCollision = new Vector2(-2, 2);
    public float speed = 1f, timeBetweenAttack = 1.5f;
    public bool isFacingRight = true;
    public AudioClip growlingSound, barkingSound, dyingSound;
    public GameObject stunAnimation;
    public delegate void DeathDelegate();
    public event DeathDelegate OnDeath;

    Animator animator;
    AudioSource audioSource;
    int state = 0;          // 0 = attack rat / 1 = attack cat
    GameObject cat;
    float lastAttackTime = 0f;
    

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        cat = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        // chase cat
        if (state == 1 && life>0)
        {
            if ((Time.time - lastAttackTime) > timeBetweenAttack)
            {
                Vector3 target = new Vector3(cat.transform.position.x, this.transform.position.y, this.transform.position.z);
                if ((target.x < this.transform.position.x && isFacingRight) ||
                        (target.x > this.transform.position.x && !isFacingRight))
                    Flip();

                // if dog is away from cat 
                if (Mathf.Abs(this.transform.position.x - target.x) > 0.67f)
                {
                    this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed / 100);
                    animator.SetFloat("x.velocity", speed);
                    animator.SetBool("isGrowling", false);
                }
                // if dog is near cat
                else
                {
                    animator.SetFloat("x.velocity", 0);
                    animator.SetBool("isGrowling", true);
                }
            }
            // is waiting after attack
            else
            {
                animator.SetFloat("x.velocity", 0);
            }
        }
    }
    
    void OnCollisionStay2D(Collision2D other)
    {
        if (other.transform.tag == "Player")
        {
            animator.SetTrigger("attack");
            other.gameObject.GetComponent<PlayerController>().Defend(this.gameObject, damageOnCollision, bumpOnCollision, 0.5f);
            lastAttackTime = Time.time;
        }
    }

    public void Defend(GameObject attacker, int damage, Vector2 bumpVelocity, float bumpTime)
    {
        this.life -= damage;
        animator.SetInteger("life", life);
        if (life <= 0)
            Die();
        else
            animator.SetTrigger("hit");
    }

    public void Die()
    {
        animator.SetInteger("life", 0);
        animator.SetFloat("x.velocity", 0);
        stunAnimation.SetActive(true);
        this.gameObject.layer = LayerMask.NameToLayer("Transparent");
        audioSource.Stop();
        audioSource.PlayOneShot(dyingSound);
        OnDeath.Invoke();
    }

    public void ChaseCat()
    {
        this.state = 1;
    }

    public void StopChaseCat()
    {
        this.state = 0;
        animator.SetBool("isGrowling", false);
    }

    public void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = new Vector3(this.transform.localScale.x * -1, this.transform.localScale.y, this.transform.localScale.z);
        this.transform.localScale = theScale;
    }

    void PlayBarkSound()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(barkingSound);
    }

    void PlayGrowlingSound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.loop = true;
            audioSource.clip = growlingSound;
            audioSource.Play();
        }
    }

    void StopSound()
    {
        audioSource.Stop();
    }
}
