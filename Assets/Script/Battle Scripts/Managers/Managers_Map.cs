using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers_Map : MonoBehaviour
{
	private DungeonSys_ObjectReferences sys_objectRef;
	public Dictionary<int, List<string>> encounterLists = new Dictionary<int, List<string>>();
	public Dictionary<int, int> battlePositions = new Dictionary<int, int>();
	private int currPlayerPos = 1;

	#region GETS & SETS

	public DungeonSys_ObjectReferences Sys_ObjectRef
	{
		get{ return this.sys_objectRef;}
		set{ this.sys_objectRef = value;}
	}

	public int CurrPlayerPos
	{
		get{ return this.currPlayerPos;}
		set{ this.currPlayerPos = value;}
	}

	#endregion

	void Start()
	{
		DontDestroyOnLoad(this.gameObject);
		this.encounterLists.Add(1, this.sys_objectRef.BoardDesign.encounters1);
		this.encounterLists.Add(2, this.sys_objectRef.BoardDesign.encounters2);
		this.encounterLists.Add(3, this.sys_objectRef.BoardDesign.encounters3);
		SpreadBattles();
	} 	

	void Update()
	{
		if(this.sys_objectRef == null)
			this.sys_objectRef = GameObject.Find("ObjectReferences").GetComponent<DungeonSys_ObjectReferences>();
	}

	void SpreadBattles()
	{
		for(int i = 1; i < 4; i++)
		{
			for(int j = 0; j < this.encounterLists[i].Count; j++)
			{
				int randomTileGot = 0;
				while(randomTileGot == 0 || this.battlePositions.ContainsKey(randomTileGot))
					randomTileGot = Random.Range(1, this.sys_objectRef.tileList.Count);
				this.battlePositions.Add(randomTileGot, i);
				this.sys_objectRef.tileList[randomTileGot].BattleEncounter = i;
				this.sys_objectRef.tileList[randomTileGot].SetBattleAnimation();
			}
		}
	}

	public void ReloadBattles()
	{
		foreach(KeyValuePair<int, int> battlePos in this.battlePositions)
		{
			this.sys_objectRef.tileList[battlePos.Key].BattleEncounter = battlePos.Value;
			this.sys_objectRef.tileList[battlePos.Key].SetBattleAnimation();
		}
	}

	public string GetRandomBattle(int difficulty)
	{
		int battleGot = Random.Range(0, this.encounterLists[difficulty].Count);
		string battleName = this.encounterLists[difficulty][battleGot];
		this.encounterLists[difficulty].RemoveAt(battleGot);
		return battleName;
	}
}
