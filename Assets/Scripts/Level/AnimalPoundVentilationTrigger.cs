using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalPoundVentilationTrigger : MonoBehaviour
{
    public Transform ventilEntry, ceiling, insideVentil;
    public float ratSpeed;
    public GameObject ventilCover;
    GameObject cat, rat;
    bool ratFacingRight = false, isTrigered = false, waitingTimeIsSet = false;
    float startWaitingTime;
    int state = 0;
    Animator ratAnim;
    GameUIController guic;
    VentilationGameController ventilGC;

    // Use this for initialization
    void Start()
    {
        rat = GameObject.FindGameObjectWithTag("Rat");
        ratAnim = rat.GetComponent<Animator>();
        guic = GameObject.Find("Canvas/GameUI").GetComponent<GameUIController>();
        cat = GameObject.FindGameObjectWithTag("Player");
        ventilGC = GameObject.Find("VentilationGame").GetComponent<VentilationGameController>();
    }

    void FixedUpdate()
    {
        if (state == 0)
            return;

        // MoveCamera
        else if (state == 1)
        {
            //guic.DisplayDialog(DialogEnum.rat_go_to_ventilation);
            ventilGC.InitCamera();
            cat.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            state++;
        }

        // Disable UI
        else if (state == 2)
        {
            guic.DisplayMobileController(false);
            guic.DisplayTopUI(false);
            state++;
        }

        // Rat dissociates from cat and face to left
        else if (state == 3)
        {
            ratAnim.SetBool("isEating", false);
            Vector3 ratScale = rat.transform.localScale;
            rat.transform.parent = null;
            rat.transform.localScale = ratScale;            
            if (rat.transform.localScale.x > 0)
                RatFlip();
            ratAnim.SetFloat("speed", ratSpeed * 4);
            ratAnim.SetFloat("y.velocity", 1f);
            ratAnim.SetTrigger("jump");
            state++;
        }

        // Rat jumps to ceiling
        else if (state == 4)
        {
            if (rat.transform.position != ceiling.position)
            {
                rat.transform.position = Vector3.MoveTowards(rat.transform.position, ceiling.position, Time.deltaTime * ratSpeed*4);
            }
            else
            {
                RatHorizontalFlip();
                ratAnim.SetFloat("y.velocity", 0f);
                ratAnim.SetFloat("speed", ratSpeed);
                state++;
            }
        }

        // Rat runs to ventil
        else if (state == 5)
        {
            if (rat.transform.position.x != ventilEntry.position.x)
            {
                rat.transform.position = Vector3.MoveTowards(rat.transform.position, ventilEntry.position, Time.deltaTime * ratSpeed);
            }
            else
            {                
                ratAnim.SetFloat("speed", 0f);
                RatFlip();
                state++;
            }
        }

        // Waiting 1s and open ventil
        else if (state == 6)
        {
            if (!waitingTimeIsSet)
            {
                startWaitingTime = Time.time;
                waitingTimeIsSet = true;
            }
            else
            {
                if ((Time.time - startWaitingTime) < 1f)
                    return;
                else
                {
                    ventilCover.GetComponent<Rigidbody2D>().isKinematic = false;
                    waitingTimeIsSet = false;
                    state++;
                }
            }
        }

        // Waiting 1s then float
        else if (state == 7)
        {
            if (!waitingTimeIsSet)
            {
                startWaitingTime = Time.time;
                waitingTimeIsSet = true;
            }
            else
            {
                if ((Time.time - startWaitingTime) < 1f)
                    return;
                else
                {
                    rat.GetComponent<Collider2D>().enabled = true;
                    RatHorizontalFlip();
                    ratAnim.SetBool("isFloating", true);
                    ratAnim.SetFloat("speed", 0f);
                    state++;
                }
            }
        }

        // Rat enters in ventil and floats
        else if (state == 8)
        {
            if (rat.transform.position != insideVentil.position)
            {
                rat.transform.position = Vector3.MoveTowards(rat.transform.position, insideVentil.position, Time.deltaTime * ratSpeed);
            }
            else
            {
                guic.DisplayDialog(DialogEnum.rat_go_to_ventilation);
                ventilGC.StartGame();
                state++;
            }
        }

        // Destroy
        else if (state == 9)
            Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isTrigered && other.gameObject.tag == "Player")
        {
            isTrigered = true;
            state++;
        }
    }

    void RatFlip()
    {
        ratFacingRight = !ratFacingRight;
        Vector3 theScale = new Vector3(rat.transform.localScale.x * -1, rat.transform.localScale.y, rat.transform.localScale.z);
        rat.transform.localScale = theScale;
    }

    void RatHorizontalFlip()
    {
        Vector3 theScale = new Vector3(rat.transform.localScale.x, rat.transform.localScale.y * -1, rat.transform.localScale.z);
        rat.transform.localScale = theScale;
    }
} 