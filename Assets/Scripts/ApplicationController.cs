using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;
using System.ComponentModel;

public class ApplicationController : MonoBehaviour
{

	public static ApplicationController ac;
	public PlayerData playerData;

	void Awake ()
	{
		if (ac == null) {
			DontDestroyOnLoad (gameObject);
			ac = this;
		} else if (ac != this) {
			Destroy (gameObject);
		}
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
		}
	}

	public void FinishLevel (LevelEnum level)
	{
		playerData.levels [level].isCompleted = true;
		Save ();
	}
}


[Serializable]
public class PlayerData
{
	public int kittyz = 0;
	//public List<KeyValuePair<string,Level>> levels;
	public Dictionary<LevelEnum,Level> levels;

	public PlayerData ()
	{
		initLevels ();
	}

	void initLevels ()
	{
		Dictionary<LevelEnum,Level> lvls = new Dictionary<LevelEnum, Level> ();
		lvls.Add (LevelEnum.level_1_01, new Level ("level_1_01", "1.01", World.Forest, 0, false));
		lvls.Add (LevelEnum.level_1_02, new Level ("level_1_02", "1.02", World.Forest, 0, false));
		lvls.Add (LevelEnum.level_1_03, new Level ("level_1_03", "1.03", World.Forest, 0, false));
		this.levels = lvls;
	}
}

[Serializable]
public class Level
{
	public string id, name;
	public int price;
	public bool isLock, isCompleted = false;
	public World world;

	public Level (string id, string name, World world, int price, bool isLock = true, bool isCompleted = false)
	{
		this.id = id;
		this.name = name;
		this.world = world;
		this.price = price;
		this.isLock = isLock;
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