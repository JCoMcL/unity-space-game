using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Module : MonoBehaviour//, //IPersistable<GameManager.GameSave,GameManager.ObjectConstructor>
{
	public ShipComponent main;
	public Collider2D containingCollider;
	public GameObject[] edges;
	public Vector2 gridLocation;
	public int gridRotation;
	public Module[] relations;
	public int baconNumber = -1;
	public int index;
	public int moduleCount = 2000;//this is a bad idea
	public bool attachable;

	public void Initialize(Collider2D coll, int rot)
	{	
		gridRotation = rot;

		transform.position = coll.transform.parent.transform.TransformPoint (coll.transform.localPosition * 2);
		gameObject.transform.SetParent (coll.transform.parent.parent);
		transform.localRotation = Quaternion.Euler (new Vector3 (0, 0, gridRotation * 90 - 90));
		Vector2 roundPos = new Vector2 (Mathf.Round (transform.localPosition [0]), Mathf.Round (transform.localPosition [1]));
		transform.localPosition = roundPos;
		gridLocation = roundPos;
	}
	public void AddRelation(Module relation, int index)
	{
		if ((relation.attachable || relation.gridRotation == index)
			&& (attachable || index == Conversion.FlipDirection (gridRotation))) 
			{relations [index] = relation;}
		edges [Conversion.RoundInt (index - gridRotation + 1)].SetActive(false);
	}
	public Module GetInstance(){return this;}
	public void UpdateBaconNumber(int number)
	{
		baconNumber = number;
		foreach (Module neighbour in relations){	if (neighbour != null && neighbour.baconNumber > number + 1) 
			{neighbour.UpdateBaconNumber (number + 1);}}
	}
	public void BaconCrawl(ref int number, ref Queue<Module> queue)
	{	
		for (int i = 0;i < 4;i++) 
		{	
			if (relations [i] != null && relations[i].baconNumber != moduleCount) 
			{	
				if (relations [i].baconNumber >= number) 
				{	number = relations [i].baconNumber;
					relations [i].baconNumber = moduleCount;
					queue.Enqueue(relations [i]);
				} else 
				{	relations [i].UpdateBaconNumber (relations [i].baconNumber);}
			}
		}
		if (queue.Count > 0) 
		{queue.Dequeue().BaconCrawl (ref number, ref queue);}
	}
	public void RemoveRelation(int relation, bool last)	
	{
		bool baconSource = last;
		if (baconSource) 
		{
			int neighbourBacon = relations [relation].baconNumber;
			baconSource = baconNumber > neighbourBacon;
			for (int i = 0; i < 4 && baconSource; i++) 
			{	
				if (relations [i] != null && i != relation && relations [i].baconNumber < neighbourBacon) 
				{baconSource = false;}
			}
		}
		relations [relation] = null;
		GameObject edge = edges [Conversion.RoundInt (relation - gridRotation + 1)];
		edge.SetActive(true);
		int number = baconNumber;
		Queue<Module> queue = new Queue<Module> ();
		if (baconSource) 
		{
			baconNumber = moduleCount;
			BaconCrawl (ref number,ref queue);
		}
	}
	public void Detach(Module first,bool last)
	{	for (int i = 0; i < 4; i++) 
		{	
			Module neighbour = relations [i];
			if (neighbour != null) 
				{neighbour.RemoveRelation (Conversion.FlipDirection (i), last);}
		}
		//clear relevant data
		main.Detach (first.transform.position);
	}
	void OnKill()
	{	
		if (enabled && main!= null && main.manager != null) 
		{main.manager.DetachModule (this);}
	}
}