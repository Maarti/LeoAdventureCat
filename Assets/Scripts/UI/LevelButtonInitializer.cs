using UnityEngine;
using UnityEngine.UI;

public class LevelButtonInitializer : MonoBehaviour
{
	public LevelEnum levelEnum = LevelEnum.level_1_01;
    public GameObject[] toDisableWhenGoToShop;
    public GameObject shopPanel;
    
	Level level;
	GameObject uiLockImage, uiScore;
	Text uiDifficulty, uiName;
	Button button;
	bool isStarted = false;

	// Use this for initialization
	void Start ()
	{
		level = ApplicationController.ac.levels [levelEnum];
		uiName = transform.Find ("Name").gameObject.GetComponent<Text> ();
		uiLockImage = transform.Find ("LockImg").gameObject;
		if (!level.isStory)
			uiDifficulty = transform.Find ("Difficulty").gameObject.GetComponent<Text> ();
		uiScore = transform.Find ("Score").gameObject;
		button = GetComponent<Button> ();
        InitButton();
        // When object is Started, OnEnable() can be called
        isStarted = true;
    }

	void InitButton ()
	{
		level = ApplicationController.ac.levels [levelEnum];
        ColorBlock cb = button.colors;
        Color nc = cb.normalColor;
        Color pc = cb.pressedColor;
        Color hc = cb.highlightedColor;
        if (level.isLocked) {
            nc.a = .4f;
            pc = new Color(1f, 0f, 0f, .4f);
            hc.a = .4f;
            uiName.color = new Color(200f / 255, 200f / 255, 200f / 255);
            //button.interactable = false;
            uiLockImage.SetActive (true);
			uiScore.SetActive (false);
		} else {
            //button.interactable = true;
            nc.a = 1f;
            pc = new Color(200f/255, 200f/255, 200f/255,1f);
            hc.a = 1f;
            uiName.color = new Color(1f,1f,1f);
            uiLockImage.SetActive (false);
			uiScore.SetActive (true);
			uiScore.GetComponent<Text> ().text = level.score.ToString () + "%";
			if (level.score >= 100) {
				uiScore.GetComponent<Text> ().color = new Color(158f/255,1f,15f/255);
                uiScore.GetComponent<Text> ().resizeTextForBestFit = true;
			}
		}
        cb.normalColor = nc;
        cb.highlightedColor = hc;
        cb.pressedColor = pc;
        button.colors = cb;
		if (!level.isStory)
			uiDifficulty.text = LocalizationManager.Instance.GetText (level.difficulty.ToString ());
		uiName.text = level.GetFullName ();
	}

	void OnEnable ()
	{
		if (isStarted)
			Start ();
	}

	public void LoadThisLevel ()
	{
        level = ApplicationController.ac.levels[levelEnum];
        if (level.isLocked) // go to shop
        {
            foreach (GameObject g in toDisableWhenGoToShop)
                g.SetActive(false);
            shopPanel.SetActive(true);
        }
        // load intro scene if the level has one
        else if (level.introScene != null) {
            // Intro scene audio crash on WebGL, see : https://docs.unity3d.com/Manual/webgl-audio.html
            // and : https://issuetracker.unity3d.com/issues/webgl-fmod-error-spam-when-playing-audio-clip-with-timeline
#if UNITY_WEBGL
            if(level.introScene=="level_1_story_intro")
                SceneLoader.LoadSceneWithLoadingScreen("level_1_story_intro_webgl");
            else
#endif
            SceneLoader.LoadSceneWithLoadingScreen(level.introScene);

        }

        // load level
        else
            SceneLoader.LoadSceneWithLoadingScreen(levelEnum.ToString());
	}

}
