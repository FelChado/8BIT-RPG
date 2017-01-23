using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Managers_Skills : MonoBehaviour 
{
	public Dictionary<string, Dictionary<string, float>> skillList = new Dictionary<string, Dictionary<string, float>>();
	public Dictionary<string, string> skillDescriptions = new Dictionary<string, string>();

	void Start ()
	{
		SkillMaker ("Sword Strike", "Deals light Slash damage to 1 foe.", 1, 0, 1, 140, 95, 5, 1, 1, 1, 0, 0, 0, 1);
		SkillMaker ("Heal", "Heals an Ally.", 4, 0, 1, 100, 100, 5, 0, 1, 4, 0, 0, 0, 1);
		SkillMaker ("Wide Blade", "Deals light Slash damage to 1 foe.", 1, 0, 1, 140, 100, 5, 1, 0, 0, 0, 0, 0, 1);
		SkillMaker ("Shoot", "Deals light Slash damage to 1 foe.", 1, 2, 1, 140, 85, 5, 1, 1, 0, 0, 0, 0, 1);
		SkillMaker ("Skewer", "Deals light Slash damage to 1 foe.", 1, 2, 1, 140, 85, 5, 1, 1, 0, 0, 0, 0, 1);
		SkillMaker ("Fire", "Deals fire damage.", 1, 3, 2, 140, 95, 5, 1, 1, 0, 0, 0, 0, 1);
	}

	void SkillMaker(string name, string description, float category, float element, float type, float power, float acc, float crit, 
	                float targetParty, float numOfTargets, float cost, float ailment, float attribute, 
	                float ailmentChance, float noOfHits)
	{
		this.skillList.Add (name, new Dictionary<string, float> ());
		Dictionary<string, float> currSkill = this.skillList [name];
		this.skillDescriptions.Add(name, description);
		currSkill.Add ("Category", category);
		currSkill.Add ("Element", element);
		currSkill.Add ("Type", type);
		currSkill.Add ("Power", power);
		currSkill.Add ("Acc", acc);
		currSkill.Add ("Crit", crit);
		currSkill.Add ("TargetParty", targetParty);
		currSkill.Add ("NumOfTargets", numOfTargets);
		currSkill.Add ("Cost", cost);
		currSkill.Add ("Ailment", ailment);
		currSkill.Add ("Attribute", attribute);
		currSkill.Add ("AilmentChance", ailmentChance);
		currSkill.Add ("NoOfHits", noOfHits);
	}
	/*
		Name: Name of the Skill
		Description: Displayed description of the skill
		Category: Type of skill
		1 - Damage (or damage with status effect, debuff)
		2 - Debuff
		3 - Buff
		4 - HP Healing
		5 - Ailment Healing
		Element: Type of Damage
		1 - Slash
		2 - Strike
		3 - Pierce
		4 - Fire
		5 - Ice
		6 - Thunder
		7 - Wind
		Type: What attributes will be used to calculate damage
		1 - Physical
		2 - Magical
		Power: Base Damage
		Accuracy: Rate of Hit
		Crit: Rate of Crit
		Target Party: Which party will be targeted
		0 - Yours
		1 - Theirs
		2 - Self
		Number of Targets:
		0 - All Targets
		1 - Single Targets
		2 or more: Number of random targets
		Cost: MP cost of skill
		Ailment: Placed status Ailment
		Attribute: Raised/Lowered Attribute
		Ailment Chance: Chance of having the target sustain the ailment
		No. of hits: Number of times skill will be executed per use.
	*/
}
