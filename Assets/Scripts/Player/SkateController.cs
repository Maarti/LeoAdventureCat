using System.Collections;
using UnityEngine;

public class SkateController : MonoBehaviour {

    public Transform scratchPrefab, landingSmokePrefab;
    public float speed = 2f, jumpVelocity = 4.6f;
    public LayerMask groundingMask;
    public bool isGrounded = false, canMoveInAir = true;
    public int life = 3;

    Rigidbody2D rb;
    Animator animator;
    Transform checkGroundTop, checkGroundBottom, attackLocation, landingSmokeLocation;
    MouthController mouth;

    void Awake()
    {
        //animator = GetComponent<Animator>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        checkGroundTop = GameObject.Find(this.name + "/ground_check_top").transform;
        checkGroundBottom = GameObject.Find(this.name + "/ground_check_bottom").transform;
        attackLocation = GameObject.Find(this.name + "/AttackLocation").transform;
        landingSmokeLocation = GameObject.Find(this.name + "/LandingSmokeLocation").transform;
        life = ApplicationController.ac.playerData.max_life;
    }

    void Update()
    {
        #if !UNITY_ANDROID && !UNITY_IPHONE && !UNITY_BLACKBERRY && !UNITY_WINRT || UNITY_EDITOR
        if (life > 0)
        {
            Move(1f);   // constantly moving to right
            if (Input.GetButtonDown("Jump"))
                Jump();
            else if (Input.GetButtonUp("Jump"))
                StopJump();
            if (Input.GetButtonDown("Fire1"))
                Attack();
        }
        #endif
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
        Debug.Log("speed=" + moveVel);
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
            Transform smoke = (Transform)Instantiate(landingSmokePrefab, landingSmokeLocation.position, Quaternion.identity);
            Destroy(smoke.gameObject, 1f);
        }

        isGrounded = newIsGrounded;
        //animator.SetBool("isGrounded", isGrounded);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            //animator.SetTrigger("jump");
            rb.velocity = jumpVelocity * Vector2.up;
          /*  if (Random.value > 0.87f)   //play sound (13% chance)
                mouth.Meowing("jump");*/
        }

    }

    public void StopJump()
    {
        if (!isGrounded && rb.velocity.y > 0)
            rb.velocity /= 2;
    }

    public void Attack()
    {
        string[] attacksAnim = { "attack01", "attack02" };
        string randomAttackAnim = attacksAnim[Random.Range(0, attacksAnim.Length)];
        //animator.SetTrigger(randomAttackAnim);
        Transform attack = (Transform)Instantiate(scratchPrefab, attackLocation.position, Quaternion.identity);
        attack.gameObject.GetComponent<ScratchController>().Init(1, Vector2.zero, 0.35f);
    }
}
