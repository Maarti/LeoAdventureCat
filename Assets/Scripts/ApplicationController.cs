using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;
using System.ComponentModel;


public class ApplicationController : MonoBehaviour
{

	public static ApplicationController ac;
	public PlayerData playerData;
	public Dictionary<LevelEnum,Level> levels;
	public Dictionary<ItemEnum,Item> items;

	void Awake ()
	{
		if (ac == null) {
			DontDestroyOnLoad (gameObject);
			ac = this;
		} else if (ac != this) {
			Destroy (gameObject);
		}
		initLevels ();
		initItems ();
		Load ();
	}


	public void Save ()
	{
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/playerData.dat");
		bf.Serialize (file, playerData);
		file.Close ();
	}

	public void Load ()
	{
		if (File.Exists (Application.persistentDataPath + "/playerData.dat")) {
			Debug.Log ("Save found :" + Application.persistentDataPath);
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/playerData.dat", FileMode.Open);
			this.playerData = (PlayerData)bf.Deserialize (file);
			file.Close ();
		} else {
			playerData = new PlayerData ();
			if (Application.systemLanguage == SystemLanguage.French)
				playerData.lang_id = 1;
			else
				playerData.lang_id = 0;
			Save ();
		}
		// Update initial data with saved player data
		MergeData ();
	}

	void initLevels ()
	{
		// Initialise all levels
		Dictionary<LevelEnum,Level> lvls = new Dictionary<LevelEnum, Level> ();
		lvls.Add (LevelEnum.level_1_01, new Level ("level_1_01", "1.01", World.Forest, /*0,*/false));
		lvls.Add (LevelEnum.level_1_02, new Level ("level_1_02", "1.02", World.Forest, /*0,*/true));
		lvls.Add (LevelEnum.level_1_03, new Level ("level_1_03", "1.03", World.Forest, /*100,*/true));
		this.levels = lvls;
	}

	void initItems ()
	{
		// Init all items
		items = new Dictionary<ItemEnum, Item> ();
		items.Add (ItemEnum.level_1_02, new Item (ItemEnum.level_1_02, "LEVEL", "LEVEL_DESC", 50, LevelEnum.level_1_02));
		items.Add (ItemEnum.level_1_03, new Item (ItemEnum.level_1_03, "LEVEL", "LEVEL_DESC", 99, LevelEnum.level_1_03));
	}

	public void FinishLevel (LevelEnum level, int score = 100, bool doSave = true)
	{
		if (this.levels [level].score < score)
			this.levels [level].score = score;
		this.playerData.setScore (level, score);
		if (doSave)
			Save ();
	}

	public void UnlockLevel (LevelEnum level, bool doSave = true)
	{
		if (level != LevelEnum.none) {
			this.levels [level].isLocked = false;
			if (!this.playerData.unlockedLvls.Contains (level))
				this.playerData.unlockedLvls.Add (level);
			if (doSave)
				Save ();
		}
	}

	public void BuyItem (ItemEnum itemEnum, Text kittyzText = null, bool doSave = true, bool initMode = false)
	{
		int price = this.items [itemEnum].price;
		if (price <= playerData.kittyz || initMode) {
			this.items [itemEnum].isBought = true;
			if (!this.playerData.boughtItems.Contains (itemEnum))
				this.playerData.boughtItems.Add (itemEnum);	

			// if the item is a level, unlock the level
			if (this.items [itemEnum].level != LevelEnum.none) {
				UnlockLevel (this.items [itemEnum].level, false);
			}

			if (!initMode)
				playerData.updateKittys (-price, kittyzText, false);

			if (doSave)
				Save ();
		}
	}

	// Equip or Unequip item (bool equip = false to unequip)
	public void EquipItem (ItemEnum itemEnum, bool equip = true, bool doSave = true)
	{
		this.items [itemEnum].isEquipped = equip;
		if (equip && !this.playerData.equippedItems.Contains (itemEnum))
			this.playerData.equippedItems.Add (itemEnum);
		else if (!equip && this.playerData.equippedItems.Contains (itemEnum))
			this.playerData.equippedItems.Remove (itemEnum);
		if (doSave)
			Save ();
	}

	// Merge initial data with the saved data of the player
	public void MergeData ()
	{
		foreach (LevelEnum lvlEnum in playerData.unlockedLvls) {
			UnlockLevel (lvlEnum, false);
		}
		/*foreach (LevelEnum lvlEnum in playerData.completedLvls) {
			FinishLevel (lvlEnum, false);
		}*/
		foreach (ItemEnum itemEnum in playerData.boughtItems) {
			BuyItem (itemEnum, null, false, true);
		}
		foreach (ItemEnum itemEnum in playerData.equippedItems) {
			EquipItem (itemEnum, true, false);
		}
		foreach (KeyValuePair<LevelEnum,int> entry in this.playerData.scores) {
			FinishLevel (entry.Key, entry.Value, false);
			//this.levels [entry.Key].score = entry.Value;
		}
	}

}


[Serializable]
public class PlayerData
{
	public int dataVersion = 1, kittyz = 0, lang_id = 0;
	public List<LevelEnum> unlockedLvls, completedLvls;
	public List<ItemEnum> boughtItems, equippedItems;
	public Dictionary<LevelEnum,int> scores;

	public PlayerData ()
	{		
		unlockedLvls = new List<LevelEnum> ();
		completedLvls = new List<LevelEnum> ();
		boughtItems = new List<ItemEnum> ();
		equippedItems = new List<ItemEnum> ();
		scores = new Dictionary<LevelEnum, int> ();
		// lang_id is initialized in Load()				
	}

	public int updateKittys (int kittyz, Text uiText = null, bool doSave = false)
	{
		this.kittyz = Mathf.Clamp (this.kittyz + kittyz, 0, 9999); // set min and max to kittyz
		if (doSave)
			ApplicationController.ac.Save ();
		if (uiText != null)
			uiText.text = this.kittyz.ToString ();
		return this.kittyz;
	}

	public void setScore (LevelEnum lvl, int score)
	{
		if (this.scores.ContainsKey (lvl)) {
			if (this.scores [lvl] < score)
				this.scores [lvl] = Mathf.Clamp (score, 0, 100);
		} else {
			this.scores.Add (lvl, Mathf.Clamp (score, 0, 100));
		}
	}
}

public class Level
{
	public string id, name;
	//public int price;
	public bool isLocked;
	public World world;
	public int score = 0;

	public Level (string id, string name, World world, /*int price,*/bool isLocked = true)
	{
		this.id = id;
		this.name = name;
		this.world = world;
		//this.price = price;
		this.isLocked = isLocked;
	}
}

public enum World : int
{
	Forest = 1,
	AnimalPound = 2
}

public enum LevelEnum
{
	none,
	main_menu,
	level_1_01,
	level_1_02,
	level_1_03
}

public class Item
{
	public ItemEnum id;
	public bool isBought = false, isEquipped = false;
	public int price;
	public LevelEnum level;
	string name_id, desc_id;

	public Item (ItemEnum id, string name_id, string desc_id, int price, LevelEnum level = LevelEnum.none)
	{
		this.id = id;
		this.name_id = name_id;
		this.desc_id = desc_id;
		this.price = price;
		this.level = level;
	}

	public string GetName ()
	{
		if (this.level != LevelEnum.none)
			return LocalizationManager.Instance.GetText ("LEVEL") + " " + ApplicationController.ac.levels [level].name;
		else
			return LocalizationManager.Instance.GetText (name_id);
	}

	public string GetDesc ()
	{
		return LocalizationManager.Instance.GetText (desc_id);
	}
		
}

public enum ItemEnum
{
	none,
	level_1_02,
	level_1_03
}
