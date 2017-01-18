using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class Tile_Global : MonoBehaviour 
{
	DungeonSys_ObjectReferences sys_objReference;
	Dictionary<int, Tile_Global> tileList = new Dictionary<int, Tile_Global>();

	[SerializeField]
	private int tileID, nextTileID, prevTileID;

	[SerializeField]
	private string battleEncounter;

	#region GETS & SETS

	public int TileID
	{
		get{return this.tileID;}
		set{this.tileID = value;}
	}

	public int NextTileID
	{
		get{return this.nextTileID;}
		set{this.nextTileID = value;}
	}

	#endregion

	void Awake () 
	{
		this.sys_objReference = GameObject.Find("ObjectReferences").GetComponent<DungeonSys_ObjectReferences>();
		this.tileList = this.sys_objReference.tileList;
		this.tileList.Add(this.tileID, this.GetComponent<Tile_Global>());
	}

	public void Check()
	{
		BattleCheck();
	}

	void BattleCheck()
	{
		if(this.battleEncounter != "")
		{
			this.sys_objReference.MapManager.CurrPlayerPos = this.tileID;
			this.sys_objReference.EncounterManager.currEncounterMonsters = this.sys_objReference.EncounterManager.encounterList
			[this.sys_objReference.MapManager.GetRandomBattle(this.battleEncounter)];
			SceneManager.LoadScene("Battle");
		}
			
	}
}
