using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITintOnButtonDisable : MonoBehaviour {

	[SerializeField]
	Color tint;
	[SerializeField]
	Text text;
	[SerializeField]
	Button button;
	Color startColor;
	void Start()
		{startColor = text.color;}
	void Update () 
	{
		if (button.interactable) 
			{text.color = startColor;}
		else
			{text.color = tint;}
	}
}
