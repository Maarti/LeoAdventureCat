using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class VideoPlayerEvent : MonoBehaviour {

    public LevelEnum levelToLoad;
    public bool loadLevelOnFinish;

    VideoPlayer vid;

	void Start () {
        vid = GetComponent<VideoPlayer>();
	}
	
	void Update () {
        if (loadLevelOnFinish && !vid.isPlaying && Time.timeSinceLevelLoad > 1f)
            LoadLevel();
	}

    public void LoadLevel() {
        SceneLoader.LoadSceneWithLoadingScreen(levelToLoad.ToString());
    }
}
