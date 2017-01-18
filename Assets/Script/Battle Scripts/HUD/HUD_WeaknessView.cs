using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HUD_WeaknessView : MonoBehaviour
{
	[SerializeField]
	BattleSystem_References sys_references;

	private string enemyName;

	public void SetTypeEffectiveness()
	{
		for(int i = 0; i < 7; i++)
		{
			Text currentText = this.transform.GetChild(i).GetComponent<Text>();
			if(SavedData.current.weaknessScan.ContainsKey(this.enemyName))
			{
				if(SavedData.current.weaknessScan[enemyName].ContainsKey(i))
					currentText.text = SavedData.current.weaknessScan[enemyName][i];
				else
					currentText.text = "?";
			}
			else
				currentText.text = "?";
		}

	}

	public void Activate(bool active)
	{
		this.GetComponent<Image>().enabled = active;
		for(int i = 0; i < 7; i++)
		{
			this.transform.GetChild(i).GetComponent<Text>().enabled = active;
		}
		if(active)
			SetTypeEffectiveness();
	}

	public void SetNewTarget()
	{
		this.enemyName = this.sys_references.Sys_Master.enemyList[this.sys_references.Sys_Interface.Cursor].Prop_Name;
	}

}
