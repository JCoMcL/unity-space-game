using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleManager : MonoBehaviour, IPersistable<GameManager.ObjectConstructor>
{
	[SerializeField]
	protected int spawnIndex; //Better way?
	protected int[] gridXMax = new int[]{0,0};
	protected int[] gridXMin = new int[]{0,0};
	protected int[] gridYMax = new int[]{0,0};
	protected int[] gridYMin = new int[]{0,0};
	protected int width =1;
	protected int height =1;
	protected Vector2 centre = Vector2.zero;
	protected Module core;
	//are dictionaries neccesary?
	protected Dictionary<Vector2, Module> gridDictionary = new Dictionary<Vector2, Module> ();
	protected List<Module> indexedModules = new List<Module> ();

	protected virtual void Start ()
	{
		//GameManager.singleton.SaveEvent += Save;
		core = gameObject.GetComponentInChildren<Module> ();
		if (core != null) 
		{
			try{AddModule (core);}
			catch{return;}
			core.baconNumber = 0;
		}
	}
	void UpdateBounds()
	{
		width = gridXMax [1] - gridXMin [1] + 1;
		height = gridYMax [1] - gridYMin [1] + 1;
		centre = new Vector2 (gridXMin[1] * -1, gridYMin[1] * -1);
	}
	public virtual void AddModule(Module module)
	{
		gridDictionary.Add (module.gridLocation, module);
		indexedModules.Add (module);
		module.index = indexedModules.IndexOf(module);
		module.relations = new Module[4]; 
		AddModuleRelations (module);
		AddBounds (module.gridLocation);
	}
	void AddBounds(Vector2 grid)
	{
		bool update = false;
		if (grid [0] == gridXMax [1]) {gridXMax [0]++;}
		else if (grid [0] > gridXMax [1]) 
		{
			gridXMax [0] = 1; 
			gridXMax [1] = (int)grid [0];
			update = true;
		}
		if (grid [0] == gridXMin [1]) {gridXMin [0]++;}
		else if (grid [0] < gridXMin [1]) 
		{
			gridXMin [0] = 1; 
			gridXMin [1] = (int)grid [0];
			update = true;
		}
		if (grid [1] == gridYMax [1]) {gridYMax [0]++;}
		else if (grid [1] > gridYMax [1]) 
		{
			gridYMax [0] = 1; 
			gridYMax [1] = (int)grid [1];
			update = true;
		}
		if (grid [1] == gridYMin [1]) {gridYMin [0]++;}
		else if (grid [1] < gridYMin [1]) 
		{
			gridYMin [0] = 1; 
			gridYMin [1] = (int)grid [1];
			update = true;
		}
		if (update) {UpdateBounds ();}
	}
	void AddModuleRelations(Module module)
	{
		for(int i = 0; i<4; i++)
		{AddModuleRelation (module, i);}

		module.baconNumber++;
		foreach (Module neighbour in module.relations)
		{	
			if (neighbour != null && neighbour.baconNumber > module.baconNumber + 1) 
			{neighbour.UpdateBaconNumber (module.baconNumber + 1);}
		}
	}
	void AddModuleRelation(Module module, int direction, bool filter = true)
	{
		Vector2 offset = Conversion.IntToVector2 (direction);
		Module relative;
		if (gridDictionary.ContainsKey (module.gridLocation + offset)) 
		{
			relative = gridDictionary [module.gridLocation + offset];
			if(relative.attachable || !filter)
			{
				module.AddRelation (relative,direction);
				relative.AddRelation (module,Conversion.FlipDirection (direction));
				if (module.baconNumber == -1 || relative.baconNumber < module.baconNumber) 
				{module.baconNumber = relative.baconNumber;}
			}
		}
	}
	public virtual void DetachModule(Module module)
	{
		Module first = module;
		Queue<Module> toRemove = new Queue<Module> ();
		int[,] map = new int[width,height];
		int baconNumber = module.baconNumber;
		Vector2 mapLoc = module.gridLocation + centre;

		map [(int)mapLoc[0],(int)mapLoc[1]] = 1;
		toRemove.Enqueue (module);
		foreach(Module baseRelation in module.relations){	if (baseRelation != null && baseRelation.baconNumber > baconNumber) 
			{
				List<Module> foundModules = new List<Module> ();
				List<Vector2> foundLocations = new List<Vector2> ();
				Queue<Module> queue = new Queue<Module> ();

				mapLoc = baseRelation.gridLocation + centre;
				map [(int)mapLoc[0],(int)mapLoc[1]] = 1;
				foundModules.Add (baseRelation);
				foundLocations.Add (mapLoc);
				queue.Enqueue (baseRelation);
				// 0 = unchecked, 1 = checked, 2 = confirmed dead end
				while (queue.Count > 0) 
				{
					Module current = queue.Dequeue ();
					foreach (Module relation in current.relations){	if (relation != null) 
						{
							mapLoc = relation.gridLocation + centre;
							int flag = map[(int)mapLoc[0],(int)mapLoc[1]];
							if (flag != 1) 	
							{
								if (flag == 2) 
								{
									foreach(Vector2 loc in foundLocations)
									{map [(int)loc[0],(int)loc[1]] = 2;}
									queue.Clear ();
									foundModules.Clear ();
									break;
								}
								foundLocations.Add (mapLoc);
								if (relation.baconNumber <= baconNumber) 
								{
									foreach(Vector2 loc in foundLocations)
									{map [(int)loc[0],(int)loc[1]] = 2;}
									queue.Clear ();
									foundModules.Clear ();
									break;
								}
								map [(int)mapLoc [0], (int)mapLoc [1]] = 1;
								foundModules.Add (relation);
								queue.Enqueue (relation);

							}
						}}
				}
				foreach (Module foundModule in foundModules) 
				{toRemove.Enqueue (foundModule);}
			}}
		while (toRemove.Count > 0)
		{
			Module removedModule = toRemove.Dequeue ();
			RemoveModule(removedModule);
			removedModule.Detach (first, toRemove.Count == 1);
		}
	}
	protected virtual void RemoveModule(Module module)
	{
		RemoveIndex (module.index);
		gridDictionary.Remove (module.gridLocation);
		RemoveBounds (module.gridLocation);
		if (!module.attachable) 
		{	for (int i = 0; i < 4; i++) 
			{
				Vector2 grid = module.gridLocation + Conversion.IntToVector2 (i);
				if(gridDictionary.ContainsKey (grid))
				{gridDictionary [grid].RemoveRelation (Conversion.FlipDirection (i),false);}
			}
		}
	}
	void RemoveBounds(Vector2 grid)
	{
		bool update = false;
		if (grid [0] == gridXMax [1]) 
		{
			gridXMax [0]--;
			if (gridXMax [0] == 0) 
			{
				gridXMax [1]--;
				gridXMax [0] = GetModulesAtLength (gridXMax [1], 0);
				update = true;
			}
		}
		if (grid [0] == gridXMin [1]) 
		{
			gridXMin [0]--;
			if (gridXMin [0] == 0) 
			{
				gridXMin [1]++;
				gridXMin [0] = GetModulesAtLength (gridXMin [1], 0);
				update = true;
			}
		}
		if (grid [1] == gridYMax [1]) 
		{
			gridYMax [0]--;
			if (gridYMax [0] == 0) 
			{
				gridYMax [1]--;
				gridYMax [0] = GetModulesAtLength (gridYMax [1], 1);
				update = true;
			}
		}
		if (grid [1] == gridYMin [1]) 
		{
			gridYMin [0]--;
			if (gridYMin [0] == 0) 
			{
				gridYMin [1]++;
				gridYMin [0] = GetModulesAtLength (gridXMin [1], 1);
				update = true;
			}
		}
		if (update) {UpdateBounds ();}
	}
	void RemoveIndex(int index)
	{
		indexedModules.RemoveAt (index);
		for (int i = index; i < indexedModules.Count; i++)
		{indexedModules [i].index--;}
	}
	int GetModulesAtLength(int x,int axis)
	{
		int count = 0;
		foreach(Module module in indexedModules)
		{if (module.gridLocation [axis] == x){count++;}}
		return count;
	}

	#region Persistance
	public virtual GameManager.ObjectConstructor Save()
	{
		return new ModuleManagerConstructor(this);
	}
	[System.Serializable]
	protected class ModuleManagerConstructor: GameManager.ObjectConstructor
	{	
		int spawnIndex;
		GameManager.ObjectConstructor[] children;

		float[] position;
		float[] scale;
		float rotation;

		public ModuleManagerConstructor(ModuleManager mm)
		{
			spawnIndex = mm.spawnIndex;
			position = Vec3Encode (mm.transform.position);
			scale = Vec3Encode (mm.transform.localScale);
			rotation = mm.transform.rotation.eulerAngles.z;
			children = new GameManager.ObjectConstructor[mm.indexedModules.Count];
			for (int i = 0; i < children.Length; i++) 
			{children [i] = mm.indexedModules [i].main.Save ();}
		}
		public override GameObject Load ()
		{
			GameObject obj = GameManager.singleton.SpawnNew (spawnIndex);
			obj.transform.position = Vec3Parse (position);
			obj.transform.localScale = Vec3Parse (scale);
			obj.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, rotation));
			// ^^ make these inherited ^^
			for (int i = 0; i < children.Length; i++) 
			{
				GameObject child = children [i].Load ();
				child.SendMessage ("AddModule", new object[]{(object)obj.transform});
			}
			return obj;
		}
	}
	void OnDestroy()
	{
		//GameManager.singleton.SaveEvent -= Save;
	}
	#endregion
}