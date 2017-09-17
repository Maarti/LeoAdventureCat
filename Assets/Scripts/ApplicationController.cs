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

	void Awake ()
	{
		if (ac == null) {
			DontDestroyOnLoad (gameObject);
			ac = this;
		} else if (ac != this) {
			Destroy (gameObject);
		}
		Load ();
		initLevels ();
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
		}
	}

	void initLevels ()
	{
		// Initialise all levels
		Dictionary<LevelEnum,Level> lvls = new Dictionary<LevelEnum, Level> ();
		lvls.Add (LevelEnum.level_1_01, new Level ("level_1_01", "1.01", World.Forest, 0, false));
		lvls.Add (LevelEnum.level_1_02, new Level ("level_1_02", "1.02", World.Forest, 0, false));
		lvls.Add (LevelEnum.level_1_03, new Level ("level_1_03", "1.03", World.Forest, 0, true));
		this.levels = lvls;
		// Update levels with player data
		MergeData ();
	}

	public void FinishLevel (LevelEnum level, bool doSave = true)
	{
		this.levels [level].isCompleted = true;
		if (!this.playerData.completedLvls.Contains (level))
			this.playerData.completedLvls.Add (level);
		if (doSave)
			Save ();
	}

	public void UnlockLevel (LevelEnum level, bool doSave = true)
	{
		this.levels [level].isLocked = false;
		if (!this.playerData.unlockedLvls.Contains (level))
			this.playerData.unlockedLvls.Add (level);
		if (doSave)
			Save ();
	}

	public void MergeData ()
	{
		foreach (LevelEnum lvlEnum in playerData.unlockedLvls) {
			UnlockLevel (lvlEnum, false);
		}
		foreach (LevelEnum lvlEnum in playerData.completedLvls) {
			FinishLevel (lvlEnum, false);
		}
	}

}


[Serializable]
public class PlayerData
{
	public int dataVersion = 1, kittyz = 0;
	public List<LevelEnum> unlockedLvls, completedLvls;

	public PlayerData ()
	{		
		unlockedLvls = new List<LevelEnum> ();
		completedLvls = new List<LevelEnum> ();
	}

	public int updateKittys (int kittyz, Text uiText = null, bool doSave = false)
	{
		this.kittyz += kittyz;
		if (doSave)
			ApplicationController.ac.Save ();
		if (uiText != null)
			uiText.text = this.kittyz.ToString ();
		return this.kittyz;
	}


}

[Serializable]
public class Level
{
	public string id, name;
	public int price;
	public bool isLocked, isCompleted = false;
	public World world;

	public Level (string id, string name, World world, int price, bool isLocked = true, bool isCompleted = false)
	{
		this.id = id;
		this.name = name;
		this.world = world;
		this.price = price;
		this.isLocked = isLocked;
		this.isCompleted = isCompleted;
	}
}

public enum World : int
{
	Forest = 1,
	AnimalPound = 2
}

public enum LevelEnum
{
	main_menu,
	level_1_01,
	level_1_02,
	level_1_03
}