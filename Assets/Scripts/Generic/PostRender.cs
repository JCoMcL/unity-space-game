using System.Collections;
using System;
using UnityEngine;

public class PostRender : MonoBehaviour {
	public event Action EarlyUpdate;
	void OnPostRender()
	{
		if (EarlyUpdate != null) 
		{EarlyUpdate ();}
	}
}
