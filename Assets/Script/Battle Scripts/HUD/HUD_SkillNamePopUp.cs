using UnityEngine;
using System.Collections;

public class HUD_SkillNamePopUp : MonoBehaviour 
{
	[SerializeField]
	BattleSystem_References battleRef;

	protected void BattleContinue()
	{
		this.battleRef.Sys_TurnActions.Sys_BattleStop = false;
	}
}
