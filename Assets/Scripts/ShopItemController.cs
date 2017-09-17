using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemController : MonoBehaviour
{
	public LevelEnum level;
	public Text kittyzText;


	public void BuyLevel ()
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
	}

	public void Cheat ()
	{
		ApplicationController.ac.playerData.updateKittys (50, kittyzText, true);
	}
}
