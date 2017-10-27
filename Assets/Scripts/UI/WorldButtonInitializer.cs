using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldButtonInitializer : MonoBehaviour
{

	public WorldEnum worldEnum;
	public Sprite lockedImage, unlockedImage;
	World world;
	Text worldName;
	Image image;
	bool isStarted = false;


	// Use this for initialization
	void Start ()
	{
		world = ApplicationController.ac.worlds [worldEnum];
		worldName = transform.Find ("WorldName").gameObject.GetComponent<Text> ();
		image = transform.Find ("WorldImage").gameObject.GetComponent<Image> ();
		Init ();
		// When object is Started, OnEnable() can be called
		isStarted = true;
	}

	void OnEnable ()
	{
		if (isStarted)
			Start ();
	}

	void Init ()
	{
		if (world.isLocked) {
			worldName.text = worldEnum.GetHashCode ().ToString () + " - " + LocalizationManager.Instance.GetText ("LOCKED_WORLD");
			image.sprite = lockedImage;
			image.raycastTarget = false;
		} else {
			worldName.text = LocalizationManager.Instance.GetText (world.name);
			image.sprite = unlockedImage;
			image.raycastTarget = true;
		}
	}
}
