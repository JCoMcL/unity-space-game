using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullContentPlane : AbsUIContentPanel 
{
	[SerializeField]
	InputField text;
	public override void Show ()
	{
		gameObject.SetActive (true);
	}
	public override void Hide ()
	{
		gameObject.SetActive (false);
		ClearContent ();
	}
	public override void AddContent (GameObject content)
	{
	}
	public override void ClearContent ()
	{
		text.text = "";
	}
}
