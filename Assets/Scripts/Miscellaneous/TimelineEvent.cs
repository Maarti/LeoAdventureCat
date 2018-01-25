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
        {
            SceneLoader.LoadSceneWithLoadingScreen(levelToLoad.ToString());
        }
    }
}
