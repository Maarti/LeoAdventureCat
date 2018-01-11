using UnityEngine;
using UnityEngine.UI;

public class HelpItemController : MonoBehaviour {

    public WorldEnum worldEnum;
    public GameObject lockImg,beastTxt,beastImg;
    public Sprite interroImg;
    World world;
    bool isStarted = false;

    // Use this for initialization
    void Start () {
        world = ApplicationController.ac.worlds[worldEnum];
        InitItem();
        // When object is Started, OnEnable() can be called
        isStarted = true;
	}
	
	void InitItem()
    {
        if (world.isLocked)
        {
            beastTxt.SetActive(false);
            lockImg.SetActive(true);
            beastImg.GetComponent<Image>().sprite = interroImg;
        }
        else
        {
            beastTxt.SetActive(true);
            lockImg.SetActive(false);
        }
    }

    void OnEnable()
    {
        if (isStarted)
            InitItem();
    }
}
