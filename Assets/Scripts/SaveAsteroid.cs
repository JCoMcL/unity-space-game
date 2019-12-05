using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveAsteroid : MonoBehaviour {

	[SerializeField]
	Text text;
	[SerializeField]
	SpawnTest st;

	public void Save() 
		{st.SaveAsteroid (text.text);}
}
