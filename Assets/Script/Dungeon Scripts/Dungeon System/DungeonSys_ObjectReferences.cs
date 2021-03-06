﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonSys_ObjectReferences : MonoBehaviour 
{
	[SerializeField]
	GameObject encounterManagerPrefab, mapManagerPrefab = null;
	Managers_EncounterManager encounterManager;
	Managers_Map mapManager;

	public Dictionary<int, Tile_Global> tileList = new Dictionary<int, Tile_Global>();

	[SerializeField]
	Managers_BoardDesign boardDesign;
	[SerializeField]
	DungeonSys_TurnActions sys_turnActions;
	[SerializeField]
	DungeonSys_Interface sys_interface;
	[SerializeField]
	MainChar_Dungeon mainCharacter;
	[SerializeField]
	DungeonInterface_Menu dungeonMenu;


	void Awake()
	{
		if(GameObject.FindGameObjectWithTag("EncounterManager") == null)
		{
			this.encounterManager = GameObject.Instantiate((GameObject)this.encounterManagerPrefab, new Vector2(0,0),
			Quaternion.identity).GetComponent<Managers_EncounterManager>();
			this.encounterManager.name = "EncounterManager";
		}
		if(this.encounterManager == null)
			this.encounterManager = GameObject.FindGameObjectWithTag("EncounterManager").GetComponent<Managers_EncounterManager>();

		if(GameObject.FindGameObjectWithTag("MapManager") == null)
		{
			this.mapManager = GameObject.Instantiate((GameObject)this.mapManagerPrefab, new Vector2(0,0),
			Quaternion.identity).GetComponent<Managers_Map>();
			this.mapManager.name = "MapManager";
		}
		if(this.mapManager == null)
			this.mapManager = GameObject.FindGameObjectWithTag("MapManager").GetComponent<Managers_Map>();
		this.mapManager.Sys_ObjectRef = this.GetComponent<DungeonSys_ObjectReferences>();
	}

	void Start()
	{
		this.mapManager.ReloadBattles();
	}

	#region GETS & SETS

	public DungeonSys_TurnActions Sys_TurnActions
	{
		get{return this.sys_turnActions;}
		set{this.sys_turnActions = value;}
	}

	public MainChar_Dungeon MainCharacter
	{
		get{return this.mainCharacter;}
		set{this.mainCharacter = value;}
	}

	public Managers_EncounterManager EncounterManager
	{
		get{return this.encounterManager;}
		set{this.encounterManager = value;}
	}

	public DungeonSys_Interface Sys_interface
	{
		get{return this.sys_interface;}
		set{this.sys_interface = value;}
	}

	public Managers_Map MapManager
	{
		get{return this.mapManager;}
		set{this.mapManager = value;}
	}

	public Managers_BoardDesign BoardDesign
	{
		get{return this.boardDesign;}
		set{this.boardDesign = value;}
	}

	public DungeonInterface_Menu DungeonMenu
	{
		get{return this.dungeonMenu;}
		set{this.dungeonMenu = value;}
	}

	#endregion
}
