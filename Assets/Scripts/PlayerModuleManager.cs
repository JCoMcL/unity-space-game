using UnityEngine;
using System.Collections.Generic;

public class PlayerModuleManager : ModuleManager 
{
	protected Dictionary<Collider2D,Module> colliderDictionary = new Dictionary<Collider2D,Module> ();

	public override void AddModule (Module module)
	{
		colliderDictionary.Add (module.containingCollider, module);
		base.AddModule (module);
		if (module.gridLocation == Vector2.up) 
		{core.GetComponent<Core> ().armed = false;}
	}
	protected override void RemoveModule (Module module)
	{
		if (module.gridLocation == Vector2.up) 
		{core.GetComponent<Core> ().armed = true;}
		colliderDictionary.Remove(module.containingCollider);
		base.RemoveModule (module);
	}
	Vector2 GetLocalMouse()
	{
		Vector3 localMouse = transform.InverseTransformPoint (Conversion.mouseToMetres3D());
		return new Vector2 (Mathf.Round (localMouse [0]), Mathf.Round (localMouse [1]));
	}
	void OnMouseOver ()
	{
		if (Input.GetMouseButtonDown (0))
		{	
			Vector2 localMouse = GetLocalMouse();
			if(localMouse != Vector2.zero)
			{DetachModule(gridDictionary [GetLocalMouse()]);}
		}
	}
/*	void OnDrawGizmos() 
	{
		Component[] colliders;
		colliders = gameObject.GetComponentsInChildren(typeof(BoxCollider2D));
		Vector3 topleft = new Vector2 (-0.5f, 0.5f);
		foreach(Collider2D box in colliders)
		{
			if (colliderDictionary.ContainsKey(box))
			{Handles.Label (box.transform.position + topleft, width + ", " + height +"\n"
				+(colliderDictionary[box].relations[0] != null) +"\n"
				+(colliderDictionary[box].relations[1] != null) +"\n"
				+(colliderDictionary[box].relations[2] != null) +"\n"
				+(colliderDictionary[box].relations[3] != null));}
		}
	}*/

	public override GameManager.ObjectConstructor Save ()
	{
		return new ModuleManagerConstructor(this);
	}
}
