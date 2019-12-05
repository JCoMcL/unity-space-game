using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolChild : MonoBehaviour 
{
	public bool available = true;
	public bool reserved = false;

	protected virtual void Start()
	{
		Camera.main.GetComponent<PostRender> ().EarlyUpdate += CheckFrameReset;
	}
	void CheckFrameReset()
	{	
		if (!available && !reserved) 
		{
			FrameReset ();
		}
	}
	protected virtual void FrameReset()
	{
		available = true;
	}
	protected virtual void SetActive()
	{
		if(!available)	{return;}
		available = false;
	}
}
