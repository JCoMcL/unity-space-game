using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIContentSizeFitter : MonoBehaviour 
{
	[SerializeField]
	RectTransform rectT;
	HashSet<RectTransform> children = new HashSet<RectTransform>();
	public void Sort () 
	{
		rectT.offsetMax = rectT.offsetMin = Vector2.zero;
		for (int i = 0; i < transform.childCount; i++) 
		{
			RectTransform child = (RectTransform)transform.GetChild (i);
			if (true)//!children.Contains (child)) 
			{InitializeChild (child);}
		}
	}
	public void AddChild(RectTransform child)
	{
		child.transform.SetParent (transform);
		InitializeChild (child);
	}
	void InitializeChild(RectTransform child)
	{
		float childHeight = Mathf.Abs (child.offsetMax [1] - child.offsetMin [1]);
		child.offsetMax = rectT.offsetMin;
		rectT.offsetMin -= new Vector2 (0, childHeight);
		child.offsetMin = rectT.offsetMin;
		children.Add (child);
	}
	public void Refresh()//needs to reset positions of children
	{
		float height = 0;
		children.Clear ();
		for (int i = 0; i < transform.childCount; i++) 
		{
			RectTransform child = transform.GetChild (i).GetComponent<RectTransform>();
			height += Mathf.Abs (child.offsetMax [1] - child.offsetMin [1]);
			children.Add (child);
			print (height);
		}
		rectT.offsetMax = Vector2.zero;
		rectT.offsetMin = new Vector2 (0, height*-1);
	}

}
