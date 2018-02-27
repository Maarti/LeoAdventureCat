using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldButtonInitializer : MonoBehaviour
{

	public WorldEnum worldEnum;
	public Sprite lockedImage, unlockedImage;
    public WorldUnlocker worldUnlocker;
	World world;
	Text worldName;
	Image image;
	bool isStarted = false;
    GameObject levelScrollView;


	// Use this for initialization
	void Start ()
	{
		world = ApplicationController.ac.worlds [worldEnum];
		worldName = transform.Find ("WorldName").gameObject.GetComponent<Text> ();
		image = transform.Find ("WorldImage").gameObject.GetComponent<Image> ();
        levelScrollView = transform.Find("LevelScrollView").gameObject;
		Init ();
		// When object is Started, OnEnable() can be called
		isStarted = true;
	}

	void OnEnable ()
	{
		if (isStarted)
			Start ();
	}

	public void Init ()
	{
		if (world.isLocked) {
			worldName.text = worldEnum.GetHashCode ().ToString () + " - " + LocalizationManager.Instance.GetText ("LOCKED_WORLD");
			image.sprite = lockedImage;
			//image.raycastTarget = false;
		} else {
			worldName.text = LocalizationManager.Instance.GetText (world.name);
			image.sprite = unlockedImage;
			image.raycastTarget = true;
		}
	}

    public void OnWorldClic() {
        if (world.isLocked) {
            worldUnlocker.DisplayPopup(this);
        } else {
            levelScrollView.SetActive(true);
        }
    }
}
