using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalPoundBulldogTrigger : MonoBehaviour
{
	public GameObject dog, ball, boundary;
	public Transform ratDown, ratUp, ratDownPipe, ratUpPipe, dogDown, dogUp;
    public float dogSpeed = 1f, dogWaitingTime = 3f;
    GameObject rat, cat;
	bool ratFacingRight = false, dogFacingRight = true;
	bool waitingTimeIsSet = false;
	int state = -1;
	float startWaitingTime;
	Animator ratAnim, dogAnim;
	GameUIController guic;
    const float ratSpeed = 1f;
    BulldogBossController bossCtrlr;

	// Use this for initialization
	void Start ()
	{
        rat = GameObject.FindGameObjectWithTag("Rat");
        cat = GameObject.FindGameObjectWithTag("Player");
        ratAnim = rat.GetComponent<Animator> ();
        dogAnim = dog.GetComponent<Animator>();
		guic = GameObject.Find ("Canvas/GameUI").GetComponent<GameUIController> ();
        bossCtrlr = dog.GetComponent<BulldogBossController>();
	}

	void FixedUpdate ()
	{
        if (state <= 0)
            return;
        
        // 
        else if (state == 1)
        {
            dogAnim.SetFloat("x.velocity", dogSpeed);
            ratAnim.SetFloat("speed", ratSpeed);
            RatFlip();
            state++;
        }

        // dog goes up AND rat run into pipe
        else if (state == 2)
        {
            if (dog.transform.position != dogUp.position) {
                rat.transform.position = Vector3.MoveTowards(rat.transform.position, ratUpPipe.position, Time.deltaTime * ratSpeed);
                dog.transform.position = Vector3.MoveTowards(dog.transform.position, dogUp.position, Time.deltaTime * dogSpeed);
            }
            else {
                rat.transform.position = ratDownPipe.position;
                dogAnim.SetFloat("x.velocity", 0f);
                dogAnim.SetBool("goUp", true);
                RatFlip();
                state++;
            }
        }

        // rat goes down
        else if (state == 3)
        {
            if (rat.transform.position != ratDown.position)
                rat.transform.position = Vector3.MoveTowards(rat.transform.position, ratDown.position, Time.deltaTime * ratSpeed);
            else
            {
                ratAnim.SetFloat("speed", 0f);
                dogAnim.SetBool("isGrowling", true);
                state++;
            }
        }

        // wait 3s
        else if (state == 4)
        {
            if (!waitingTimeIsSet)
            {
                startWaitingTime = Time.time;
                waitingTimeIsSet = true;
            }
            else
            {
                if ((Time.time - startWaitingTime) < dogWaitingTime)
                    return;
                else
                {
                    RatFlip();
                    ratAnim.SetFloat("speed", ratSpeed);
                    dogAnim.SetBool("goUp",false);
                    dogAnim.SetBool("isGrowling", false);
                    state++;
                    waitingTimeIsSet = false;
                }
            }
        }

        // dog goes down / rat run into pipe
        else if (state == 5)
        {
            if (dog.transform.position != dogDown.position && rat.transform.position != ratDownPipe.position)
            {
                rat.transform.position = Vector3.MoveTowards(rat.transform.position, ratDownPipe.position, Time.deltaTime * ratSpeed);
                dog.transform.position = Vector3.MoveTowards(dog.transform.position, dogDown.position, Time.deltaTime * dogSpeed);
            }
            else
            {
                rat.transform.position = ratUpPipe.position;
                dogAnim.SetBool("goDown", true);
                RatFlip();
                state++;
            }
        }

        // rat goes up
        else if (state == 6)
        {
            if (rat.transform.position != ratUp.position)
                rat.transform.position = Vector3.MoveTowards(rat.transform.position, ratUp.position, Time.deltaTime * ratSpeed);
            else
            {
                ratAnim.SetFloat("speed", 0f);
                dogAnim.SetBool("isGrowling", true);
                state++;
            }
        }

        // wait 3s
        else if (state == 7)
        {
            if (!waitingTimeIsSet)
            {
                startWaitingTime = Time.time;
                waitingTimeIsSet = true;
            }
            else
            {
                if ((Time.time - startWaitingTime) < dogWaitingTime)
                    return;
                else
                {
                    ratAnim.SetFloat("speed", ratSpeed);
                    dogAnim.SetBool("goDown", false);
                    dogAnim.SetBool("isGrowling", false);
                    state++;
                    waitingTimeIsSet = false;
                }
            }
        }
        
        // dog chases cat
        else if (state == 8)
        {
            bossCtrlr.ChaseCat();
            dogAnim.SetFloat("x.velocity", bossCtrlr.speed);
            dogAnim.SetBool("isGrowling", true);
            state++;
        }

        // wait 10s
        else if (state == 9)
        {
            if (!waitingTimeIsSet)
            {
                startWaitingTime = Time.time;
                waitingTimeIsSet = true;
            }
            else
            {
                if ((Time.time - startWaitingTime) < 10f)
                    return;
                else
                {
                    bossCtrlr.StopChaseCat();
                    dogAnim.SetBool("isGrowling", false);
                    state = 1;
                    waitingTimeIsSet = false;
                }
            }
        }

    }


	void OnTriggerEnter2D (Collider2D other)
	{
		if (state==-1 && other.gameObject.tag == "Player") {
            state++;
			StartCoroutine (StartAnimation ());
		}
	}

    IEnumerator StartAnimation()
    {
        // disable UI        
        guic.DisplayMobileController(false);
        guic.DisplayTopUI(false);
        cat.GetComponent<PlayerController>().StartMoving(0f);

        // throw ball
        ball.GetComponent<Rigidbody2D>().isKinematic = false;
        ball.GetComponent<Rigidbody2D>().velocity = new Vector2(-2, 5); ;
        yield return new WaitForSeconds(1.5f);

        // activate boundary
        boundary.SetActive(true);

        // rat dialog
        guic.DisplayDialog(DialogEnum.rat_start_bulldog);
        yield return null;

        // display ui
        guic.DisplayMobileController();
        guic.DisplayTopUI();

        

        state++;
        yield return null;
    }

    void DogFaceRight()
    {
        if(!bossCtrlr.isFacingRight)
            bossCtrlr.Flip();
    }

    void DogFaceLeft()
    {
        if (bossCtrlr.isFacingRight)
            bossCtrlr.Flip();
    }

    void RatFlip ()
	{
		ratFacingRight = !ratFacingRight;
		Vector3 theScale = new Vector3 (rat.transform.localScale.x * -1, rat.transform.localScale.y, rat.transform.localScale.z);
		rat.transform.localScale = theScale;		
	}
}
