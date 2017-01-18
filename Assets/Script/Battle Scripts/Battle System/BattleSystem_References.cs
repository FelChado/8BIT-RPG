using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BattleSystem_References : MonoBehaviour {

	[SerializeField]
	private BattleSystem_Master sys_master;
	[SerializeField]
	private BattleSystem_TurnActions sys_turnActions;
	[SerializeField]
	private BattleSystem_Interface sys_interface;
	[SerializeField]
	private BattleSystem_ExecuteActions sys_execActions;
	[SerializeField]
	private BattleSystem_TempData sys_tempData;
	[SerializeField]
	private Managers_Skills skillManager;
	[SerializeField]
	private Managers_Characters charManager;
	[SerializeField]
	private Managers_Monsters monsterManager;
	[SerializeField]
	private Animator skillNameAnimator;
	[SerializeField]
	private Text skillName;

	Managers_EncounterManager encounterManager;

	void Awake()
	{
		this.encounterManager = GameObject.Find("EncounterManager").GetComponent<Managers_EncounterManager>();
	}

	#region GETS & SETS
	
	public BattleSystem_Master Sys_Master
	{
		get{return this.sys_master;}
		set{this.sys_master = value;}
	}

	public BattleSystem_TurnActions Sys_TurnActions
	{
		get{return this.sys_turnActions;}
		set{this.sys_turnActions = value;}
	}
	
	public BattleSystem_Interface Sys_Interface
	{
		get{return this.sys_interface;}
		set{this.sys_interface = value;}
	}

	public BattleSystem_ExecuteActions Sys_ExecActions
	{
		get{return this.sys_execActions;}
		set{this.sys_execActions = value;}
	}

	public BattleSystem_TempData Sys_TempData
	{
		get{return this.sys_tempData;}
		set{this.sys_tempData = value;}
	}

	public Managers_Skills SkillManager
	{
		get{return this.skillManager;}
		set{this.skillManager = value;}
	}

	public Managers_Characters CharManager
	{
		get{return this.charManager;}
		set{this.charManager = value;}
	}

	public Managers_Monsters MonsterManager
	{
		get{return this.monsterManager;}
		set{this.monsterManager = value;}
	}

	public Managers_EncounterManager EncounterManager
	{
		get{return this.encounterManager;}
		set{this.encounterManager = value;}
	}

	public Animator SkillNameAnimator
	{
		get{return this.skillNameAnimator;}
		set{this.skillNameAnimator = value;}
	}

	public Text SkillName
	{
		get{return this.skillName;}
		set{this.skillName = value;}
	}

	#endregion
}
