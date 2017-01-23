using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BattleSystem_ExecuteActions : MonoBehaviour 
{
	#region Variables

	// Battle Reference
	[SerializeField]
	BattleSystem_References battleRef;
	// Objects
	[SerializeField]
	GameObject damagePrefab = null, healPrefab = null, weaknessPrefab = null, canvasObject = null;
	GameObject damageCounterSpawn, weaknessSpawn;

	// Complete skill database
	Dictionary<string, Dictionary<string, float>> skillList = new Dictionary<string, Dictionary<string, float>>();

	// Currently used skill
	Dictionary<string, float> currSkill = new Dictionary<string, float>();

	// Current action actors
	Characters_Global currActor, currReceiver;

	// If current skill will stagger
	bool willStagger;

	#endregion

	void Awake()
	{
		this.skillList = battleRef.SkillManager.skillList;
	}

	public void ExecuteActions(Characters_Global actor, Characters_Global receiver)
	{
		this.currActor = actor;
		this.currReceiver = receiver;
		switch (actor.ActionType)
		{
			case "Attack":
			{
				TargetNumFilter (currActor.UsedAction);
			}
			break;
			case "Skill":
			{
				TargetNumFilter (currActor.UsedAction);
				if(actor.CompareTag("Ally"))
					actor.GetComponent<Characters_Allies>().Temp_CurrStam -= (int)this.skillList[actor.UsedAction]["Cost"];
			}
			break;
			case "Guard":
			{
				Guard ();
			}
			break;
			case "Switch":
			{
				Switch ();
			}
			break;
		}
		actor.TargetCharacter = null;
		actor.UsedAction = "";
		actor.ActionType = "";
		this.willStagger = false;
		
	}

	#region Skill Processing

	void TargetNumFilter(string usedAction)
	{
		this.currSkill = this.skillList [currActor.UsedAction];
		switch ((int)this.currSkill ["NumOfTargets"])
		{
			case 0:
				if (this.currReceiver.CompareTag ("Ally")) {
					for (int i = 0; i < this.battleRef.Sys_Master.allyList.Count; i++) 
					{
						this.currReceiver = this.battleRef.Sys_Master.allyList [i];
						if(!currReceiver.GetComponent<Characters_Allies>().Fainted)
							FilterSkillType (usedAction);
					}
				}
				if (this.currReceiver.CompareTag ("Enemy"))
				{
					for (int i = 0; i < this.battleRef.Sys_Master.enemyList.Count; i++)
					 {
						this.currReceiver = this.battleRef.Sys_Master.enemyList [i];
						FilterSkillType (usedAction);
					}
				}
			break;
			case 1:
				FilterSkillType (usedAction);
			break;
		}
	}

	void FilterSkillType(string usedAction)
	{
		switch((int)this.currSkill["Category"])
		{
			case 1:
				Damage ();
			break;
			case 4:
				Heal ();
			break;
		}
	}

	#endregion

	#region Battle Action Call

	void Guard()
	{

	}

	void Switch()
	{
		this.currActor.GetComponent<Characters_Allies> ().WriteTempData ();
		this.currActor.GetStats (this.currActor.GetComponent<Characters_Allies> ().SwitchedCharacter);
	}

	void Damage()
	{
		this.damageCounterSpawn = (GameObject)GameObject.Instantiate(this.damagePrefab, this.currReceiver.transform.position, Quaternion.identity);
		this.damageCounterSpawn.transform.SetParent(this.canvasObject.transform);
		this.damageCounterSpawn.transform.localScale = new Vector3 (0.32f, 0.32f, 1);
		// Hit or Miss
		if (Random.Range (0, 100) < this.currSkill ["Acc"]) {
			float offensiveStat, defensiveStat;
			// Define Type
			if (this.currSkill ["Type"] == 1) 
			{
				offensiveStat = this.currActor.Stat_Str;
				defensiveStat = this.currReceiver.Stat_Def;
			} 
			else 
			{
				offensiveStat = this.currActor.Stat_Mag;
				defensiveStat = this.currReceiver.Stat_Min;
			}
			// Damage Formula
			float damage = (((float)this.currActor.Stat_Level * 2 / 250) * (offensiveStat / defensiveStat) 
				* this.currSkill ["Power"]) * Random.Range (0.8f, 1.1f);
			if (damage < 0)
				damage = 0;
			else if (Random.Range (0, 100) < this.currSkill ["Crit"])
				damage *= 1.5f;
			damage *= ReturnTypeEffectiveness ((int)this.currSkill ["Element"]);
			this.damageCounterSpawn.transform.GetChild(0).GetComponent<Text>().text = ((int)damage).ToString();
			this.currReceiver.GetDamaged ((int)damage, (int)this.currSkill ["Type"], willStagger);
		} 
		else 
		{
			this.damageCounterSpawn.transform.GetChild(0).GetComponent<Text>().text = "Miss";
		}
	}

	void Heal()
	{
		this.damageCounterSpawn = (GameObject)GameObject.Instantiate(this.healPrefab, this.currReceiver.transform.position, Quaternion.identity);
		this.damageCounterSpawn.transform.SetParent(this.canvasObject.transform);
		this.damageCounterSpawn.transform.localScale = new Vector3 (0.32f, 0.32f, 1);

		float heal = (((float)this.currActor.Stat_Level * 2 / 250) * (this.currActor.Stat_Min) 
			* this.currSkill ["Power"]) * Random.Range (0.8f, 1.1f);
		this.damageCounterSpawn.transform.GetChild(0).GetComponent<Text>().text = ((int)heal).ToString();
		this.currReceiver.GetHealed ((int)heal);
	}

	#endregion

	#region Utilities

	float ReturnTypeEffectiveness(int element)
	{
		float effectiveness = this.currReceiver.weaknessList [element];
		if (effectiveness != 1) 
		{
			this.weaknessSpawn = (GameObject)GameObject.Instantiate (this.weaknessPrefab, this.currReceiver.transform.position, Quaternion.identity);
			this.weaknessSpawn.transform.SetParent (this.canvasObject.transform);
			this.weaknessSpawn.transform.localScale = new Vector3 (0.32f, 0.32f, 1);
			if (effectiveness > 1)
			{
				this.weaknessSpawn.transform.GetChild (0).GetComponent<Text> ().text = "WEAK";
				this.willStagger = true;
			}
			if(effectiveness == 0.5f)
				this.weaknessSpawn.transform.GetChild(0).GetComponent<Text>().text = "RESIST";
			if(effectiveness == 0)
				this.weaknessSpawn.transform.GetChild(0).GetComponent<Text>().text = "IMMUNE";
				
		}
		if(currReceiver.CompareTag("Enemy"))
			AddToWeaknessDictionary(currReceiver.Prop_Name, (int)element, effectiveness);
		return effectiveness;
	}

	void AddToWeaknessDictionary(string name, int element, float effectiveness)
	{
		string effectivName = "-";
		if(effectiveness > 1)
			effectivName = "Wk";
		if(effectiveness == 0.5f)
			effectivName = "Rs";
		if(effectiveness == 0)
			effectivName = "Nu";
		if(!SavedData.current.weaknessScan.ContainsKey(name))
			SavedData.current.weaknessScan.Add(name, new Dictionary<int, string>());
		if(!SavedData.current.weaknessScan[name].ContainsKey(element))
			SavedData.current.weaknessScan[name].Add(element, effectivName);
	}

	#endregion
}
