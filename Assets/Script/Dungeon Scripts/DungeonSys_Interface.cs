using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonSys_Interface : MonoBehaviour
 {
 	[SerializeField]
 	DungeonSys_ObjectReferences sys_objectReferences;

 	private bool canAct;

 	void Update()
 	{
 		if(Input.GetButtonDown("Submit"))
 		{
 			this.sys_objectReferences.Sys_TurnActions.TurnRoutine();
 		}
 	}

 
	
}
