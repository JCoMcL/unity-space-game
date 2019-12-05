using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolChildManager : MonoBehaviour 
{
	public GameObject childPrefab;
	List<PoolChild> children = new List<PoolChild> ();

	void Start()
	{
		AddChild ();
	}
	PoolChild AddChild ()
	{
		GameObject child = Instantiate (childPrefab,transform);
		PoolChild poolChild = child.GetComponent<PoolChild> ();
		children.Add (poolChild);
		return poolChild;
	}
	public PoolChild GetAvailableChild()
	{
		foreach (PoolChild child in children) 
		{if (child.available) {return child;}}
		return AddChild ();
	}

}
