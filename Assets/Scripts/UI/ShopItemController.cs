using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemController : MonoBehaviour
{

	public ItemEnum itemEnum;
	public Text kittyzText;
	public Sprite boughtImg;
	Text itemNameText, itemDescText, itemPriceText;
	GameObject itemBuyButton, itemPrice;
	Item item;
	bool isStarted = false;
	AudioSource audioSource;

	void Start ()
	{
		itemNameText = GameObject.Find (this.name + "/ItemTitle").GetComponent<Text> ();
		itemDescText = GameObject.Find (this.name + "/ItemDesc").GetComponent<Text> ();
		itemPriceText = GameObject.Find (this.name + "/ItemPrice/PriceText").GetComponent<Text> ();
		itemBuyButton = GameObject.Find (this.name + "/BuyButton");
		itemPrice = GameObject.Find (this.name + "/ItemPrice");
		audioSource = GetComponent<AudioSource> ();

		InitButton ();
		isStarted = true;
	}

	public void InitButton ()
	{
		if (itemEnum != ItemEnum.none) {
			item = ApplicationController.ac.items [itemEnum];
			itemNameText.text = item.GetName ();
			itemDescText.text = item.GetDesc ();
			itemPriceText.text = item.price.ToString ();
			if (item.isBought) {
				itemBuyButton.GetComponent<Button> ().interactable = false;
				itemPrice.SetActive (false);
				itemBuyButton.transform.Find ("Image").GetComponent<Image> ().sprite = boughtImg;
				//itemBuyButton.gameObject.SetActive (false);
				//itemBuyButton.GetComponentInChildren<Text> ().text = LocalizationManager.Instance.GetText ("BOUGHT");
			} else {
				//itemBuyButton.GetComponentInChildren<Text> ().text = LocalizationManager.Instance.GetText ("BUY");
			}
		}
	}

	public void BuyThis ()
	{
		if (itemEnum != ItemEnum.none) {
			if (ApplicationController.ac.BuyItem (itemEnum, kittyzText)) {
				audioSource.PlayOneShot (audioSource.clip);
				InitButton ();
			}
		}
	}


	public void Cheat ()
	{
		ApplicationController.ac.playerData.updateKittys (50, kittyzText, true);
	}

	void OnEnable ()
	{
		if (isStarted)
			InitButton ();
	}
}
