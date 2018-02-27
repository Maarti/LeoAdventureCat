using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class KittyzDisplayer : MonoBehaviour {

    Text kittyzTxt;
    bool isInit = false;


    private void Start() {
        kittyzTxt = GetComponent<Text>();
        Init();
        isInit = true;
    }

    private void OnEnable() {
        if (!isInit)
            Start();
        else
            Init();
    }

    public void Init() {
        kittyzTxt.text = ApplicationController.ac.playerData.kittyz.ToString();
    }
}
