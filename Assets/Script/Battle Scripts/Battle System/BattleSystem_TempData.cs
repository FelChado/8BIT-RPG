using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleSystem_TempData : MonoBehaviour 
{
	public Dictionary<string, int> currCharHp = new Dictionary<string, int>();
	public Dictionary<string, List<int>> currCharStagger = new Dictionary<string, List<int>>();

	public void WriteCharData(string name, int hp, List<int> staggerList)
	{
		if (!this.currCharHp.ContainsKey (name))
		{
			this.currCharHp.Add (name, hp);
			this.currCharStagger.Add (name, staggerList);
		}
		else
		{
			this.currCharHp [name] = hp;
			this.currCharStagger[name] = staggerList;
		}
	}

}
