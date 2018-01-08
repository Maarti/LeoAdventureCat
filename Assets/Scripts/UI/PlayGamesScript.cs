using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

public class PlayGamesScript : MonoBehaviour
{
     
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
        });
    }

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


}