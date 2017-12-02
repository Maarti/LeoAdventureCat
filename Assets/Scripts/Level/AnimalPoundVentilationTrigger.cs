using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalPoundVentilationTrigger : MonoBehaviour
{
    public Transform ventilEntry, ceiling;
    public float ratSpeed;
    GameObject cat, rat;
    bool ratFacingRight = false, isTrigered = false;
    int state = 0;
    Animator ratAnim;
    GameUIController guic;

    // Use this for initialization
    void Start()
    {
        rat = GameObject.FindGameObjectWithTag("Rat");
        ratAnim = rat.GetComponent<Animator>();
        guic = GameObject.Find("Canvas/GameUI").GetComponent<GameUIController>();
        cat = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        if (state == 0)
            return;

        // Rat speak
        else if (state == 1)
        {
            guic.DisplayDialog(DialogEnum.rat_go_to_ventilation);
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

        // Rat dissociate from cat and face to left
        else if (state == 3)
        {
            ratAnim.SetBool("isEating", false);
            Vector3 ratScale = rat.transform.localScale;
            rat.transform.parent = null;
            rat.transform.localScale = ratScale;
            rat.GetComponent<Collider2D>().enabled = true;
            if (rat.transform.localScale.x < 0)
                RatFlip();
            state++;
        }

        // Rat jump to ceiling
        else if (state == 4)
        {
            if (rat.transform.position != ceiling.position)
            {
                rat.transform.position = Vector3.MoveTowards(rat.transform.position, ceiling.position, Time.deltaTime * ratSpeed);
                ratAnim.SetFloat("speed", ratSpeed);
            }
            else
            {
                RatHorizontalFlip();
                state++;
            }
        }

        // Rat run to ventil
        else if (state == 5)
        {
            if (rat.transform.position.x != ventilEntry.position.x)
            {
                rat.transform.position = Vector3.MoveTowards(rat.transform.position, ventilEntry.position, Time.deltaTime * ratSpeed);
                ratAnim.SetFloat("speed", ratSpeed);
            }
            else
            {
                ratAnim.SetFloat("speed", 0f);
                guic.DisplayDialog(DialogEnum.rat_ready);
                state++;
            }
        }

        // Destroy
        else if (state == 6)
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