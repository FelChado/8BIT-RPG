using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SavedData 
{
	// Current Save File
	public static SavedData current;
	public static int currentSave;

	// System variables
	public int currentPkmnId = 0;

	// Party Info
	public List<string> currentParty = new List<string>();
	public List<string> completeParty = new List<string>();
	public Dictionary<string, int> partyLevels = new Dictionary<string, int>();
	public Dictionary<string, List<string>> partySkills = new Dictionary<string, List<string>>();

	// Monster info
	public Dictionary<string, Dictionary<int, string>> weaknessScan = new Dictionary<string, Dictionary<int, string>>();
}
