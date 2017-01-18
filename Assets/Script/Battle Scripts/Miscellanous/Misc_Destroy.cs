using UnityEngine;
using System.Collections;

public class Misc_Destroy : MonoBehaviour 
{

	private void Destroy()
	{
		Destroy (this.gameObject);
	}
}
