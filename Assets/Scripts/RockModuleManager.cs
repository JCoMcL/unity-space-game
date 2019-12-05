using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockModuleManager : ModuleManager 
{
	public GameObject Asteroid;
	public GameObject block;
	HashSet<Vector2> occupied = new HashSet<Vector2> ();
	List<Vector2> borders = new List<Vector2>();
	protected override void Start ()
	{
		base.Start ();
		if (block != null) {
			Generate ();//fix later
		}
	}
	public override void DetachModule (Module module)
	{
		Queue<Module> toRemove = new Queue<Module> ();
		List<List<Module>> toSeperate = new List<List<Module>> ();

		BranchManager manager = new BranchManager (width, height,centre,module);
		foreach (List<Module> segment in manager.segments) 
		{	
			if (segment.Count == 1) 
				{toRemove.Enqueue (segment[0]);}
			else toSeperate.Add (segment);
		}
		toRemove.Enqueue (module);

		while (toRemove.Count > 0)
		{
			Module removedModule = toRemove.Dequeue ();
			RemoveModule(removedModule);
			removedModule.Detach (module, false);
		}
		toSeperate.Sort((p1,p2)=>p1.Count.CompareTo(p2.Count));
		for(int i = 0; i < toSeperate.Count-1; i++)
		{
			List<Module> segment = toSeperate [i];
			Transform t = segment [0].transform;
			GameObject subStroid = Instantiate (Asteroid, t.position, t.rotation);
			foreach (Module m in segment) 
			{
				RemoveModule (m);
				Vector3 localPos = subStroid.transform.InverseTransformPoint (m.transform.position);
				Vector2 gridLocation = new Vector2 (Mathf.Round (localPos [0]), Mathf.Round (localPos [1]));
				m.SendMessage ("AddModule",new object[]{subStroid.transform,gridLocation});
			}
		}
	}
	public void Generate ()
	{
		occupied.Add (Vector2.zero);
		GetOutline (Vector2.zero);
		while (Random.value < 0.98) 
		{
			int index = Random.Range (0,borders.Count);
			Vector2 loc = borders[index];
			occupied.Add (loc);
			borders.Remove (loc);
			Instantiate (block).SendMessage ("AddModule",new object[]{transform, loc});
			GetOutline (loc);
		}

	}
	void GetOutline(Vector2 centre)
	{
		Vector2 loc = centre + Vector2.right;
		if (!occupied.Contains (loc) && !borders.Contains (loc)) 
		{borders.Add (loc);}
		loc = centre + Vector2.up;
		if (!occupied.Contains (loc) && !borders.Contains (loc)) 
		{borders.Add (loc);}
		loc = centre + Vector2.left;
		if (!occupied.Contains (loc) && !borders.Contains (loc)) 
		{borders.Add (loc);}
		loc = centre + Vector2.down;
		if (!occupied.Contains (loc) && !borders.Contains (loc)) 
		{borders.Add (loc);}
	}

	class BranchManager
	{
		public Vector2 centre;
		public List<Module>[] segments; 
		int[,] map;
		Dictionary<int, Branch> branchDictionary = new Dictionary<int, Branch>();
		List<Branch> branchList = new List<Branch> ();

		public BranchManager(int width,int height, Vector2 center, Module module)
		{
			map = new int[width,height];
			centre = center;
			SetMapFlag (module.gridLocation+centre,-1);
			int count = 0;
			for (int i = 0; i < 4; i++) 
			{
				Module baseRelation = module.relations [i];
				if (baseRelation != null) 
				{
					Branch branch = new Branch(i+1,baseRelation,this);
					branchDictionary.Add (branch.id,branch);
					branchList.Add (branch);
					count ++;
				}
			}
			for(int i = 0; i <count;)
			{
				for(int a = 0; a<branchList.Count; a++)
				{
					i+=branchList[a].Iterate ();
				}
				if(branchList.Count == 1){break;}
			}
			segments = new List<Module>[branchList.Count];
			for(int i = 0; i<segments.Length; i++)
			{segments[i] = branchList[i].foundModules;}
		}
		public void MergeBranches(Branch merger, int index)
		{
			branchDictionary.Remove (merger.id);
			branchList.Remove (merger);
			merger.Merge (branchDictionary[index]);
		}
		public int GetMapFlag(Vector2 coords)
			{return map [(int)coords [0], (int)coords [1]];}
		public void SetMapFlag(Vector2 coords, int value)			
			{map [(int)coords [0], (int)coords [1]] = value;}

		public class Branch
		{
			public Queue<Module> queue = new Queue<Module>();
			public List<Vector2> foundLocations = new List<Vector2>();
			public List<Module> foundModules = new List<Module> ();
			public int id;
			Vector2 mapLoc;
			BranchManager manager;

			public Branch(int uid, Module root, BranchManager branchManager)
			{
				id = uid;
				manager = branchManager;
				queue.Enqueue (root);
				foundModules.Add (root);
				foundLocations.Add (root.gridLocation+manager.centre);
				manager.SetMapFlag (foundLocations[0],id);
			}
			public int Iterate()
			{	if (queue.Count > 0) 
			{
				Module current = queue.Dequeue ();
				int mergee = 0;
				foreach (Module relation in current.relations){	if (relation != null) 
				{
					mapLoc = relation.gridLocation + manager.centre;
					int flag = manager.GetMapFlag (mapLoc);
					if (flag != -1 && flag != id) 	
					{
						if (flag == 0) 
						{
							manager.SetMapFlag (mapLoc,id);
							foundModules.Add (relation);
							foundLocations.Add (mapLoc);
							queue.Enqueue (relation);
						}
						else{mergee = flag;}
					}
				}}
				if (mergee > 0) {manager.MergeBranches (this,mergee);return 1;}
				if(queue.Count == 0){return 1;}
				return 0;
			}else return 0;
			}
			public void Merge (Branch mergee)
			{
				foreach (Vector2 vec in foundLocations) 
				{
					mergee.foundLocations.Add (vec);
					manager.SetMapFlag (vec,mergee.id);
				}
				foundLocations.Clear ();
				foreach (Module mod in foundModules) 
				{	mergee.foundModules.Add (mod);}
				foundModules.Clear ();
				foreach (Module mod in queue) 
				{	if (!mergee.queue.Contains (mod)) 
					{mergee.queue.Enqueue (mod);}
				}
				queue.Clear ();

			}
		}
	}

	public override GameManager.ObjectConstructor Save ()
	{
		return new ModuleManagerConstructor(this);
	}

}
