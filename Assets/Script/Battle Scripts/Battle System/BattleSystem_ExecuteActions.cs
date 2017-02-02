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
			case 3:
				Buff();
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
		// Hit or Miss
		if (Random.Range (0, 100) < this.currSkill ["Acc"] * ((this.currActor.Stat_Agi + (0.3f * this.currActor.Buff_Agi) * 0.8f) / 
			(this.currReceiver.Stat_Agi * (0.3f * this.currReceiver.Buff_Agi) * 0.8f))) 
			{
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
			* this.currSkill ["Power"]) * (1 + 0.2f * this.currActor.Buff_Atk) * (1 + 0.2f * -this.currActor.Buff_Def) *
			Random.Range (0.8f, 1.1f);
			if (damage < 0)
				damage = 0;
			else if (Random.Range (0, 100) < this.currSkill ["Crit"])
				damage *= 1.5f;
			damage *= ReturnTypeEffectiveness ((int)this.currSkill ["Element"]);
			this.battleRef.HUD_PopUps.SpawnDamageCounter(damage, this.currReceiver.transform, "Damage");
			this.currReceiver.GetDamaged ((int)damage, (int)this.currSkill ["Type"], willStagger);
		} 
		else 
			this.battleRef.HUD_PopUps.SpawnWord("Miss", this.currReceiver.transform);

	}

	void Heal()
	{
		float heal = (((float)this.currActor.Stat_Level * 2 / 250) * (this.currActor.Stat_Min) 
					 * this.currSkill ["Power"]) * Random.Range (0.8f, 1.1f);
		this.battleRef.HUD_PopUps.SpawnDamageCounter(heal, this.currReceiver.transform, "Heal");
		this.currReceiver.GetHealed ((int)heal);
	}

	void Buff()
	{
		string buffText = "";
		switch((int)this.currSkill["Attribute"])
		{
			case 1:
				buffText = "Attack Up";
			break;
			case 2:
				buffText = "Defense Up";
			break;
			case 3:
				buffText = "Agility Up";
			break;
		}
		this.battleRef.HUD_PopUps.SpawnWord(buffText, this.currReceiver.transform);
		this.currReceiver.GetBuffed((int)this.currSkill["Attribute"], 1);
	}

	#endregion

	#region Utilities

	float ReturnTypeEffectiveness(int element)
	{
		float effectiveness = this.currReceiver.weaknessList [element];
		if (effectiveness != 1) 
		{
			string effectName = null;
			if (effectiveness > 1)
			{
				effectName = "WEAK";
				this.willStagger = true;
			}
			if(effectiveness == 0.5f)
				effectName = "RESIST";
			if(effectiveness == 0)
				effectName = "IMMUNE";
			this.battleRef.HUD_PopUps.SpawnWord(effectName, this.currReceiver.transform);
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
			print(effectivName);
	}

	#endregion
}
