using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

// Documentation : https://github.com/playgameservices/play-games-plugin-for-unity

public class PlayGamesScript : MonoBehaviour
{
    public static int perfectionnistCurrentValue = -1;

    public void ShowAllLeaderboards()
    {
        PlayGamesScript.ShowLeaderboardsUI();
    }
    public void ShowOverallLeaderboards()
    {
        PlayGamesScript.ShowSpecificLeaderboardsUI(Config.LEADERBOARD_OVERALL_SCORE);
    }

    public void ShowAchievements()
    {
        PlayGamesScript.ShowAchievementsUI();
    }

    #region StaticMethods
    public static void SignIn()
    {
       /* Authentication will show the required consent dialogs.If the user
        * has already signed into the game in the past, this process will 
        * be silent and the user will not have to interact with any dialogs.*/
       Social.localUser.Authenticate((bool success) => {
           Debug.Log("Google Play Games : authenticate=" + success);
           perfectionnistCurrentValue = GetAchievementValue(Config.PERFECTIONIST_20);
           Debug.Log("init perfectionnistCurrentValue=" + PlayGamesScript.perfectionnistCurrentValue);
       });
    }

    #region Achievements
    public static void UnlockAchievement(string id)
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated())
            Social.ReportProgress(id, 100.0f, success => { });        
    }
    
    public static void IncrementAchievement(string id, int stepsToIncrement)
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated())
            PlayGamesPlatform.Instance.IncrementAchievement(id, stepsToIncrement, success => { });
    }

    public static void ShowAchievementsUI()
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated())
            Social.ShowAchievementsUI();
        else
            SignIn();
    }

    // Return the current step value of an incremental achievement
    public static int GetAchievementValue(string id)
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            Achievement a = PlayGamesPlatform.Instance.GetAchievement(id);
            if (a.IsIncremental)
                return a.CurrentSteps;
        }
        return -1;
    }

    // Return 0 if locked, 1 if unlocked, -1 if not c0nnected
    public static int IsAchievementUnlocked(string id)
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            if (PlayGamesPlatform.Instance.GetAchievement(id).IsUnlocked)
                return 1;
            else
                return 0;
        }
        return -1;
    }

    // Reveal a hidden achievement
    public static void RevealAchievement(string id)
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            Social.ReportProgress(id, 0.0f, (bool success) =>
            {
                Debug.Log("Revealing achievement " + id);
            });
        }
    }
    #endregion

    #region LeaderBoard
    public static void AddScoreToLeaderboard(string leaderboardId, long score)
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated())
            Social.ReportScore(score, leaderboardId, (bool success) =>
            {
                Debug.Log("Report Score :" + score + " => " + success);
            });       
    }

    public static void ShowLeaderboardsUI()
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated())
            Social.ShowLeaderboardUI();
        else
            SignIn();
    }

    public static void ShowSpecificLeaderboardsUI(string id)
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated())
            PlayGamesPlatform.Instance.ShowLeaderboardUI(id);
        else
            SignIn();
    }
    #endregion

    #endregion


}