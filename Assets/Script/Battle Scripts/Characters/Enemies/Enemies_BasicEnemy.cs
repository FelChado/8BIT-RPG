using UnityEngine;
using System.Collections;

public class Enemies_BasicEnemy : Characters_Enemies 
{
	new void Start()
	{
		base.Start ();
		CreateSkillPool ();
	}

	public override void Act()
	{
	}
}
