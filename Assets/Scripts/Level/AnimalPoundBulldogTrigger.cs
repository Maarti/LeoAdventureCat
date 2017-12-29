using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalPoundBulldogTrigger : MonoBehaviour
{
	public GameObject dog, ball, boundary;
	public Transform ratDown, ratUp, ratDownPipe, ratUpPipe, dogDown, dogUp, platformPrefab;
    public Transform minPlatformZone, maxPlatformZone;
    public float dogSpeed = 1f, dogWaitingTime = 3f;
    const int nbPlatforms = 200;
    Transform startPlatform;
    GameObject rat, cat;
	bool ratFacingRight = false, dogFacingRight = true;
	bool waitingTimeIsSet = false;
	int state = -1, nbLoop=0;
    const int nbLoopTotal = 1; // nb times to look for rat before chase cat
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
        startPlatform = ratUp;
	}

	void FixedUpdate ()
	{
        if (state <= 0)
            return;
        
        // init anim
        else if (state == 1)
        {
            DogFaceRight();
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
                dog.transform.position = Vector3.MoveTowards(dog.transform.position, dogUp.position, Time.deltaTime * dogSpeed/2);
            }
            else {
                rat.transform.position = ratDownPipe.position;
                dogAnim.SetFloat("x.velocity", 0f);
                dogAnim.SetBool("goUp", true);
                RatFlip();
                nbLoop++;
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
                dog.transform.position = Vector3.MoveTowards(dog.transform.position, dogDown.position, Time.deltaTime * dogSpeed/2);
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
                    dogAnim.SetBool("goDown", false);
                    dogAnim.SetBool("isGrowling", false);
                    waitingTimeIsSet = false;
                    state = (nbLoop >= nbLoopTotal) ? state+1 : 1;
                }
            }
        }
        
        // dog chases cat
        else if (state == 8)
        {
            nbLoop = 0;
            //bossCtrlr.ChaseCat();
            //dogAnim.SetFloat("x.velocity", bossCtrlr.speed);
            dogAnim.SetBool("isGrowling", true);
            StartCoroutine(ThrowingPlatforms());
            state++;
        }

        // wait 10s
        /*else if (state == 9)
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
                    state++;
                    waitingTimeIsSet = false;
                }
            }
        }*/

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

    IEnumerator ThrowingPlatforms()
    {
        int platformCount = 0;
        while (platformCount < nbPlatforms)
        {
            //Vector3 target = new Vector3(23f+Random.Range(-1f,1f) , 21.7f+Random.Range(0f,0.5f));
            Vector3 target = GetPlatformPosition();
            Quaternion rota = new Quaternion();
            Transform platform = (Transform)Instantiate(platformPrefab, ratUp.position, rota);
            StartCoroutine(CurveThrow(platform.gameObject,target));
            platformCount++;
           // if (platformCount % 2 != 0)
                yield return new WaitForSeconds(.9f);
          //  else
           //     yield return null;
           /* if (platformCount == 1)
                bossCtrlr.ChaseCat();*/
        }
        bossCtrlr.StopChaseCat();
        dogAnim.SetBool("isGrowling", false);
        state = 1;
        yield return null;
    }

    IEnumerator CurveThrow(GameObject platform, Vector3 target)
    {
        Rigidbody2D rb = platform.GetComponent<Rigidbody2D>();
        float journeyTime = .6f, startTime = Time.time;
        while (platform.transform.position != target)
        {
            Vector3 center = (startPlatform.position+ target) * 0.5F;
            center -= new Vector3(0, 1, 0);
            Vector3 startRelCenter = startPlatform.position - center;
            Vector3 targetRelCenter = target - center;
            float fracComplete = (Time.time - startTime) / journeyTime;
            platform.transform.position = Vector3.Slerp(startRelCenter, targetRelCenter, fracComplete);
            platform.transform.position += center;



            /* platform.transform.position = Vector3.Slerp(ratUp.position, target, time);
            time = Mathf.Clamp(time + Time.deltaTime, 0f, 1f);
             Debug.Log("target=" + target + "  platform=" + platform.transform.position+"    time="+time);*/
            yield return null;
        }

        platform.GetComponent<FallingPlatform>().Trigger();
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

    Vector3 GetPlatformPosition()
    {
        // random position in a circle of 1 (because cat jump height is 1)
        Vector3 position = Random.insideUnitCircle;
        // increase x position
        position.x *= 1.5f;
        Debug.Log("position=" + position);
        // if cat is jumping, lower the y
        if (!cat.GetComponent<PlayerController>().isGrounded)
            position.y -= 1f;
        // allow lower position if y is negative
        if (position.y < 0)
            position.y *= 1.5f;
        // position around the cat
        position +=  cat.transform.position;
        // clamp position inside the limit zone
        position.x = Mathf.Clamp(position.x, minPlatformZone.position.x, maxPlatformZone.position.x);
        position.y = Mathf.Clamp(position.y, minPlatformZone.position.y, maxPlatformZone.position.y);
        return position;
    }
}
