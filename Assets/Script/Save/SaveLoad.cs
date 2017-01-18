using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad 
{
	public static List<SavedData> savedGames = new List<SavedData>();

	public static void Save()
	{
		if (savedGames.Count > 0)
			savedGames[SavedData.currentSave] = SavedData.current;
		else
			savedGames.Add(SavedData.current);
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/savedGames.gd");
		bf.Serialize(file, SaveLoad.savedGames);
		file.Close();
	}

	public static void Load()
	{
		if (File.Exists(Application.persistentDataPath + "/savedGames.gd"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
			SaveLoad.savedGames = (List<SavedData>)bf.Deserialize(file);
			file.Close();
		}
	}
}
