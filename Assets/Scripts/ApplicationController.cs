using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;
using System.Linq;


public class ApplicationController : MonoBehaviour
{

	public static ApplicationController ac;
	public PlayerData playerData;
	public Dictionary<WorldEnum,World> worlds;
	public Dictionary<LevelEnum,Level> levels;
	public Dictionary<ItemEnum,Item> items;
	public string nextSceneToLoad = "";

	void Awake ()
	{
		if (ac == null) {
			DontDestroyOnLoad (gameObject);
			ac = this;
		} else if (ac != this) {
			Destroy (gameObject);
		}
		initWorlds ();
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

	void initWorlds ()
	{
		// Initialise all worlds
		Dictionary<WorldEnum,World> worlds = new Dictionary<WorldEnum, World> ();
		worlds.Add (WorldEnum.Forest, new World (WorldEnum.Forest, "1_FOREST", false));
		worlds.Add (WorldEnum.AnimalPound, new World (WorldEnum.AnimalPound, "2_ANIMAL_POUND", true));
        worlds.Add(WorldEnum.City, new World(WorldEnum.City, "3_CITY", true));
        this.worlds = worlds;
	}

	void initLevels ()
	{
		// Initialise all levels
		Dictionary<LevelEnum,Level> lvls = new Dictionary<LevelEnum, Level> ();
		lvls.Add (LevelEnum.level_1_story, new Level (LevelEnum.level_1_story, "1-", WorldEnum.Forest, DifficultyEnum.EASY, 10, 135, 3, LevelEnum.level_2_story, false, true));
		lvls.Add (LevelEnum.level_1_01, new Level (LevelEnum.level_1_01, "1-01", WorldEnum.Forest, DifficultyEnum.EASY, 10, 90, 3, LevelEnum.level_1_02, false));
		lvls.Add (LevelEnum.level_1_02, new Level (LevelEnum.level_1_02, "1-02", WorldEnum.Forest, DifficultyEnum.EASY, 10, 45, 0, LevelEnum.level_1_03, false));
		lvls.Add (LevelEnum.level_1_03, new Level (LevelEnum.level_1_03, "1-03", WorldEnum.Forest, DifficultyEnum.MEDIUM, 15, 80, 3, LevelEnum.level_1_06, false));
		//lvls.Add (LevelEnum.level_1_04, new Level (LevelEnum.level_1_04, "1-04", World.Forest, DifficultyEnum.MEDIUM, 15, 80, 3, LevelEnum.level_1_12, false));
		//lvls.Add (LevelEnum.level_1_05, new Level (LevelEnum.level_1_05, "1-05", World.Forest, DifficultyEnum.MEDIUM, 15, 80, 3, LevelEnum.level_1_12, false));
		lvls.Add (LevelEnum.level_1_06, new Level (LevelEnum.level_1_06, "1-06", WorldEnum.Forest, DifficultyEnum.HARD, 25, 125, 3, LevelEnum.level_1_11, false));
		//lvls.Add (LevelEnum.level_1_07, new Level (LevelEnum.level_1_07, "1-07", World.Forest, DifficultyEnum.MEDIUM, 15, 80, 3, LevelEnum.level_1_12, false));
		//lvls.Add (LevelEnum.level_1_08, new Level (LevelEnum.level_1_08, "1-08", World.Forest, DifficultyEnum.MEDIUM, 15, 80, 3, LevelEnum.level_1_12, false));
		//lvls.Add (LevelEnum.level_1_09, new Level (LevelEnum.level_1_09, "1-09", World.Forest, DifficultyEnum.MEDIUM, 15, 80, 3, LevelEnum.level_1_12, false));
		//lvls.Add (LevelEnum.level_1_10, new Level (LevelEnum.level_1_10, "1-10", World.Forest, DifficultyEnum.MEDIUM, 15, 80, 3, LevelEnum.level_1_12, false));
		lvls.Add (LevelEnum.level_1_11, new Level (LevelEnum.level_1_11, "1-11", WorldEnum.Forest, DifficultyEnum.MEDIUM, 15, 55, 1, LevelEnum.level_1_12, true));
		lvls.Add (LevelEnum.level_1_12, new Level (LevelEnum.level_1_12, "1-12", WorldEnum.Forest, DifficultyEnum.NIGHTMAR, 40, 120, 8, LevelEnum.level_1_01, true));
		lvls.Add (LevelEnum.level_2_story, new Level (LevelEnum.level_2_story, "2-", WorldEnum.AnimalPound, DifficultyEnum.MEDIUM, 25, 320, 5, LevelEnum.level_1_01, false, true));
		this.levels = lvls;
	}

	void initItems ()
	{
		// Init all items
		items = new Dictionary<ItemEnum, Item> ();
		items.Add (ItemEnum.level_1_11, new Item (ItemEnum.level_1_11, "LEVEL", "LEVEL_DESC", 50, LevelEnum.level_1_11));
		items.Add (ItemEnum.level_1_12, new Item (ItemEnum.level_1_12, "LEVEL", "LEVEL_DESC", 50, LevelEnum.level_1_12));
		items.Add (ItemEnum.max_life_1, new Item (ItemEnum.max_life_1, "ITEM_MAX_LIFE", "ITEM_MAX_LIFE_DESC", 200));
		items.Add (ItemEnum.max_life_2, new Item (ItemEnum.max_life_2, "ITEM_MAX_LIFE", "ITEM_MAX_LIFE_DESC", 500));
		items.Add (ItemEnum.max_life_3, new Item (ItemEnum.max_life_3, "ITEM_MAX_LIFE", "ITEM_MAX_LIFE_DESC", 1000));
		items.Add (ItemEnum.level_2_story, new Item (ItemEnum.level_2_story, "LEVEL", "LEVEL_DESC", 50, LevelEnum.level_2_story));
	}

	public void FinishLevel (LevelEnum level, int score = 100, bool doSave = true)
	{
		Level lvl = this.levels [level];
		if (lvl.score < score)
			lvl.score = score;
		this.playerData.setScore (level, score);

		// If level is a story, unlock the next world
		if (lvl.isStory)
			UnlockWorld (this.levels [lvl.nextLevel].world.id);

		if (doSave)
			Save ();
	}

	public void UnlockWorld (WorldEnum worldEnum, bool doSave = true)
	{		
		ApplicationController.ac.worlds [worldEnum].isLocked = false;
		if (!this.playerData.unlockedWorld.Contains (worldEnum))
			this.playerData.unlockedWorld.Add (worldEnum);
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

	public bool BuyItem (ItemEnum itemEnum, Text kittyzText = null, bool doSave = true, bool initMode = false)
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

			if (!initMode) {
				playerData.updateKittys (-price, kittyzText, false);

				// if the item is a "max_life" item
				if (Item.max_life_items.Contains (itemEnum)) {
					this.playerData.max_life++;
				}
			}

			if (doSave)
				Save ();

			return true;
		} else {
			return false;
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
		foreach (WorldEnum worldEnum in playerData.unlockedWorld) {
			UnlockWorld (worldEnum, false);
		}
		foreach (LevelEnum lvlEnum in playerData.unlockedLvls) {
			UnlockLevel (lvlEnum, false);
		}
		foreach (ItemEnum itemEnum in playerData.boughtItems) {
			BuyItem (itemEnum, null, false, true);
		}
		foreach (ItemEnum itemEnum in playerData.equippedItems) {
			EquipItem (itemEnum, true, false);
		}
		foreach (KeyValuePair<LevelEnum,int> entry in this.playerData.scores) {
			FinishLevel (entry.Key, entry.Value, false);
		}
	}

}


[Serializable]
public class PlayerData
{
	public int dataVersion = 1, kittyz = 0, lang_id = 0, max_life = 3;
	public List<LevelEnum> unlockedLvls;
	public List<ItemEnum> boughtItems, equippedItems;
	public List<WorldEnum> unlockedWorld;
	public Dictionary<LevelEnum,int> scores;
	public bool isMute = false;

	public PlayerData ()
	{		
		unlockedLvls = new List<LevelEnum> ();
		boughtItems = new List<ItemEnum> ();
		equippedItems = new List<ItemEnum> ();
		scores = new Dictionary<LevelEnum, int> ();
		unlockedWorld = new List<WorldEnum> (){ WorldEnum.Forest };
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
	public LevelEnum id;
	public string name;
	public bool isLocked, isStory;
	public World world;
	public int score = 0, targetKittyz, targetLife, targetTime;
	public DifficultyEnum difficulty;
	public LevelEnum nextLevel;

	public Level (LevelEnum id, string name, WorldEnum world, DifficultyEnum difficulty, int targetKittyz, int targetTime,
	              int targetLife, LevelEnum nexLevel, bool isLocked = true, bool isStory = false)
	{
		this.id = id;
		this.name = name;
		this.world = ApplicationController.ac.worlds [world];
		this.isLocked = isLocked;
		this.difficulty = difficulty;
		this.targetKittyz = targetKittyz;
		this.targetTime = targetTime;
		this.targetLife = targetLife;
		this.nextLevel = nexLevel;
		this.isStory = isStory;
	}

	public string GetFullName ()
	{
		string levelName = (this.isStory) ? this.name + LocalizationManager.Instance.GetText ("STORY") : this.name;
		return LocalizationManager.Instance.GetText ("LEVEL") + " " + levelName;
	}

	public Level GetNextUnlockedLevel ()
	{
		if (this.nextLevel == LevelEnum.none || this.nextLevel == LevelEnum.main_menu)
			return this;
			
		Level nextLvl = this;
		do {
			nextLvl = ApplicationController.ac.levels [nextLvl.nextLevel];		
		} while(nextLvl.isLocked && nextLvl.nextLevel != LevelEnum.none && nextLvl.nextLevel != LevelEnum.main_menu);
		return nextLvl;
	}

	public static bool isWorldLocked (LevelEnum levelEnum)
	{
		return ApplicationController.ac.levels [levelEnum].isWorldLocked ();
	}

	public bool isWorldLocked ()
	{
		return this.world.isLocked;
	}
}

public class World
{
	public WorldEnum id;
	public string name;
	public bool isLocked;

	public World (WorldEnum id, string name, bool isLocked)
	{
		this.id = id;
		this.name = name;
		this.isLocked = isLocked;
	}
}

public enum WorldEnum : int
{
	Forest = 1,
	AnimalPound = 2,
	City = 3,
	House = 4
}

public enum LevelEnum
{
	none,
	main_menu,
	level_1_story,
	level_1_01,
	level_1_02,
	level_1_03,
	level_1_04,
	level_1_05,
	level_1_06,
	level_1_07,
	level_1_08,
	level_1_09,
	level_1_10,
	level_1_11,
	level_1_12,
	level_1_13,
	level_1_14,
	level_1_15,
	level_1_16,
	level_1_17,
	level_1_18,
	level_1_19,
	level_1_20,
	level_2_story,
	level_2_01,
	level_2_02,
	level_2_03,
	level_2_04,
	level_2_05,
	level_2_06,
	level_2_07,
	level_2_08,
	level_2_09,
	level_2_10,
	level_2_11,
	level_2_12,
	level_2_13,
	level_2_14,
	level_2_15,
	level_2_16,
	level_2_17,
	level_2_18,
	level_2_19,
	level_2_20
}

public class Item
{
	public static ItemEnum[] max_life_items = { ItemEnum.max_life_1, ItemEnum.max_life_2, ItemEnum.max_life_3 };
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
	max_life_1,
	max_life_2,
	max_life_3,
	level_1_11,
	level_1_12,
	level_1_13,
	level_1_14,
	level_1_15,
	level_1_16,
	level_1_17,
	level_1_18,
	level_1_19,
	level_1_20,
	level_2_story
}

public enum DifficultyEnum
{
	EASY,
	MEDIUM,
	HARD,
	NIGHTMAR
}