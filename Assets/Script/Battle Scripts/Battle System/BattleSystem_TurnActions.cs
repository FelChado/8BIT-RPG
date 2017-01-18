using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BattleSystem_TurnActions : MonoBehaviour {

	[SerializeField]
	Canvas canvas;
	[SerializeField]
	BattleSystem_References battleRef;
	[SerializeField]
	GameObject allyList, enemyList, allyPrefab, enemyPrefab;

	private bool sys_battleStop;
	private int sys_currTurn = 0;

	public bool Sys_BattleStop
	{
		get{ return this.sys_battleStop;}
		set{ this.sys_battleStop = value;}
	}

	public void SetBattleOrder()
	{
		for (int i = 0; i < 3; i++)
		{
			GameObject allySpawn = (GameObject)GameObject.Instantiate (this.allyPrefab, this.allyList.transform.position, Quaternion.identity);
			allySpawn.transform.SetParent (this.allyList.transform);
			allySpawn.transform.localScale = new Vector3 (1, 1, 1);
			allySpawn.transform.Translate (200 * this.canvas.scaleFactor * i, 0 , 0);
			allySpawn.GetComponent<Characters_Allies> ().Entry ();
			allySpawn.GetComponent<Characters_Allies> ().GetStats (SavedData.current.currentParty [i]);
			this.battleRef.Sys_Master.battleOrder.Add(allySpawn.GetComponent<Characters_Global>());
			this.battleRef.Sys_Master.allyList.Add(allySpawn.GetComponent<Characters_Allies>());
		}
		for(int i = 0; i < battleRef.EncounterManager.currEncounterMonsters.Count; i++)
		{
			GameObject enemySpawn = (GameObject)GameObject.Instantiate (this.enemyPrefab, this.enemyList.transform.position, Quaternion.identity);
			enemySpawn.transform.SetParent (this.enemyList.transform);
			enemySpawn.transform.localScale = new Vector3 (1, 1, 1);
			enemySpawn.transform.Translate (-200 * this.canvas.scaleFactor * i, 0 , 0);
			enemySpawn.GetComponent<Characters_Enemies> ().Entry ();
			enemySpawn.GetComponent<Characters_Enemies> ().GetStats (battleRef.EncounterManager.currEncounterMonsters[i]);
			this.battleRef.Sys_Master.battleOrder.Add(enemySpawn.GetComponent<Characters_Global>());
			this.battleRef.Sys_Master.enemyList.Add(enemySpawn.GetComponent<Characters_Enemies>());
		}

		battleRef.Sys_Master.battleOrder.Sort(delegate(Characters_Global a, Characters_Global b) { return b.Stat_Agi.CompareTo(a.Stat_Agi);});
	}

	public void NextTurn()
	{
		if (sys_currTurn < this.battleRef.Sys_Master.battleOrder.Count - 1)
			sys_currTurn ++;
		else
			sys_currTurn = 0;
		this.battleRef.Sys_Master.battleOrder [this.sys_currTurn].Act ();
	}

	void CheckFainted()
	{
		for(int i = 0; i < this.battleRef.Sys_Master.allyList.Count; i++)
		{
			if (this.battleRef.Sys_Master.allyList[i].Temp_currHp <= 0)
			{
				this.battleRef.Sys_Master.allyList[i].Fainted = true;
				this.battleRef.Sys_Master.allyList[i].staggerReceived.Clear();
				this.battleRef.Sys_Master.allyList[i].Temp_Staggering = false;
				this.battleRef.Sys_Master.allyList[i].UpdateStaggerMarkers();
			}
		}

		for(int i = 0; i < this.battleRef.Sys_Master.enemyList.Count; i++)
		{
			if (this.battleRef.Sys_Master.enemyList[i].Temp_currHp <= 0) 
			{
				Destroy (this.battleRef.Sys_Master.enemyList[i].gameObject);
				this.battleRef.Sys_Master.battleOrder.Remove (this.battleRef.Sys_Master.enemyList[i]);
				this.battleRef.Sys_Master.enemyList.Remove (this.battleRef.Sys_Master.enemyList[i]);
			}
		}
	}

	bool CheckVictory()
	{
		if(this.battleRef.Sys_Master.allyList.Count == 0)
		{
			this.battleRef.SkillNameAnimator.transform.GetChild(0).GetComponent<Text>().text = "The party was defeated.";
			this.battleRef.SkillNameAnimator.SetTrigger("On");
			return true;
		}
		else if(this.battleRef.Sys_Master.enemyList.Count == 0)
		{
			this.battleRef.SkillNameAnimator.transform.GetChild(0).GetComponent<Text>().text = "Victory";
			this.battleRef.SkillNameAnimator.SetTrigger("On");
			SceneManager.LoadScene("Board_01");
			return true;
		}
		else
			return false;
	}

	public IEnumerator BattleRoutine(Characters_Global actor, Characters_Global receiver)
	{
		this.sys_battleStop = true;
		this.battleRef.SkillName.text = actor.UsedAction;
		this.battleRef.SkillNameAnimator.SetTrigger ("On");
		this.battleRef.Sys_ExecActions.ExecuteActions (actor, receiver);
		CheckFainted ();
		while(this.sys_battleStop)
			yield return new WaitForSeconds (0.1f);
		if(!CheckVictory())
			NextTurn ();

	}

}
