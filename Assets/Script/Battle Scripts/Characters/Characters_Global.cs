using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public abstract class Characters_Global : MonoBehaviour 
{
	// Object References
	[SerializeField]
	protected BattleSystem_References sys_references;
	[SerializeField]
	protected Text staggerText;
	[SerializeField]
	protected Image gauge_hp;

	// Skills and Basic Attack
	[SerializeField]
	public List<string> skillList = new List<string>();
	[SerializeField]
	public List<float> weaknessList = new List<float> ();
	[SerializeField]
	protected string basicAttack;

	// Status Attributes
	[SerializeField]
	protected int stat_hp, stat_level, stat_stagger, stat_stamina;
	[SerializeField]
	protected float stat_str, stat_def, stat_mag, stat_min, stat_agi, stat_lu;
	protected int buff_atk, buff_def, buff_agi;

	// Temporary Values
	public List<int> staggerReceived = new List<int>();
	[SerializeField]
	List<string> statPenalties = new List<string>();
	protected bool temp_staggering;
	protected int temp_currHp;	

	// Battle Informations
	private Characters_Global targetCharacter;
	[SerializeField]
	protected string prop_name;
	private string actionType, usedAction;
	
	#region GETS & SETS

	public bool Temp_Staggering
	{
		get{ return this.temp_staggering; }
		set{ this.temp_staggering = value;}
	}

	public int Stat_Level
	{
		get{ return this.stat_level; }
		set{ this.stat_level = value;}
	}

	public float Stat_Str
	{
		get{ return this.stat_str; }
		set{ this.stat_str = value;}
	}

	public float Stat_Def
	{
		get{ return this.stat_def; }
		set{ this.stat_def = value;}
	}
	
	public float Stat_Mag
	{
		get{ return this.stat_mag; }
		set{ this.stat_mag = value;}
	}

	public float Stat_Min
	{
		get{ return this.stat_min; }
		set{ this.stat_min = value;}
	}

	public float Stat_Agi
	{
		get{ return this.stat_agi; }
		set{ this.stat_agi = value;}
	}

	public int Temp_currHp
	{
		get{ return this.temp_currHp; }
		set{ this.temp_currHp = value;}
	}

	public string ActionType
	{
		get{ return this.actionType; }
		set{ this.actionType = value;}
	}
	
	public string UsedAction
	{
		get{ return this.usedAction; }
		set{ this.usedAction = value;}
	}
	
	public Characters_Global TargetCharacter
	{
		get{ return this.targetCharacter; }
		set{ this.targetCharacter = value;}
	}

	public string BasicAttack
	{
		get{ return this.basicAttack; }
		set{ this.basicAttack = value;}
	}

	public string Prop_Name
	{
		get{ return this.prop_name; }
		set{ this.prop_name = value;}
	}

	public int Buff_Atk
	{
		get{ return this.buff_atk; }
		set{ this.buff_atk = value;}
	}

	public int Buff_Def
	{
		get{ return this.buff_def; }
		set{ this.buff_def = value;}
	}

	public int Buff_Agi
	{
		get{ return this.buff_agi; }
		set{ this.buff_agi = value;}
	}

	#endregion

	public virtual void Act()
	{
		RecoverStagger ();
		StatusPenaltiesEffects();
	}

	protected void Start()
	{
		this.sys_references = GameObject.FindGameObjectWithTag ("References").GetComponent<BattleSystem_References>();
	}

	public void CallNextTurn()
	{
		sys_references.Sys_Interface.CurrentActor = null;
		sys_references.Sys_TurnActions.NextTurn ();
	}

	public virtual void UseAction()
	{

	}

	public abstract void GetStats (string name);

	public void GetDamaged(float damage, int stagger, bool willStagger)
	{
		if (!this.temp_staggering)
		{
			for (int i = 0; i < stagger; i++)
				if (this.staggerReceived.Count < this.stat_stagger)
					this.staggerReceived.Add (stagger);
		} 
		else
		{
			this.temp_staggering = false;
			StaggerIndicaction(false);
			this.staggerReceived.Clear ();
		}

		this.temp_currHp -= (int)damage;
		if (this.temp_currHp < 0)
			this.temp_currHp = 0;

		CompareStagger (willStagger);
		UpdateStaggerMarkers ();
		UpdateUI ();
	}

	public void GetHealed(float heal)
	{
		this.temp_currHp += (int)heal;
		if (this.temp_currHp > this.stat_hp)
			this.temp_currHp = this.stat_hp;
		UpdateUI ();
	}

	public void GetBuffed(int attrib, int side)
	{
		switch(attrib)
		{
			case 1:
				this.buff_atk += side;
			break;
			case 2:
				this.buff_def += side;
			break;
			case 3:
				this.buff_agi += side;
			break;
		}
		this.buff_atk = Mathf.Clamp(this.buff_atk, -1, 1);
		this.buff_def = Mathf.Clamp(this.buff_def, -1, 1);
		this.buff_agi = Mathf.Clamp(this.buff_agi, -1, 1);
	}

	public IEnumerator StatusPenaltiesEffects()
	{
		for(int i = 0; i < this.statPenalties.Count; i++)
		{
			switch(statPenalties[i])
			{
				case "Poison":
					yield return new WaitForSeconds(0.1f);
					this.sys_references.HUD_PopUps.SpawnDamageCounter(this.stat_hp/10, this.transform, "Damage");
					this.sys_references.HUD_PopUps.SpawnWord("Poison", this.transform);
					this.temp_currHp -= this.stat_hp/10;
				break;
				case "Strain":

				break;
				case "Bleed":

				break;

			}
		}
	}

	public virtual void UpdateUI()
	{
		this.gauge_hp.fillAmount = (float)this.temp_currHp / (float)this.stat_hp;
	}

	protected void CompareStagger(bool willStagger)
	{
		if (this.staggerReceived.Count == this.stat_stagger || willStagger)
		{
			this.temp_staggering = true;
			StaggerIndicaction(true);
		}
	}

	protected virtual void StaggerAction()
	{
		this.sys_references.Sys_Interface.Active = false;
		this.ActionType = "Stagger";
		this.UsedAction = "Staggering";
	}

	public void UpdateStaggerMarkers()
	{
		Transform staggerList = this.transform.FindChild("Stagger").transform;
		for (int i = 0; i < 6; i++) 
		{
			if (i < this.stat_stagger) 
			{
				if(this.staggerReceived.Count > i)
					staggerList.GetChild (i).GetComponent<Animator> ().SetInteger ("Type", this.staggerReceived [i]);
				else
					staggerList.GetChild (i).GetComponent<Animator> ().SetInteger ("Type", 0);
			}
			else
				staggerList.GetChild (i).GetComponent<Animator> ().SetInteger ("Type", -1);
		}
	}

	protected void RecoverStagger()
	{
		if (this.staggerReceived.Count > 0) 
		{
			bool finished = false;
			if (this.staggerReceived [staggerReceived.Count - 1] == 1) 
			{
				this.staggerReceived.RemoveAt (staggerReceived.Count - 1);
				finished = true;
			}
			while (!finished && this.staggerReceived.Count > 0 && this.staggerReceived [staggerReceived.Count - 1] == 2)
			{
				this.staggerReceived.RemoveAt (staggerReceived.Count - 1);
			}
		} 

		if(this.staggerReceived.Count == 0)
		{
			this.temp_staggering = false;
			StaggerIndicaction(false);
		}

		UpdateStaggerMarkers ();

	}

	protected void StaggerIndicaction(bool active)
	{
		this.staggerText.enabled = active;
	}
}
