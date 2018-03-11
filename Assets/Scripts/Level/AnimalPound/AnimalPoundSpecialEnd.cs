using System.Collections;
using UnityEngine;

public class AnimalPoundSpecialEnd : EndSign
{
    public GameObject dog, hangGlider;
    public Transform endSignPlace, ratDownPos, poleDownPos, poleUpPos, ratFinishPos, hangGliderPos;
    public float signSpeed = 3f, ratSpeed = 2f;

    bool isTriggered = false, isBossDefeated=false;
    GameObject cat, rat, cam;
    BulldogBossController dogCtrlr;
    Animator ratAnim, catAnim;

    protected override void Start ()
	{
		base.Start ();
        cat = GameObject.FindWithTag("Player");
        rat = GameObject.FindWithTag("Rat");
        ratAnim = rat.GetComponent<Animator>();
        dogCtrlr = dog.GetComponent<BulldogBossController>();
        dogCtrlr.OnDeath += OnBossDeath; // listener on boss death
        cam = GameObject.FindGameObjectWithTag("MainCamera");
    }

    void Update()
    {
        if (isBossDefeated && this.transform.position != endSignPlace.position)
            this.transform.position = Vector3.MoveTowards(this.transform.position, endSignPlace.position, Time.deltaTime*signSpeed);
    }

    protected override void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player" && !isTriggered) {
			isTriggered = true;
			GameController.gc.gameFinished = true;
			guic.DisplayMobileController (false);
			guic.DisplayTopUI (false);
            cat.GetComponent<PlayerController>().freeze = true;
            cat.GetComponent<PlayerController> ().StartMoving (0f);
			StartCoroutine (RatEndingAnimation ());
		}
	}

	IEnumerator RatEndingAnimation ()
	{
        // rat goes to pole
        hangGlider.SetActive(true);
        ratAnim.SetFloat("speed", ratSpeed);
        while(rat.transform.position != ratDownPos.position)
        {
            rat.transform.position = Vector3.MoveTowards(rat.transform.position, ratDownPos.position, Time.deltaTime * ratSpeed);
            yield return null;
        }
        RatFlip();
        while (rat.transform.position != poleDownPos.position)
        {
            rat.transform.position = Vector3.MoveTowards(rat.transform.position, poleDownPos.position, Time.deltaTime * ratSpeed);
            yield return null;
        }
        RatFlip();

        // rat climb to pole
        rat.transform.Rotate(Vector3.forward * -90);
        while (rat.transform.position != poleUpPos.position)
        {
            rat.transform.position = Vector3.MoveTowards(rat.transform.position, poleUpPos.position, Time.deltaTime * ratSpeed);
            yield return null;
        }
        //ratAnim.SetFloat("speed", 0f);
        rat.transform.Rotate(Vector3.forward * 90);
        yield return null;

        // rat drops hang glider to cat
        StartCoroutine(CatEndingAnimation());

        // rat slide to the end
        RatFlip();
        ratAnim.SetBool("isTailGliding", true);
        yield return null;
        while (rat.transform.position != ratFinishPos.position)
        {
            rat.transform.position = Vector3.MoveTowards(rat.transform.position, ratFinishPos.position, Time.deltaTime * ratSpeed/1.2f);
            yield return null;
        }
	}

    IEnumerator CatEndingAnimation()
    {
        // freeze
        cat.GetComponent<Rigidbody2D>().isKinematic = true;
        cat.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        cam.GetComponent<CameraFloorController>().enabled = false;
        yield return null;

        // hangglider fall / cat jumps
        hangGlider.transform.parent = null;
        cat.GetComponent<Animator>().SetBool("isHangGliding", true);
        float time = 0f;
        while (hangGlider.transform.position != hangGliderPos.position ||
            cat.transform.position != hangGliderPos.position)
        {
            cat.transform.position = Vector3.MoveTowards(cat.transform.position, hangGliderPos.position, Time.deltaTime*1.5f);
            hangGlider.transform.position = Vector3.MoveTowards(hangGlider.transform.position, hangGliderPos.position, Time.deltaTime);
            time += Time.deltaTime;
            // timeout
            if (time>0.5f){
                cat.transform.position = hangGliderPos.position;
                hangGlider.transform.position = hangGliderPos.position;
                break;
            }
            yield return null;
        }

        // cat equips hangglider
        hangGlider.transform.localScale = new Vector3(1f, 1f);
        hangGlider.transform.rotation = Quaternion.identity;
        yield return null;
        hangGlider.transform.parent = cat.transform;        
        hangGlider.transform.localPosition = cat.GetComponent<IGlider>().GetHangGliderPosition();
        yield return null;

        // cat fly to the end
        time = 0f;
        while (cat.transform.position != ratFinishPos.position)
        {
            cat.transform.position = Vector3.MoveTowards(cat.transform.position, ratFinishPos.position, Time.deltaTime*1.2f);
            time += Time.deltaTime;
            // timeout
            if (time > 5f) {
                cat.transform.position = ratFinishPos.position;
                break;
            }
            yield return null;
        }

        // Score panel
        guic.EndGame();
        yield return null;
    }

    // Triggered when boss is defeated
    void OnBossDeath()
    {
        isBossDefeated = true;
    }

    void RatFlip()
    {
        Vector3 theScale = new Vector3(rat.transform.localScale.x * -1, rat.transform.localScale.y, rat.transform.localScale.z);
        rat.transform.localScale = theScale;
    }

}
