using UnityEngine;
using UnityEngine.Playables;

public class TimelineEvent : MonoBehaviour {

    public LevelEnum levelToLoad;
    PlayableDirector pd;

	void Start () {
        pd = GetComponent<PlayableDirector>();
	}
	

	void Update () {
        if (pd.state != PlayState.Playing)
            LoadLevel();
    }

    public void LoadLevel()
    {
        pd.Pause();
        SceneLoader.LoadSceneWithLoadingScreen(levelToLoad.ToString());
    }
}
