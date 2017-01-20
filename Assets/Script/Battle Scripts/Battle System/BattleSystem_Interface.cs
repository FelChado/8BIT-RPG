using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BattleSystem_Interface : MonoBehaviour 
{
	[SerializeField]
	Animator anim_battleMenu;
	[SerializeField]
	Image cursorImage;
	[SerializeField]
	BattleSystem_References battleRef;
	[SerializeField]
	BattleInterface_SkillMenu skillMenu;
	[SerializeField]
	BattleInterface_SwitchMenu switchMenu;
	[SerializeField]
	HUD_WeaknessView hud_weaknessView;

	Characters_Allies currentActor;

	private bool active, switchMenuOn;
	private int menuState = 0, cursor = 0, cursorLimit = 3, cursorMemory;
	private string cursorDirection = "Vertical", currTargetParty;

	#region GETS & SETS

	public Characters_Allies CurrentActor
	{
		get{ return this.currentActor;}
		set{ this.currentActor = value;}
	}

	public bool Active
	{
		get{ return this.active;}
		set{ this.active = value;}
	}

	public int Cursor
	{
		get{ return this.cursor;}
		set{ this.cursor = value;}
	}

	#endregion

	void Update()
	{
		if (this.currentActor != null && this.active) 
		{
			if(Input.GetKeyDown(KeyCode.Z))
			{
				switch(this.menuState)
				{
					// Choose Action
					case 0:
						ChooseMenuAction ();
					break;
					// Choose Target
					case 1:
						if(this.currTargetParty == "Enemy")
							this.currentActor.TargetCharacter = this.battleRef.Sys_Master.enemyList[this.cursor];
						if(this.currTargetParty == "Ally")
							this.currentActor.TargetCharacter = this.battleRef.Sys_Master.allyList[this.cursorLimit - this.cursor];
						CallBattleRoutine();
					break;
					// Choose Skill
					case 2:
						ChooseSkill ();
					break;
				}
			}

			// Open Switch Menu
			if(this.cursorDirection.Equals("Vertical"))
			{
				if (Input.GetButtonDown ("Horizontal"))
				{
					if (Input.GetAxisRaw ("Horizontal") == 1)
					{
						this.switchMenuOn = true;
						this.switchMenu.Activate ();
					}
					if (Input.GetAxisRaw ("Horizontal") == -1)
					{
						this.switchMenuOn = false;
						this.switchMenu.Deactivate ();
					}
				}
			}
				
			if(!this.switchMenuOn)
				MoveCursor ();

			// Go back on the menu
			if (this.menuState > 0)
			{
				if (Input.GetKeyDown (KeyCode.X))
				{
					GoBackOnMenu ();
				}
			}
		}

	}

	#region Menu Choices

	void ChooseMenuAction()
	{
		switch(this.cursor)
		{
			case 0:
				this.currentActor.ActionType = "Attack";
				this.cursorMemory = 0;
				this.currentActor.UsedAction = this.currentActor.BasicAttack;
				this.menuState = 1;
				this.currTargetParty = "Enemy";
				this.hud_weaknessView.SetNewTarget();
				this.hud_weaknessView.Activate(true);
				ChooseTarget();
			break;
			case 1:
				this.currentActor.ActionType = "Skill";
				this.cursorMemory = 1;
				this.cursorLimit = this.currentActor.skillList.Count - 1;
				this.menuState = 2;
				this.cursor = 0;
				this.anim_battleMenu.SetInteger("Cursor", this.cursor);
				this.anim_battleMenu.SetInteger("Type", 2);
				this.skillMenu.SetSkills(this.currentActor.skillList);
				this.skillMenu.SetDescription(0);
			break;
			case 2:
				this.currentActor.ActionType = "Guard";
				this.currentActor.UsedAction = "Guard";
				this.CallBattleRoutine ();
			break;
			case 3:
				this.menuState = 4;
			break;
		}
	}

	void ChooseSkill()
	{
		if(this.battleRef.SkillManager.skillList[this.currentActor.skillList [this.cursor]]["Cost"] < this.currentActor.Temp_CurrStam)
		{
			this.cursorMemory = this.cursor;
			this.currentActor.UsedAction = this.currentActor.skillList [this.cursor];
			switch ((int)this.battleRef.SkillManager.skillList [this.currentActor.UsedAction] ["TargetParty"]) 
			{
				case 0:
					this.currTargetParty = "Ally";
					break;
				case 1:
					this.currTargetParty = "Enemy";
					break;
			}
			this.cursor = 0;
			this.menuState = 1;
			this.hud_weaknessView.SetNewTarget();
			this.hud_weaknessView.Activate(true);
			ChooseTarget();
		}
		else
		{
			this.battleRef.SkillNameAnimator.transform.GetChild(0).GetComponent<Text>().text = "Not enough stamina!";
			this.battleRef.SkillNameAnimator.SetTrigger("On");
		}
	}
		
	void ChooseTarget()
	{
		// Set Cursor Limit
		if (this.currTargetParty.Equals ("Enemy") && this.cursorLimit != this.battleRef.Sys_Master.enemyList.Count - 1) 
		{
			this.cursorLimit = this.battleRef.Sys_Master.enemyList.Count - 1;
		}
		if (this.currTargetParty.Equals ("Ally") && this.cursorLimit != this.battleRef.Sys_Master.allyList.Count - 1) 
		{
			this.cursorLimit = this.battleRef.Sys_Master.allyList.Count - 1;
		}
		// Set Cursor Direction
		if (this.cursorDirection != "Horizontal") 
		{
			this.cursorDirection = "Horizontal";
			this.cursor = this.cursorLimit;
			CursorAwayFromDead (1);
		}
		// Set Cursor Image
		if(!this.cursorImage.enabled)
			this.cursorImage.enabled = true;
		// Set Cursor Position
		if(this.currTargetParty.Equals("Enemy"))
			this.cursorImage.gameObject.transform.position = this.battleRef.Sys_Master.enemyList[this.cursor].transform.position;
		if(this.currTargetParty.Equals("Ally"))
			this.cursorImage.gameObject.transform.position = this.battleRef.Sys_Master.allyList[this.cursorLimit - this.cursor].transform.position;
	}

	void GoBackOnMenu()
	{
		if (this.menuState == 2 || (this.menuState == 1 && this.currentActor.ActionType == "Attack")) 
		{
			ResetMenus ();
			this.hud_weaknessView.Activate(false);
			this.cursor = this.cursorMemory;
			this.anim_battleMenu.SetInteger("Cursor", this.cursorMemory);
			this.currentActor.ActionType = "";
			this.skillMenu.ClearDescription();
		}
		if ((this.menuState == 1 && this.currentActor.ActionType == "Skill"))
		{
			this.currTargetParty = "";
			this.hud_weaknessView.Activate(false);
			this.cursor = this.cursorMemory;
			this.anim_battleMenu.SetInteger("Cursor", this.cursorMemory);
			this.cursorMemory = 1;
			this.cursorImage.enabled = false;
			this.cursorLimit = this.currentActor.skillList.Count - 1;
			this.cursorDirection = "Vertical";
			this.currentActor.UsedAction = "";
			this.menuState = 2;
		}
	}

	#endregion

	#region Utilities

	void MoveCursor()
	{
		if(Input.GetButtonDown(this.cursorDirection))
		{
			if(Input.GetAxisRaw(this.cursorDirection) == 1)
			{
				if(this.cursor > -1)
					this.cursor--;
				if(this.menuState == 2)
				{
					this.skillMenu.ScrollList(-1, this.cursor);
					this.skillMenu.SetDescription(this.cursor);
				}
				if (this.cursor < 0)
				{
					this.cursor = 0;
					if (this.menuState == 1)
						this.cursor = this.cursorLimit;
				}
				if((this.menuState == 0 || this.menuState == 2) && this.anim_battleMenu.GetInteger("Cursor") > 0)
					this.anim_battleMenu.SetInteger("Cursor", this.anim_battleMenu.GetInteger("Cursor") - 1);
				if (this.menuState == 1) 
				{
					CursorAwayFromDead (1);
					ChooseTarget ();
				}

			}
			if(Input.GetAxisRaw(this.cursorDirection) == -1)
			{
				if (this.cursor < this.cursorLimit)
					this.cursor++;
				else if (this.menuState == 1)
					this.cursor = 0;
				if (this.menuState == 2) 
				{
					this.skillMenu.ScrollList (1, this.cursor);
					this.skillMenu.SetDescription(this.cursor);
				}
				if (this.cursor > 3 && this.menuState != 1)
					this.cursor = 3;
				if((this.menuState == 0 || this.menuState == 2) && this.anim_battleMenu.GetInteger("Cursor") < 4 &&
					this.anim_battleMenu.GetInteger("Cursor") < 3 && this.anim_battleMenu.GetInteger("Cursor") < this.cursorLimit)
					this.anim_battleMenu.SetInteger("Cursor", this.anim_battleMenu.GetInteger("Cursor") + 1);
				if (this.menuState == 1)
				{
					CursorAwayFromDead (-1);
					ChooseTarget ();
				}
			}

			if(menuState == 1)
				this.hud_weaknessView.SetNewTarget();
		}
	}

	// Set menu to Default
	void ResetMenus()
	{
		this.switchMenuOn = false;
		this.skillMenu.ClearDescription();
		this.currTargetParty = "";
		this.cursor = 0;
		this.menuState = 0;
		this.cursorLimit = 3;
		this.cursorDirection = "Vertical";
		this.anim_battleMenu.SetInteger("Type", 0);
		this.anim_battleMenu.SetInteger("Cursor", 0);
		this.cursorImage.enabled = false;
	}

	// End Choices
	public void CallBattleRoutine()
	{
		this.active = false;
		ResetMenus();
		this.currentActor.UseAction ();
		StartCoroutine (this.battleRef.Sys_TurnActions.BattleRoutine (this.currentActor, this.currentActor.TargetCharacter));
		this.currentActor.UpdateUI();
		this.currentActor = null;
	}

	void CursorAwayFromDead(int dir)
	{
		if (this.currTargetParty == "Ally" && this.menuState == 1) 
		{
			while (this.battleRef.Sys_Master.allyList [this.cursorLimit - this.cursor].Fainted)
			{
				if (dir == 1) 
				{
					if (this.cursor > 0)
						this.cursor--;
					else
						this.cursor = this.cursorLimit;
				}
				if (dir == -1) 
				{
					if (this.cursor < this.cursorLimit)
						this.cursor++;
					else
						this.cursor = 0;
				}
			}
		}
	}

	#endregion
}
