using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextPadding : MonoBehaviour 
{
	[SerializeField]
	Text text;
	[SerializeField]
	RectTransform parent;
	[SerializeField]
	int padding;
	[SerializeField]
	[Range (0,1)]
	float percentage;
	void Update () 
	{
		Refresh ();
	}
	void Refresh()
	{
		float height = Mathf.Abs(GetParent ().offsetMax [1] - GetParent ().offsetMin [1]);
		if (height == 0) 
		{
			height = Mathf.Abs (GetParent ().anchorMax [1] - GetParent ().anchorMin [1]);
			height *= Camera.main.pixelHeight;
		}
		GetTextComponent ().fontSize = (int)Mathf.Round((height - padding) * percentage);
	}
	Text GetTextComponent()
	{
		if (text == null) 
			{text = GetComponent<Text> ();}
		return text;
	}
	RectTransform GetParent()
	{
		if (parent == null) 
			{parent = transform.parent.GetComponent<RectTransform> ();}
		return parent;
	}

}
