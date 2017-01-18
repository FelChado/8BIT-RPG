using UnityEngine;
using System.Collections;

public class NewGame : MonoBehaviour 
{

	void Awake()
	{
		CreateNewGame ();
	}

	public void CreateNewGame()
	{
		SavedData.current = new SavedData();
		SaveLoad.savedGames.Add(SavedData.current);
		AddToSave.AddCharacter ("Halstein", 1);
		AddToSave.AddToParty ("Halstein");
		AddToSave.AddSkillToCharacter ("Halstein", "Heal");
		AddToSave.AddSkillToCharacter ("Halstein", "Fire");

		AddToSave.AddCharacter ("Asmund", 1);
		AddToSave.AddToParty ("Asmund");
		AddToSave.AddSkillToCharacter ("Asmund", "Heal");

		AddToSave.AddCharacter ("Nanna", 1);
		AddToSave.AddToParty ("Nanna");
		AddToSave.AddSkillToCharacter ("Nanna", "Sword Strike");

		AddToSave.AddCharacter ("Gustav", 1);
		AddToSave.AddSkillToCharacter ("Gustav", "Heal");
		AddToSave.AddToParty ("Gustav");
	}
}
