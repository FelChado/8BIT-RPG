using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainChar_Dungeon : MonoBehaviour 
{
	[SerializeField]
	DungeonSys_ObjectReferences sys_objReferences;

	List<int> pathList = new List<int>();

	Vector2 currTile, nextTile;

	private bool selectingPath;

	[SerializeField]
	private int currTileID;

	private int nextTileID, stepsRemaining, pathCursor = 0;

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
		this.pathCursor = 0;
		this.walkJourney = 0;
 		this.stepsRemaining = steps;
		CallNextAction();
 	}

 	void Update()
 	{
 		if(this.walkJourney != 0)
			Walk();
		if(this.selectingPath)
			SelectingPath();
 	}

 	void CallNextAction()
 	{
		this.sys_objReferences.tileList[this.currTileID].Check(this.stepsRemaining);
 		if(stepsRemaining > 0)
 		{
 			CallMovement();
 			stepsRemaining--;
		}
		else
			this.sys_objReferences.Sys_TurnActions.EndTurn();
 	}

 	void CallMovement()
 	{
 		this.walkJourney = 0;
		this.currTile = this.sys_objReferences.tileList[this.currTileID].transform.position;
		this.pathList = this.sys_objReferences.tileList[this.currTileID].NextTiles;
		if(this.pathList.Count == 1)
		{
			this.nextTileID = this.sys_objReferences.tileList[this.currTileID].NextTiles[0];
			ExecuteWalk();
		}
		else
		{
			SetSelectedEffect(true);
			this.selectingPath = true;
			this.pathCursor = 0;
		}
 	}

 	void Walk()
 	{
 		if(this.currTileID != this.nextTileID)
	 		this.transform.position = Vector2.Lerp(currTile, nextTile, walkJourney);
	 	else
	 		CallNextAction();
 	}

 	void SetSelectedEffect(bool active)
 	{
		this.sys_objReferences.tileList[this.pathList[this.pathCursor]].GetComponent<Animator>().SetBool("Selected", active);
 	}

 	void SelectingPath()
 	{
 		if(Input.GetButtonDown("Vertical"))
		{
			SetSelectedEffect(false);
			this.pathCursor -= (int)Input.GetAxisRaw("Vertical");
 			if(this.pathCursor < 0)
 				this.pathCursor = 0;
 			if(this.pathCursor >= this.pathList.Count)
				this.pathCursor = this.pathList.Count - 1;
			SetSelectedEffect(true);
 		}
 		if(Input.GetButtonDown("Submit"))
		{	
			SetSelectedEffect(false);
 			this.nextTileID = this.pathList[this.pathCursor];
 			ExecuteWalk ();
 		}
 	}

 	void ExecuteWalk()
 	{
		this.nextTile = this.sys_objReferences.tileList[nextTileID].transform.position;
		StartCoroutine(Journey());
		this.selectingPath = false;
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
