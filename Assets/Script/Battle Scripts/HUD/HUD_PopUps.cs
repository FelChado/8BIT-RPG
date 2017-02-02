using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_PopUps : MonoBehaviour 
{
	[SerializeField]
	GameObject damagePrefab = null, healPrefab = null, weaknessPrefab = null, canvasObject = null;
	GameObject damageCounterSpawn, weaknessSpawn;

	public void SpawnDamageCounter(float quantity, Transform objectPos, string type)
	{
		if(type.Equals("Damage"))
			this.damageCounterSpawn = (GameObject)GameObject.Instantiate(this.damagePrefab, objectPos.position, Quaternion.identity);
		if(type.Equals("Heal"))
			this.damageCounterSpawn = (GameObject)GameObject.Instantiate(this.healPrefab, objectPos.position, Quaternion.identity);
		this.damageCounterSpawn.transform.SetParent(this.canvasObject.transform);
		this.damageCounterSpawn.transform.localScale = new Vector3 (0.32f, 0.32f, 1);
		this.damageCounterSpawn.transform.GetChild(0).GetComponent<Text>().text = ((int)quantity).ToString();
	}

	public void SpawnWord(string word, Transform objectPos)
	{
		this.weaknessSpawn = (GameObject)GameObject.Instantiate (this.weaknessPrefab, objectPos.position, Quaternion.identity);
		this.weaknessSpawn.transform.SetParent (this.canvasObject.transform);
		this.weaknessSpawn.transform.localScale = new Vector3 (0.32f, 0.32f, 1);
		this.weaknessSpawn.transform.GetChild (0).GetComponent<Text> ().text = word;
	}
}
