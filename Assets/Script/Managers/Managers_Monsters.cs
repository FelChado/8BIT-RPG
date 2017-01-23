using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers_Monsters : MonoBehaviour 
{
	public Dictionary<string, Dictionary<string, int>> MonsterProperties = new Dictionary<string, Dictionary<string, int>>();
	public Dictionary<string, List<string>> MonsterSkills = new Dictionary<string, List<string>>();
	public Dictionary<string, List<int>> MonsterSkillPriorities = new Dictionary<string, List<int>>();
	public Dictionary<string, List<float>> MonsterWeaknessess = new Dictionary<string, List<float>>();

	public void CreateMonsterDatabase()
	{
		AddMonster("Goblin", 3, 1, 5, 1, 3, 2, 3, 1, 1, 1 , 1, 2, 1, 1, 1, 3, 
		returnStrList(new string[]{"Wide Blade", "Sword Strike"}),
		returnIntList(new int[]{3, 1}));
	}

	List<string> returnStrList(string[] skills)
	{
		return new List<string>(skills);
	}

	List<int> returnIntList(int[] priorities)
	{
		return new List<int>(priorities);
	}

	void AddMonster(string name, int level, int hp, int strenght, int magic, int defense, int mind, int agility, int luck, float slash, 
					float strike, float pierce, float fire, float ice, float thunder, float wind, int stag, List<string> skills, List<int> skillPriorities)
	{
		this.MonsterProperties.Add(name, new Dictionary<string, int>());
		this.MonsterProperties[name].Add("Level", level);
		this.MonsterProperties[name].Add("HP", hp);
		this.MonsterProperties[name].Add("Str", strenght);
		this.MonsterProperties[name].Add("Mag", magic);
		this.MonsterProperties[name].Add("Def", defense);
		this.MonsterProperties[name].Add("Mnd", mind);
		this.MonsterProperties[name].Add("Agi", agility);
		this.MonsterProperties[name].Add("Luc", luck);
		this.MonsterProperties[name].Add("Stag", stag);
		MonsterSkills.Add(name, skills);
		MonsterSkillPriorities.Add(name, skillPriorities);
		MonsterWeaknessess.Add(name, new List<float>(new float[]{ slash, strike, pierce, fire, ice, thunder, wind}));
	}


}
