using UnityEngine;

public class CityIntroOrchester : MonoBehaviour {

    public GameObject cat, rat, skateboard, skateCat, skateRat, hangGlider;
    public Transform catGroundPos, catSkatePos, ratPolePos, ratGroundPos, ratSkatePos, skateRightPos;
    public float catSpeed = 2f, ratSpeed = 2f;
    public LevelEnum levelToLoad;

    Animator catAnim, ratAnim, skateboardAnim;
    int catState = 0, ratState=0;

    private void Start() {
        catAnim = cat.GetComponent<Animator>();
        ratAnim = rat.GetComponent<Animator>();
        skateboardAnim = skateboard.GetComponent<Animator>();

        // Init cat
        catAnim.SetTrigger("jump");
        catAnim.SetFloat("y.velocity", 0.5f);
        catAnim.SetBool("isHangGliding", true);
        catAnim.SetBool("isGrounded", false);

        // Init rat
        ratAnim.SetBool("isTailGliding", true);

        // Init skate
        skateboardAnim.SetFloat("speed", 2f);
        skateCat.SetActive(false);
        skateRat.SetActive(false);
    }

    private void Update() {
        MoveCat();
        MoveRat();
        MoveSkate();
    }

    void MoveCat() {
        // Ground cat
        if (catState == 0) {
            if (cat.transform.position != catGroundPos.position)
                cat.transform.position = Vector3.MoveTowards(cat.transform.position, catGroundPos.position, Time.deltaTime * catSpeed);
            else {
                hangGlider.SetActive(false);
                catAnim.SetFloat("y.velocity", 0f);
                catAnim.SetBool("isHangGliding", false);
                catAnim.SetBool("isGrounded", true);
                catAnim.SetFloat("speed", 1f);
                catState++;
            }
        }
        //Cat walk to skate
        else if (catState == 1) {
            if (cat.transform.position != catSkatePos.position)
                cat.transform.position = Vector3.MoveTowards(cat.transform.position, catSkatePos.position, Time.deltaTime * catSpeed);
            else {                
                catAnim.SetFloat("speed", 0f);
                catState++;
            }
        }
        
    }

    void MoveRat() {
        // Rat glide to pole
        if (ratState == 0) {
            if (rat.transform.position != ratPolePos.position)
                rat.transform.position = Vector3.MoveTowards(rat.transform.position, ratPolePos.position, Time.deltaTime * ratSpeed);
            else {
                ratAnim.SetBool("isTailGliding", false);
                ratAnim.SetBool("isFloating", true);
                ratState++;
            }
        }
        // Rat falls to ground
        else if (ratState == 1) {
            if (rat.transform.position != ratGroundPos.position)
                rat.transform.position = Vector3.MoveTowards(rat.transform.position, ratGroundPos.position, Time.deltaTime * ratSpeed);
            else {
                ratAnim.SetBool("isFloating", false);
                ratAnim.SetFloat("speed", 1f);
                ratState++;
            }
        }
        // Rat runs to skate
        else if (ratState == 2) {
            if (rat.transform.position != ratSkatePos.position)
                rat.transform.position = Vector3.MoveTowards(rat.transform.position, ratSkatePos.position, Time.deltaTime * ratSpeed);
            else {
                ratAnim.SetFloat("speed", 0f);
                ratState++;
            }
        }
    }

    void MoveSkate() {
        // Replace rat/cat by skate rat/cat
        if (catState==2 && ratState == 3) {
            cat.SetActive(false);
            rat.SetActive(false);
            skateCat.SetActive(true);
            skateRat.SetActive(true);
            catState++;
        }
        // skate move to right
        else if (catState == 3) {
            if (skateboard.transform.position != skateRightPos.position)
                skateboard.transform.position = Vector3.MoveTowards(skateboard.transform.position, skateRightPos.position, Time.deltaTime * catSpeed);
            else
                LoadLevel();
        }
    }

    void LoadLevel() {
        SceneLoader.LoadSceneWithLoadingScreen(levelToLoad.ToString());
    }
}
