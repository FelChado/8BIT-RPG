using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainChar_Dungeon : MonoBehaviour 
{
	[SerializeField]
	DungeonSys_ObjectReferences sys_objReferences;

	Vector2 currTile, nextTile;

	[SerializeField]
	private int currTileID;

	private int nextTileID, stepsRemaining;

	private float walkJourney;

	#region GETS & SETS

	public int CurrTileID
	{
		get{return this.currTileID;}
		set{this.currTileID = value;}
	}

 	#endregion

 	void Start()
 	{
		this.CurrTileID = this.sys_objReferences.MapManager.CurrPlayerPos;
		this.transform.position = this.sys_objReferences.tileList[currTileID].transform.position;
 	}

	public void CallAction(int steps)
	{
		this.walkJourney = 0;
 		this.stepsRemaining = steps;
		CallNextAction();
 	}

 	void Update()
 	{
 		if(this.walkJourney != 0)
			Walk();
 	}

 	void CallNextAction()
 	{
		this.sys_objReferences.tileList[this.currTileID].Check();
 		if(stepsRemaining > 0)
 		{
 			CallMovement();
 			stepsRemaining--;
 		}
 	}

 	void CallMovement()
 	{
 		this.walkJourney = 0;
		this.currTile = this.sys_objReferences.tileList[this.currTileID].transform.position;
		this.nextTileID = this.sys_objReferences.tileList[this.currTileID].NextTileID;
		this.nextTile = this.sys_objReferences.tileList[nextTileID].transform.position;
		StartCoroutine(Journey());
 	}

 	void Walk()
 	{
 		if(this.currTileID != this.nextTileID)
	 		this.transform.position = Vector2.Lerp(currTile, nextTile, walkJourney);
	 	else
	 		CallNextAction();
 	}

 	IEnumerator Journey()
 	{
 		if(walkJourney < 1)
		{
	 		yield return new WaitForSeconds(0.01f);
			this.walkJourney += 0.03f * 60 * Time.deltaTime;
			StartCoroutine(Journey());
		}
		else
		{
			this.walkJourney = 1;
	 		this.currTileID = nextTileID;
		}
 	}

}
