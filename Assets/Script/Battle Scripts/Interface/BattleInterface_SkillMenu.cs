using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BattleInterface_SkillMenu : MonoBehaviour 
{
	[SerializeField]
	BattleSystem_References battleRef;
	[SerializeField]
	Transform descriptionBox;
	public List<string> skillList = new List<string>();
	private int cursorDistance;

	public void ScrollList(int dir, int cursor)
	{
		if ((dir == 1 && this.cursorDistance + 4 < this.skillList.Count && cursor > 3 && cursor < this.skillList.Count) ||
			(dir == -1 && cursor < this.cursorDistance - 1 && cursor == -1) ) 
		{
			this.cursorDistance += dir;
			SetSkills (this.skillList);
		}
	}

	public void SetSkills(List<string> skillList)
	{
		ClearList();
		this.skillList = skillList;
		for (int i = 0; i < this.skillList.Count; i++) 
		{
			if(i < 4)
			{
				this.transform.GetChild(i).GetComponent<Text>().text = this.skillList[i + this.cursorDistance];
				this.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = this.battleRef.SkillManager.skillList[this.skillList[i + this.cursorDistance]]["Cost"].ToString();
			}
		}

	}

	void ClearList()
	{
		for (int i = 0; i < 4; i++) 
		{
			if(i < 4)
			{
				this.transform.GetChild(i).GetComponent<Text>().text = "";
				this.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "";
			}
		}
	}

	public void SetDescription(int cursor)
	{
		if(cursor < this.skillList.Count && cursor > -1	)
			this.descriptionBox.FindChild("Description").GetComponent<Text>().text = this.battleRef.SkillManager.skillDescriptions[this.skillList[this.cursorDistance + cursor]];
	}

	public void ClearDescription()
	{
			this.descriptionBox.FindChild("Description").GetComponent<Text>().text = "";
	}
}
