using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Characters_Enemies : Characters_Global 
{
	[SerializeField]
	protected List<int> skillChances;
	[SerializeField]
	protected Animator animator;

	protected int skillChanceNum;

	public void Entry()
	{
		base.Start ();
	}

	public override void Act()
	{
		base.Act ();

		this.animator.SetTrigger("Act");
		if (!this.temp_staggering)
		{
			int usedSkill = this.ReturnSkill ();
			this.ActionType = "Attack";
			this.UsedAction = this.skillList [usedSkill];
			while (this.TargetCharacter == null || (this.TargetCharacter.GetComponent<Characters_Allies> () != null && this.TargetCharacter.GetComponent<Characters_Allies> ().Fainted))
				this.GetTarget (this.sys_references.SkillManager.skillList [this.skillList [usedSkill]]);
			StartCoroutine(this.sys_references.Sys_TurnActions.BattleRoutine(this.GetComponent<Characters_Enemies>(), this.TargetCharacter));
		}
		else 
		{
			StaggerAction ();
		}
	}

	public override void GetStats(string name)
	{
		Dictionary<string, int> monsterStats = new Dictionary<string, int>();
		monsterStats = this.sys_references.MonsterManager.MonsterProperties[name];
		this.prop_name = name;
		this.stat_level = monsterStats["Level"];
		this.stat_hp = monsterStats["HP"];
		this.stat_str = monsterStats["Str"];
		this.stat_mag = monsterStats["Mag"];
		this.stat_def = monsterStats["Def"];
		this.stat_min = monsterStats["Mnd"];
		this.stat_agi = monsterStats["Agi"];
		this.stat_lu = monsterStats["Luc"];
		this.stat_stagger = monsterStats["Stag"];
		this.temp_currHp = this.stat_hp;
		this.weaknessList = this.sys_references.MonsterManager.MonsterWeaknessess[name];
		this.skillList = this.sys_references.MonsterManager.MonsterSkills[name];
		this.skillChances = this.sys_references.MonsterManager.MonsterSkillPriorities[name];
		CreateSkillPool ();
		UpdateStaggerMarkers ();
	}

	protected override void StaggerAction()
	{
		base.StaggerAction ();
		StartCoroutine(this.sys_references.Sys_TurnActions.BattleRoutine(this.GetComponent<Characters_Enemies>(), this.TargetCharacter));
	}

	protected void CreateSkillPool()
	{
		for (int i = 0; i < this.skillChances.Count; i++) 
		{
			this.skillChanceNum += this.skillChances[i];
		}
	}

	protected int ReturnSkill()
	{
		int skillGot = Random.Range (1, this.skillChanceNum + 1);
		int numStack = 0;
		for (int i = 0; i < this.skillChances.Count; i++)
		{
			numStack += this.skillChances [i];
			if(skillGot <= numStack)
				return i;
		}
		return 0;
	}

	protected void GetTarget(Dictionary<string, float> skillProperties)
	{
		switch ((int)skillProperties ["TargetParty"])
		{
			case 1:
				this.TargetCharacter = this.sys_references.Sys_Master.allyList[Random.Range(0, 	
                this.sys_references.Sys_Master.allyList.Count)];
			break;
			case 0:
				this.TargetCharacter = this.sys_references.Sys_Master.enemyList[Random.Range(0, 	
				this.sys_references.Sys_Master.enemyList.Count)];
			break;
		}
	}
}