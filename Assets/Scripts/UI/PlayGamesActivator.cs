#if UNITY_ANDROID || UNITY_IOS
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

public class PlayGamesActivator : MonoBehaviour {

    void Start()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        //PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        PlayGamesScript.SignIn();
    }
}

#else
using UnityEngine;
public class PlayGamesActivator : MonoBehaviour
{

    void Start() {
    }
}
#endif