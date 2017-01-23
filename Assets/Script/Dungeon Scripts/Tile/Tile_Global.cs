using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class Tile_Global : MonoBehaviour 
{
	Animator tileAnimator;
	DungeonSys_ObjectReferences sys_objReference;

	Dictionary<int, Tile_Global> tileList = new Dictionary<int, Tile_Global>();

	[SerializeField]
	private int tileID, prevTileID;
	[SerializeField]
	private List<int> nextTiles;
	[SerializeField]
	private int battleEncounter;

	#region GETS & SETS

	public int TileID
	{
		get{return this.tileID;}
		set{this.tileID = value;}
	}

	public int BattleEncounter
	{
		get{return this.battleEncounter;}
		set{this.battleEncounter = value;}
	}

	public List<int> NextTiles
	{
		get{return this.nextTiles;}
		set{this.nextTiles = value;}
	}

	#endregion

	void Awake () 
	{
		this.sys_objReference = GameObject.Find("ObjectReferences").GetComponent<DungeonSys_ObjectReferences>();
		this.tileList = this.sys_objReference.tileList;
		this.tileList.Add(this.tileID, this.GetComponent<Tile_Global>());
		this.tileAnimator = this.GetComponent<Animator>();
		SetBattleAnimation();
	}

	public void Check(int steps)
	{
		if(steps == 0)
			BattleCheck();
	}

	void BattleCheck()
	{
		if(this.battleEncounter != 0)
		{
			this.sys_objReference.MapManager.CurrPlayerPos = this.tileID;
			this.sys_objReference.EncounterManager.currEncounterMonsters = this.sys_objReference.EncounterManager.encounterList
				[this.sys_objReference.MapManager.GetRandomBattle(this.battleEncounter)];
			this.sys_objReference.MapManager.battlePositions.Remove(this.TileID);
			SceneManager.LoadScene("Battle");
		}
			
	}

	public void SetBattleAnimation()
	{
		if(this.battleEncounter != 0)
		{
			this.tileAnimator.SetInteger("Type", this.battleEncounter);
		}
	}
}
