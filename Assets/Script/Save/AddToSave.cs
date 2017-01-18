using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class AddToSave
{
	public static void AddCharacter(string name, int level)
	{
		SavedData.current.completeParty.Add (name);
		SavedData.current.partyLevels.Add (name, level);
	}

	public static void AddToParty(string name)
	{
		SavedData.current.currentParty.Add (name);
	}

	public static void AddSkillToCharacter(string charName, string skillName)
	{
		if(!SavedData.current.partySkills.ContainsKey(charName))
			SavedData.current.partySkills.Add (charName, new List<string> ());
		SavedData.current.partySkills [charName].Add (skillName);
	}
}
