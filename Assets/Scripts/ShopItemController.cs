using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemController : MonoBehaviour
{

	public ItemEnum itemEnum;
	public Text kittyzText;
	Text itemNameText, itemDescText, itemPriceText;
	Item item;

	void Start ()
	{
		itemNameText = GameObject.Find (this.name + "/ItemTitle").GetComponent<Text> ();
		itemDescText = GameObject.Find (this.name + "/ItemDesc").GetComponent<Text> ();
		itemPriceText = GameObject.Find (this.name + "/ItemPrice/PriceText").GetComponent<Text> ();

		InitButton ();
	}

	void InitButton ()
	{
		item = ApplicationController.ac.items [itemEnum];
		itemNameText.text = item.GetName ();
		itemDescText.text = item.GetDesc ();
		itemPriceText.text = item.price.ToString ();
	}

	public void BuyThis ()
	{
		ApplicationController.ac.BuyItem (itemEnum, kittyzText);
	}

	/*	public void BuyLevel ()
	{
		int levelPrice = ApplicationController.ac.levels [level].price;
		int kittyz = ApplicationController.ac.playerData.kittyz;
		if (levelPrice <= kittyz) {
			//return false;
			//else {
			Debug.Log ("ok");
			ApplicationController.ac.UnlockLevel (level);
			ApplicationController.ac.playerData.updateKittys (-levelPrice, kittyzText, true);
			//return true;
		}
	}*/

	public void Cheat ()
	{
		ApplicationController.ac.playerData.updateKittys (50, kittyzText, true);
	}
}
