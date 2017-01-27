using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleSystem_Master : MonoBehaviour
{
	// Battle Reference
	[SerializeField]
	BattleSystem_References battleRef;

	// Character Lists
	public List<Characters_Global> battleOrder = new List<Characters_Global>();
	public List<Characters_Allies> allyList = new List<Characters_Allies>();
	public List<Characters_Enemies> enemyList = new List<Characters_Enemies>();

	void Start()
	{
		this.battleRef.MonsterManager.CreateMonsterDatabase();
		this.battleRef.Sys_TurnActions.SetBattleOrder ();
		this.battleRef.Sys_Master.battleOrder [0].Act ();
	}
}
