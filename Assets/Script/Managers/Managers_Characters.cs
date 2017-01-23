using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Managers_Characters : MonoBehaviour 
{
	public Dictionary<string, Dictionary<string, int>> characterBaseStats = new Dictionary<string, Dictionary<string, int>>();
	public Dictionary<string, string> characterBasicAttacks = new Dictionary<string, string>();
	public Dictionary<string, List<float>> characterWeaknesses = new Dictionary<string, List<float>>();

	// Use this for initialization
	public void CreateCharDatabase () 
	{
		CreateNewCharacter ("Halstein", "Sword Strike", 100, 60, 30, 15, 60, 35, 25, 15, 6);
		AddCharacterWeakness ("Halstein", 1, 1, 1, 1, 1, 1, 1);
		CreateNewCharacter ("Asmund", "Sword Strike", 80, 40, 35, 30, 25, 50, 30, 15, 2);
		AddCharacterWeakness ("Asmund", 1, 1, 1, 1, 1, 1, 1);
		CreateNewCharacter ("Nanna", "Sword Strike", 55, 50, 65, 5, 35, 30, 50, 20, 4);
		AddCharacterWeakness ("Nanna", 1, 1, 1, 1, 1, 1, 1);
		CreateNewCharacter ("Gustav", "Sword Strike", 50, 40, 10, 50, 20, 40, 35, 15, 3);
		AddCharacterWeakness ("Gustav", 1, 1, 1, 1, 1, 1, 1);
	}

	void CreateNewCharacter(string name, string basicAttack, int hp, int stam, int str, int mag, int def, int mnd, int agi, int luc, int stag)
	{
		Dictionary<string, int> charBaseStats = new Dictionary<string, int> ();
		charBaseStats.Add ("Hp", hp);
		charBaseStats.Add ("Stam", stam);
		charBaseStats.Add ("Str", str);
		charBaseStats.Add ("Mag", mag);
		charBaseStats.Add ("Def", def);
		charBaseStats.Add ("Mnd", mnd);
		charBaseStats.Add ("Agi", agi);
		charBaseStats.Add ("Luc", luc);
		charBaseStats.Add ("Stag", stag);
		this.characterBaseStats.Add (name, charBaseStats);
		this.characterBasicAttacks.Add (name, basicAttack);
	}

	void AddCharacterWeakness(string name, float slash, float strike, float pierce, float fire, float ice, float wind, float elec)
	{
		List<float> currCharWeaknesses = new List<float> ();
		currCharWeaknesses.Add (slash);
		currCharWeaknesses.Add (strike);
		currCharWeaknesses.Add (pierce);
		currCharWeaknesses.Add (fire);
		currCharWeaknesses.Add (ice);
		currCharWeaknesses.Add (wind);
		currCharWeaknesses.Add (elec);
		this.characterWeaknesses.Add (name, currCharWeaknesses);
	}

}
