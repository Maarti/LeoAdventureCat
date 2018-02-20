using UnityEngine;
using UnityEngine.UI;

public class ShopItemController : MonoBehaviour
{

	public ItemEnum itemEnum;
	public Text kittyzText;
	public Sprite boughtImg, equipImg, unequipImg;
	Text itemNameText, itemDescText, itemPriceText;
	GameObject itemBuyButton, itemEquipButton, itemPrice;
	private Item item;
	bool isStarted = false;
	AudioSource audioSource;

	void Start ()
	{
		itemNameText = GameObject.Find (this.name + "/ItemTitle").GetComponent<Text> ();
		itemDescText = GameObject.Find (this.name + "/ItemDesc").GetComponent<Text> ();
		itemPriceText = GameObject.Find (this.name + "/ItemPrice/PriceText").GetComponent<Text> ();
		itemBuyButton = GameObject.Find (this.name + "/BuyButton");
        itemEquipButton = GameObject.Find(this.name + "/EquipButton");
        itemPrice = GameObject.Find (this.name + "/ItemPrice");
		audioSource = GetComponent<AudioSource> ();
        itemEquipButton.SetActive(false);

		InitButton ();
		isStarted = true;
	}

	public void InitButton ()
	{
		if (itemEnum != ItemEnum.none) {
			item = ApplicationController.ac.items [itemEnum];
			if (item.level != LevelEnum.none) {
				if (Level.isWorldLocked (item.level)) {
					Destroy (this.gameObject);
					return;
				}
			}
			itemNameText.text = item.GetName ();
			itemDescText.text = item.GetDesc ();
			itemPriceText.text = item.price.ToString ();
			if (item.isBought) {
				itemBuyButton.GetComponent<Button> ().interactable = false;
				itemPrice.SetActive (false);
				itemBuyButton.transform.Find ("Image").GetComponent<Image> ().sprite = boughtImg;
                // Display equip/unequip button if item is equipable
                if (item.isEquipable)
                {
                    itemEquipButton.SetActive(true);
                    if (IsItemEquiped())
                        itemEquipButton.transform.Find("Image").GetComponent<Image>().sprite = unequipImg;
                    else
                        itemEquipButton.transform.Find("Image").GetComponent<Image>().sprite = equipImg;
                }
                else
                    GetComponent<RectTransform>().SetAsLastSibling(); // move the item at the end of the list
			}
		}
	}

	public void BuyThis ()
	{
		if (itemEnum != ItemEnum.none) {
			if (ApplicationController.ac.BuyItem (itemEnum, kittyzText)) {
				audioSource.PlayOneShot (audioSource.clip);
				InitButton ();
                if (item.isEquipable)
                    EquipThis();
			}
		}
	}

    // Equip/Unequip the item
    public void EquipThis()
    {
        if (itemEnum != ItemEnum.none)
        {
            if (IsItemEquiped())
                ApplicationController.ac.UnequipItem(item.type);
            else
                ApplicationController.ac.EquipItem(itemEnum);
            //audioSource.PlayOneShot(audioSource.clip);
            InitButton();
            RefreshSiblings(); // refresh the "equipped" label and button of same type items
        }
    }

    bool IsItemEquiped()
    {
        return Item.IsEquiped(item);
    }

    // Refresh the siblings buttons with the same item type
    void RefreshSiblings()
    {
        ShopItemController[] ctrlrs = this.transform.parent.GetComponentsInChildren<ShopItemController>();
        foreach(ShopItemController ctrlr in ctrlrs)
        {
            if (ctrlr.item.type == this.item.type)
                ctrlr.InitButton();
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
