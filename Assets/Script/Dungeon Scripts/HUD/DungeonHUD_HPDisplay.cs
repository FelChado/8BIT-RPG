using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonHUD_HPDisplay : MonoBehaviour 
{
	[SerializeField]
	Managers_Characters charManager;
	Managers_Map mapManager;

	void Start()
	{
		this.mapManager = GameObject.FindGameObjectWithTag("MapManager").GetComponent<Managers_Map>();
		UpdateHP();
	}

	void UpdateHP()
	{
		for(int i = 0; i < SavedData.current.currentParty.Count; i++)
		{
			string currMember = SavedData.current.currentParty[i];
			Transform currObj = this.transform.GetChild(i + 1).GetChild(0);
			int currHp;
			if(this.mapManager.currHPList.ContainsKey(currMember))
				currHp = this.mapManager.currHPList[currMember];
			else
				currHp = this.charManager.GetCharCurrStat(currMember, "Hp");
			currObj.GetChild(0).GetComponent<Text>().text = (currHp + "/" + this.charManager.GetCharCurrStat(currMember, "Hp")).ToString();
			currObj.GetChild(1).GetComponent<Image>().fillAmount = currHp / this.charManager.GetCharCurrStat(currMember, "Hp");
		}
	}

}
