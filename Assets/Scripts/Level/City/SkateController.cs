using System.Collections;
using UnityEngine;

public class SkateController : MonoBehaviour, IDefendable,IKittyzCollecter {

    public Transform scratchPrefab, landingSmokePrefab;
    public float jumpVelocity = 4.6f;
    public LayerMask groundingMask;
    public bool isGrounded = false, allowDoubleJump = true;
    public int life = 3;
    [Range(2f, 10f)]
    public float minSpeed = 2f;
    [Range(4f, 30f)]
    public float maxSpeed = 6f;
    public AudioClip jumpSound, kickflipSound, catJump, catHit, catDie, ratSqueek, ratHit;
    float speed = 2f;
    const float speedGainBySecond = 0.2f;
    Rigidbody2D rb;
    Animator anim;
    Transform checkGroundTop, checkGroundBottom, attackLocation, landingSmokeLocation;
    bool doubleJumped = false, isCrouching = false;
    AudioSource audioS, audioCat, audioRat;
   
    public float Speed {
        get { return speed; }
        set {
            speed = Mathf.Clamp(value, minSpeed, maxSpeed);
            anim.SetFloat("speed", speed);
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        checkGroundTop = GameObject.Find(this.name + "/ground_check_top").transform;
        checkGroundBottom = GameObject.Find(this.name + "/ground_check_bottom").transform;
        attackLocation = GameObject.Find(this.name + "/AttackLocation").transform;
        landingSmokeLocation = GameObject.Find(this.name + "/LandingSmokeLocation").transform;
        life = ApplicationController.ac.playerData.max_life;
        InvokeRepeating("SpeedGain", 1.0f, 0.25f);
        anim = GetComponent<Animator>();
        audioS = GetComponent<AudioSource>();
        audioCat = transform.Find("Cat").GetComponent<AudioSource>();
        audioRat = transform.Find("Rat").GetComponent<AudioSource>();
    }

    void Update()
    {
        
        if (life > 0)
        {
            Move(1f);   // constantly moving to right
            #if !UNITY_ANDROID && !UNITY_IPHONE && !UNITY_BLACKBERRY && !UNITY_WINRT || UNITY_EDITOR
            if (Input.GetButtonDown("Jump"))
                Jump();
            else if (Input.GetButtonUp("Jump"))
                StopJump();
            if (Input.GetButtonDown("Attack"))
                Attack();
            if(!isCrouching && Input.GetButtonDown("Crouch"))           
                Crouch();
            else if (isCrouching && Input.GetButtonUp("Crouch"))
                StopCrouch();
            #endif
            anim.SetFloat("y.velocity", rb.velocity.y);
        }

    }

    void FixedUpdate()
    {
        CheckGround();
        //animator.SetFloat("y.velocity", rb.velocity.y);
    }

    void Move(float horizonalInput)
    {
        Vector2 moveVel = rb.velocity;
        moveVel.x = horizonalInput * speed;
        rb.velocity = moveVel;

        // Update animator
        //animator.SetFloat("speed", Mathf.Abs(horizonalInput));
    }

    void CheckGround()
    {
        bool newIsGrounded = Physics2D.OverlapArea(checkGroundTop.position, checkGroundBottom.position, groundingMask);

        if (newIsGrounded != this.isGrounded) {
            // Pop some smoke when landing for juicy effect
            if (newIsGrounded) {
                doubleJumped = false;
                Transform smoke = (Transform)Instantiate(landingSmokePrefab, landingSmokeLocation.position, Quaternion.identity);
                audioS.PlayOneShot(jumpSound);
                Destroy(smoke.gameObject, 1f);
            }

            isGrounded = newIsGrounded;
            anim.SetBool("isGrounded", isGrounded);
        }

        
    }

    public void Jump()
    {
        if (isGrounded){
            anim.SetTrigger("jump");
            rb.velocity = jumpVelocity * Vector2.up;
            if (Random.value > 0.9f)   //play sound (10% chance)
                audioCat.PlayOneShot(catJump);
            audioS.PlayOneShot(jumpSound);
        }else if (allowDoubleJump && !doubleJumped){
            rb.velocity = (jumpVelocity/2) * Vector2.up;
            doubleJumped = true;
            anim.SetTrigger("doubleJump");
            audioS.PlayOneShot(kickflipSound);
        }
    }

    public void StopJump()
    {
        if (!isGrounded && rb.velocity.y > 0)
            rb.velocity /= 2;
    }

    public void Crouch() {
        isCrouching = true;
        anim.SetBool("crouch", true);
    }

    public void StopCrouch() {
        isCrouching = false;
        anim.SetBool("crouch", false);
    }

    public void Attack()
    {
        string[] attacksAnim = { "attack01", "attack02" };
        string randomAttackAnim = attacksAnim[Random.Range(0, attacksAnim.Length)];
        //animator.SetTrigger(randomAttackAnim);
        Transform attack = (Transform)Instantiate(scratchPrefab, attackLocation.position, Quaternion.identity);
        attack.gameObject.GetComponent<ScratchController>().Init(1, Vector2.zero, 0.35f);
    }

    void SpeedGain(){
        if (life > 0) {
            float speedGain = (isCrouching) ? -speedGainBySecond / 4 : speedGainBySecond / 4;
            Speed = speed + speedGain;
        }
    }

    public void Defend(GameObject attacker, int damage, Vector2 bumpVelocity, float bumpTime)
    {
        if (life <= 0)
            return;
        GetInjured(damage);
        Speed = minSpeed;
        anim.SetTrigger("hit");
    }

    void GetInjured(int dmg)
    {
        dmg = Mathf.Clamp(dmg, 0, life);
        life -= dmg;
        CityGameController.gc.PlayerInjured(dmg);
        if (life <= 0)
            Die();
        else {
            if (Random.value < 0.5f)   //play cat or rat sound
                audioCat.PlayOneShot(catHit);
            else
                audioRat.PlayOneShot(ratHit);
        }
    }

    void Die()
    {
        audioCat.PlayOneShot(catDie);
        audioRat.PlayOneShot(ratSqueek);
        //animator.SetBool("isDead", true);
        //animator.SetTrigger("die"); // trigger + bool to prevent animator to play death multiple times
        CityGameController.gc.GameOver();
        Move(0f);
    }

    public void CollectKittyz(int amount = 1)
    {
        CityGameController.gc.CollectKittyz(amount);
    }

}
