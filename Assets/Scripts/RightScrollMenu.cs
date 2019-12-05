using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightScrollMenu : AbsUIContentPanel
{
	[SerializeField]
	UIContentSizeFitter contentPlane;
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
		contentPlane.AddChild ((RectTransform)content.transform);
	}
	public override void ClearContent ()
	{
		for (int i = 0; i < contentPlane.transform.childCount; i++) 
		{Destroy(contentPlane.transform.GetChild(i).gameObject);}
		((RectTransform)contentPlane.transform).offsetMax = ((RectTransform)contentPlane.transform).offsetMin = Vector2.zero;
	}
}
