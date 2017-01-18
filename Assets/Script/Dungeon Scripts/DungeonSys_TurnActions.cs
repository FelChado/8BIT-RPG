using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonSys_TurnActions : MonoBehaviour
 {
 	[SerializeField]
 	DungeonSys_ObjectReferences sys_objectReference;

	public void TurnRoutine()
	{
		int charSteps = DiceRoll();
		this.sys_objectReference.MainCharacter.CallAction(2);
	}

	int DiceRoll()
 	{
 		return Random.Range(1, 6);
 	}
}
