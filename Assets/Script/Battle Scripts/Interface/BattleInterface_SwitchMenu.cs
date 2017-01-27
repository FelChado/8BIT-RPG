using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BattleInterface_SwitchMenu : MonoBehaviour 
{
	[SerializeField]
	BattleSystem_References battleRef;

	[SerializeField]
	Animator animator;

	List<string> partyBench = new List<string>();

	int cursor = 0;

	void Start()
	{
		for (int i = 3; i < SavedData.current.currentParty.Count; i++)
			this.partyBench.Add (SavedData.current.currentParty [i]);
		for(int i = this.partyBench.Count; i < 3; i++)
		{
			this.transform.GetChild(2 + i).GetChild(0).gameObject.SetActive(false);
			this.transform.GetChild(2 + i).GetChild(1).gameObject.SetActive(false);
		}
	}

	// Update is called once per frame
	void Update () 
	{
		if (this.animator.GetInteger ("State") == 1) 
		{
			MoveCursor ();
			ChooseCharacter ();
		}
	}

	public void Activate()
	{
		for (int i = 0; i < this.partyBench.Count; i++)
		{
			this.transform.GetChild (2 + i).GetComponent<Text> ().text = this.partyBench [i];
			BenchCharInfo(i);
		}
		this.animator.SetInteger ("State", 1);
	}

	public void Deactivate()
	{
		this.animator.SetInteger ("State", 0);
	}

	void MoveCursor()
	{
		if(Input.GetButtonDown("Vertical"))
		{
			this.cursor -= (int)Input.GetAxisRaw ("Vertical");
			if (this.cursor > this.partyBench.Count - 1)
				this.cursor = 0;
			if (this.cursor < 0)
				this.cursor = this.partyBench.Count - 1;
			this.animator.SetInteger ("Cursor", this.cursor);
		}
	}

	void ChooseCharacter()
	{
		if (Input.GetKeyDown (KeyCode.Z)) 
		{
			Characters_Allies currentAlly = this.battleRef.Sys_Interface.CurrentActor;
			currentAlly.SwitchedCharacter = this.partyBench [this.cursor];
			this.partyBench [this.cursor] = currentAlly.Prop_Name;
			currentAlly.ActionType = "Switch";
			currentAlly.UsedAction = "Summon";
			this.animator.SetInteger("State", 0);
			this.battleRef.Sys_Interface.CallBattleRoutine ();
		}
	}

	void BenchCharInfo(int index)
	{
		Transform currChar = this.transform.GetChild (2 + index);
		Transform HPObject = currChar.FindChild("HP");
		string charName = this.partyBench[index];
		int charMaxHp = StatFormula(SavedData.current.partyLevels[charName], this.battleRef.CharManager.characterBaseStats[charName]["Hp"]);
		if(this.battleRef.Sys_TempData.currCharHp.ContainsKey(charName))
		{
			HPObject.FindChild("HPValue").GetComponent<Text>().text = this.battleRef.Sys_TempData.currCharHp[charName] + "/"
			+ charMaxHp;
			HPObject.FindChild("HPGauge").GetComponent<Image>().fillAmount = (float)this.battleRef.Sys_TempData.currCharHp[charName] /
			(float)charMaxHp;
		}
		else
		{
			HPObject.FindChild("HPValue").GetComponent<Text>().text = charMaxHp + "/" + charMaxHp;
			HPObject.FindChild("HPGauge").GetComponent<Image>().fillAmount = charMaxHp / charMaxHp;
		}

		UpdateStaggerMarkers(currChar.FindChild("Stagger"), charName);
	}

	private int StatFormula(int level, int baseStat)
	{
		int result = (int)(((float)baseStat * 2.2f * (float)level) / 100 + 5) + level;
		return result;
	}

	public void UpdateStaggerMarkers(Transform staggerList, string charName)
	{
		int staggerStat = this.battleRef.CharManager.characterBaseStats[charName]["Stag"];
		List<int> staggerReceived = new List<int>();
		if(battleRef.Sys_TempData.currCharStagger.ContainsKey(charName))
			staggerReceived = battleRef.Sys_TempData.currCharStagger[charName];
		for (int i = 0; i < 6; i++) 
		{
			if (i < staggerStat) 
			{
				if(staggerReceived.Count > i)
					staggerList.GetChild (i).GetComponent<Animator> ().SetInteger ("Type", staggerReceived [i]);
				else
					staggerList.GetChild (i).GetComponent<Animator> ().SetInteger ("Type", 0);
			}
			else
				staggerList.GetChild (i).GetComponent<Animator> ().SetInteger ("Type", -1);
		}
	}
}
