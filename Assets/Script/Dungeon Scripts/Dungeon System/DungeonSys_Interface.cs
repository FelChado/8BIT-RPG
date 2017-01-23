using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonSys_Interface : MonoBehaviour
 {
 	[SerializeField]
 	DungeonSys_ObjectReferences sys_objectReferences;

 	private bool canAct = true;

	public bool CanAct
	{
		get{return this.canAct;}
		set{this.canAct = value;}
	}

 	void Update()
 	{
 		if(Input.GetButtonDown("Submit") && this.canAct)
		{
 			this.canAct = false;
 			this.sys_objectReferences.Sys_TurnActions.TurnRoutine();
 		}
 	}
}
