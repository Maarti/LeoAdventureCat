using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemController : MonoBehaviour
{

	public ItemEnum itemEnum;
	public Text kittyzText;
	Text itemNameText, itemDescText, itemPriceText;
	GameObject itemBuyButton;
	Item item;
	bool isStarted = false;

	void Start ()
	{
		itemNameText = GameObject.Find (this.name + "/ItemTitle").GetComponent<Text> ();
		itemDescText = GameObject.Find (this.name + "/ItemDesc").GetComponent<Text> ();
		itemPriceText = GameObject.Find (this.name + "/ItemPrice/PriceText").GetComponent<Text> ();
		itemBuyButton = GameObject.Find (this.name + "/BuyButton");

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
			Debug.Log ("buy this");
			ApplicationController.ac.BuyItem (itemEnum, kittyzText);
			InitButton ();
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
