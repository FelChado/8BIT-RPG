using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonInterface_Menu : MonoBehaviour 
{
	[SerializeField]
	Animator animator;
	[SerializeField]
	DungeonSys_ObjectReferences sys_ojectReferences;
	private int cursor, cursorLimit;

	private bool state_menuOpen;

	public bool State_MenuOpen
	{
		get{return this.state_menuOpen;}
		set{this.state_menuOpen = value;}
	}

	void Start()
	{
		this.cursorLimit = this.transform.childCount - 1;
	}

	void Update () 
	{
		OpenCloseMenu();
		if(this.state_menuOpen)
			SetCursor();
	}

	void OpenCloseMenu()
	{
		if(Input.GetButtonDown("Cancel") && this.sys_ojectReferences.Sys_interface.CanAct)
		{
			if(this.state_menuOpen)
			{
				this.animator.SetTrigger("Close");
			}
			else
				this.animator.SetTrigger("Open");
			this.state_menuOpen = !this.state_menuOpen;
			if(!this.state_menuOpen)
			{
				this.transform.GetChild(this.cursor).GetComponent<Animator>().SetTrigger("Off");
				this.cursor = 0;
			}
			else
			{
				this.transform.GetChild(this.cursor).GetComponent<Animator>().SetTrigger("On");
			}
		}
	}

	void SetCursor()
	{
		if(Input.GetButtonDown("Horizontal"))
		{
			this.transform.GetChild(this.cursor).GetComponent<Animator>().SetTrigger("Off");
			this.cursor += (int)Input.GetAxisRaw("Horizontal");
			if(this.cursor > this.cursorLimit)
				this.cursor = 0;
			if(this.cursor < 0)
				this.cursor = this.cursorLimit;
			this.transform.GetChild(this.cursor).GetComponent<Animator>().SetTrigger("On");
		}
	}

}
