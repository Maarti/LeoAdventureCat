using System.Collections;
using UnityEngine;

public class SkateController : MonoBehaviour, IDefendable,IKittyzCollecter {

    public Transform scratchPrefab, landingSmokePrefab;
    public float speed = 2f, jumpVelocity = 4.6f;
    public LayerMask groundingMask;
    public bool isGrounded = false, allowDoubleJump = true;
    public int life = 3;
    public float minSpeed = 2f, maxSpeed = 6f;

    const float speedGainBySecond = 0.2f;
    Rigidbody2D rb;
    Animator anim;
    Transform checkGroundTop, checkGroundBottom, attackLocation, landingSmokeLocation;
    MouthController mouth;
    bool doubleJumped = false;

   
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
            if (Input.GetButtonDown("Fire1"))
                Attack();
            if (Input.GetButtonDown("Fire2"))
                Crouch();
            else if (Input.GetButtonUp("Fire2"))
                StopCrouch();
            #endif
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

        // Pop some smoke when landing for juicy effect
        if (this.isGrounded == false && newIsGrounded == true)
        {
            doubleJumped = false;
            Transform smoke = (Transform)Instantiate(landingSmokePrefab, landingSmokeLocation.position, Quaternion.identity);
            Destroy(smoke.gameObject, 1f);
        }

        isGrounded = newIsGrounded;
        //animator.SetBool("isGrounded", isGrounded);
    }

    public void Jump()
    {
        if (isGrounded){
            //animator.SetTrigger("jump");
            rb.velocity = jumpVelocity * Vector2.up;
            /*  if (Random.value > 0.87f)   //play sound (13% chance)
                  mouth.Meowing("jump");*/
        }else if (allowDoubleJump && !doubleJumped){
            rb.velocity = (jumpVelocity/2) * Vector2.up;
            doubleJumped = true;
        }
    }

    public void StopJump()
    {
        if (!isGrounded && rb.velocity.y > 0)
            rb.velocity /= 2;
    }

    public void Crouch() {
        anim.SetBool("crouch", true);
    }

    public void StopCrouch() {
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
        if (life > 0)
            speed = Mathf.Clamp(speed + (speedGainBySecond / 4), minSpeed, maxSpeed);
    }

    public void Defend(GameObject attacker, int damage, Vector2 bumpVelocity, float bumpTime)
    {
        if (life <= 0)
            return;
        GetInjured(damage);
        speed = minSpeed;
        //animator.SetTrigger("hit");
    }

    void GetInjured(int dmg)
    {
        dmg = Mathf.Clamp(dmg, 0, life);
        life -= dmg;
        CityGameController.gc.PlayerInjured(dmg);
        if (life <= 0)
            Die();
        /*else
            mouth.Meowing("hit");*/
    }

    void Die()
    {
        //mouth.Meowing("die");
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
