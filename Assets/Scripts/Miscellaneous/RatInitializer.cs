using UnityEngine;

public class RatInitializer : MonoBehaviour {

    public bool isEating = false;
    public GameObject[] detectCheeses;
    Animator anim;
    Transform cheeseAreaTop, cheeseAreaBottom;
    SpeechBubbleController bubble;
	
	void Start () {
        anim = GetComponent<Animator>();
        anim.SetBool("isEating", isEating);
        if(detectCheeses!=null && detectCheeses.Length > 0) {
            cheeseAreaTop = transform.Find("CheeseAreaTop");
            cheeseAreaBottom = transform.Find("CheeseAreaBottom");
        }
        bubble = GetComponentInChildren<SpeechBubbleController>(true);
    }

    void Update()
    {
        // In the 2-04 level, detect if cheeses are thrown for the achievement
        if (detectCheeses != null && detectCheeses.Length > 0){
            bool cheesesThrown = true;
            foreach (GameObject cheese in detectCheeses) {
                if (cheese.transform.position.x > cheeseAreaTop.position.x && cheese.transform.position.x < cheeseAreaBottom.position.x &&
                    cheese.transform.position.y < cheeseAreaTop.position.y && cheese.transform.position.y > cheeseAreaBottom.position.y) {
                    cheesesThrown = false;
                    break;
                }
            }
            if (cheesesThrown) {
                bubble.SetText("RAT_NOT_COOL", 5f);
                detectCheeses = null;
                // Achievement
                PlayGamesScript.UnlockAchievement(GPGSIds.achievement_why_are_you_so_angry);
            }
        }
    }


}
