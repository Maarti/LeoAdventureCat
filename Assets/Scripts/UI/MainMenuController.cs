using UnityEngine.UI;
using UnityEngine;
using GoogleMobileAds.Api;

public class MainMenuController : MonoBehaviour
{
    public GameObject helpScrollView;

    bool trackHelpAchievement = false;
    ScrollRect helpScrollRect;
    

    private void Start()
    {
        helpScrollRect = helpScrollView.GetComponent<ScrollRect>();
        LoadAd();
    }

    void Update ()
	{
		// Exit if Android back button is pressed
		if (Input.GetKeyDown (KeyCode.Escape))
			Application.Quit ();

        // Track the "Nosy" achievement on the help panel
        if (trackHelpAchievement && helpScrollRect.normalizedPosition.y < 0.1f)
        {
            PlayGamesScript.UnlockAchievement(Config.NOSY);
            trackHelpAchievement = false;
        }
	}

    // Called when opening the help panel to check if we need to track the "Nosy" achievement
    public void TrackHelpAchievement()
    {
        if (PlayGamesScript.IsAchievementUnlocked(Config.NOSY) == 0)
           trackHelpAchievement = true;
    }

    // Called when closing the help panel to stop tracking the achievement
    public void UntrackHelpAchievement()
    {
        trackHelpAchievement = false;
    }

    // Pre-load ad video in shop menu
    void LoadAd()
    {
        if (!RewardBasedVideoAd.Instance.IsLoaded())
        {
            AdRequest request = new AdRequest.Builder().Build();
            RewardBasedVideoAd.Instance.LoadAd(request, Config.adUnitId);
        }
    }
}
