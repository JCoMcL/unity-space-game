using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextCheck : MonoBehaviour {
	[SerializeField]
	Text text;
	[SerializeField]
	Button submitButton;

	void Update () 
	{
		string txt = text.text;
		if (
			txt.Length < 1
		)
			{submitButton.interactable = false;}
		else
			{submitButton.interactable = true;}	
	}
}
