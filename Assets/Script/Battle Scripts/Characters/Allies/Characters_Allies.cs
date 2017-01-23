using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Characters_Allies : Characters_Global
{
	Canvas canvas;
	Managers_Characters characterManager;
	[SerializeField]
	Image gauge_stam;
	[SerializeField]
	Text text_name = null, text_hp = null, text_stam = null;

	int temp_currStam;
	string switchedCharacter;

	private bool fainted;

	#region GETS & SETS

	public bool Fainted
	{
		get{ return this.fainted;}
		set{ this.fainted = value;}
	}

	public string SwitchedCharacter
	{
		get{ return this.switchedCharacter;}
		set{ this.switchedCharacter = value;}
	}

	public int Temp_CurrStam
	{
		get{ return this.temp_currStam;}
		set{ this.temp_currStam = value;}
	}

	#endregion

	public void Entry()
	{
		base.Start ();
		this.characterManager = GameObject.FindGameObjectWithTag ("Managers").GetComponent<Managers_Characters> ();
		this.canvas = GameObject.Find ("Canvas").GetComponent<Canvas> ();
	}

	public override void Act()
	{
		base.Act ();

		if (!this.Fainted) 
		{
			this.transform.Translate (new Vector2 (0, 10 * this.canvas.scaleFactor));
			this.sys_references.Sys_Interface.CurrentActor = this.GetComponent<Characters_Allies> ();
			if (!this.temp_staggering) 
				this.sys_references.Sys_Interface.Active = true;
		} 
		else
			this.CallNextTurn ();
		
		if (this.temp_staggering) 
		{
			StaggerAction ();
		}
	}

	public override void GetStats(string name)
	{
		this.prop_name = name;
		this.basicAttack = this.characterManager.characterBasicAttacks [name];
		this.weaknessList = this.characterManager.characterWeaknesses [this.prop_name];
		Dictionary<string, int> charStats = this.characterManager.characterBaseStats[name];
		this.stat_level = SavedData.current.partyLevels [name];
		this.skillList = SavedData.current.partySkills [name];
		this.stat_hp = StatFormula (this.stat_level, charStats["Hp"]) + this.stat_level;
		this.stat_stamina = StatFormula (this.stat_level, charStats["Stam"]) + this.stat_level;
		this.temp_currStam = this.stat_stamina;
		RetrieveTempData();
		this.stat_stagger = charStats["Stag"];
		this.stat_str = StatFormula (this.stat_level, charStats["Str"]);
		this.stat_mag = StatFormula (this.stat_level, charStats["Mag"]);
		this.stat_def = StatFormula (this.stat_level, charStats["Def"]);
		this.stat_min = StatFormula (this.stat_level, charStats["Mnd"]);
		this.stat_agi = StatFormula (this.stat_level, charStats["Agi"]);
		this.stat_lu = StatFormula (this.stat_level, charStats["Luc"]);
		this.text_name.text = this.prop_name;
		UpdateUI ();
		UpdateStaggerMarkers ();
	}

	protected override void StaggerAction()
	{
		base.StaggerAction ();
		this.sys_references.Sys_Interface.CallBattleRoutine ();
	}

	void RetrieveTempData()
	{
		if (this.sys_references.Sys_TempData.currCharHp.ContainsKey (this.prop_name))
		{
			this.temp_currHp = this.sys_references.Sys_TempData.currCharHp [this.prop_name];
			if(this.sys_references.Sys_TempData.currCharStagger.ContainsKey(this.prop_name))
				this.staggerReceived = this.sys_references.Sys_TempData.currCharStagger [this.prop_name];
		}
		else
		{
			this.temp_currHp = this.stat_hp;
			WriteTempData();
			this.staggerReceived = new List<int>();
			print(this.sys_references.Sys_TempData.currCharHp[this.Prop_Name]);
		}
	}

	public void WriteTempData()
	{
		this.sys_references.Sys_TempData.WriteCharData (this.prop_name, this.temp_currHp, this.staggerReceived);
	}

	public int StatFormula(int level, int baseStat)
	{
		int result = (int)(((float)baseStat * 2.2f * (float)level) / 100 + 5);
		return result;
	}

	public override void UseAction()
	{
		this.transform.Translate (new Vector2 (0, -10 * this.canvas.scaleFactor));
	}

	public override void UpdateUI()
	{
		base.UpdateUI ();
		this.text_hp.text = this.temp_currHp + "/" + this.stat_hp;
		this.text_stam.text = this.temp_currStam + "/" + this.stat_stamina;
		this.gauge_stam.fillAmount = (float)this.temp_currStam / (float)this.stat_stamina;
		UpdateStaggerMarkers ();
	}


}
