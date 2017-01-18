using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers_Map : MonoBehaviour
{
	private DungeonSys_ObjectReferences sys_objectRef;
	public Dictionary<string, List<string>> encounterLists = new Dictionary<string, List<string>>();
	private int currPlayerPos = 1;

	#region GETS & SETS

	public int CurrPlayerPos
	{
		get{ return this.currPlayerPos;}
		set{ this.currPlayerPos = value;}
	}

	#endregion

	void Start()
	{
		this.sys_objectRef = GameObject.Find("ObjectReferences").GetComponent<DungeonSys_ObjectReferences>();
		DontDestroyOnLoad(this.gameObject);
		this.encounterLists.Add("Weak", this.sys_objectRef.BoardDesign.encounters1);
		this.encounterLists.Add("Medium", this.sys_objectRef.BoardDesign.encounters2);
		this.encounterLists.Add("Strong", this.sys_objectRef.BoardDesign.encounters3);
	} 	

	void Update()
	{
		if(this.sys_objectRef == null)
			this.sys_objectRef = GameObject.Find("ObjectReferences").GetComponent<DungeonSys_ObjectReferences>();
	}

	public string GetRandomBattle(string difficulty)
	{
		int battleGot = Random.Range(0, this.encounterLists[difficulty].Count);
		string battleName = this.encounterLists[difficulty][battleGot];
		this.encounterLists[difficulty].RemoveAt(battleGot);
		return battleName;
	}

}
