using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers_EncounterManager : MonoBehaviour
{
	public Dictionary<string, List<string>> encounterList = new Dictionary<string, List<string>>(); 
	public List<string> currEncounterMonsters;

	void Start()
	{
		DontDestroyOnLoad(this.gameObject);
		AddEncounter("Prison1", returnStrList(new string[]{"Goblin"}));
		AddEncounter("Prison2", returnStrList(new string[]{"Goblin", "Goblin"}));
		AddEncounter("Prison3", returnStrList(new string[]{"Goblin", "Goblin", "Goblin"}));
	}

	List<string> returnStrList(string[] skills)
	{
		return new List<string>(skills);
	}

	void AddEncounter(string label, List<string> monsters)
	{
		this.encounterList.Add(label, monsters);
	}
}
