using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VoxelGrow : MonoBehaviour {

	public GameObject pickup;

	int width;
	int height;
	Vector2 centre;

	Vector2 removeCentre;

	BoxCollider2D baseCollider;

	Dictionary<BoxCollider2D,OldModule> colliderDictionary = new Dictionary<BoxCollider2D,OldModule> ();
	Dictionary<Vector2, OldModule> gridDictionary = new Dictionary<Vector2, OldModule> ();

	List<Vector3> vertices;
	List<int> triangles;
	List<Vector2> UVs;
	List<OldModule> indexedOldModules = new List<OldModule> ();

	//OldModule debugOldModule;
	OldModule[] bounds = new OldModule[4];

	void Start ()
	{
		colliderDictionary.Clear ();
		gridDictionary.Clear ();

		vertices = new List<Vector3>();
		triangles = new List<int>();
		UVs = new List<Vector2> ();

		vertices.Add (new Vector3(0.5f,-0.5f,0));
		vertices.Add (new Vector3(0.5f,0.5f,0));
		vertices.Add (new Vector3(-0.5f,0.5f,0));
		vertices.Add (new Vector3(-0.5f,-0.5f,0));

		int[] square = new int[4];
		for (int i = 0; i < 4; i++) {square [i] = i;}

		BoxCollider2D coll = gameObject.AddComponent<BoxCollider2D> ();
		OldModule core = new OldModule (Vector2.zero, 1, square, this.gameObject, ref coll,pickup);
		core.baconNumber = 0;
		core.relations = new OldModule[4];
		indexedOldModules.Add (core);
		core.index = 0;

		foreach (Vector2 uv in core.GetUV()) {UVs.Add (uv);}
			
		colliderDictionary.Add (core.collider, core);
		gridDictionary.Add (core.gridLocation, core);

		for (int i = 0; i < 4; i++) {bounds [i] = core;}

		width = 1;
		height = 1;
		centre = Vector2.zero;

		triangles.Add (2);
		triangles.Add (1);
		triangles.Add (0);

		triangles.Add (3);
		triangles.Add (2);
		triangles.Add (0);

		GenerateMesh ();
	}
	void GenerateMesh()
	{
		Mesh mesh = new Mesh ();
		GetComponent<MeshFilter> ().mesh = mesh;
		mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles.ToArray();
		mesh.uv = UVs.ToArray ();

		width = (int)(bounds[0].gridLocation[0] - bounds[2].gridLocation[0]) + 1;
		height = (int)(bounds[1].gridLocation[1] - bounds[3].gridLocation[1]) + 1;
		centre = new Vector2 (bounds [2].gridLocation [0] * -1, bounds[3].gridLocation[1] * -1);
	}
	void AddOldModule(object[] package)
	{
		BoxCollider2D rootCollider = (BoxCollider2D)package [0];
		int[] rootVerts = colliderDictionary [rootCollider].vertReference;		
		Vector2 rootLoctaion = colliderDictionary [rootCollider].gridLocation;
		int direction = (int)package [1];
		Vector2 transform = Vector2.right;
		Vector2 gridLocation = Vector2.zero;
		bool outOfBounds = false;
		int se = 0;
		int ne = 0;
		int nw = 0;
		int sw = 0;
		switch (direction) {
		case 0:
			vertices.Add (vertices [rootVerts [0]] + Vector3.right);
			se = vertices.Count - 1;
			vertices.Add (vertices [rootVerts [1]] + Vector3.right);
			ne = vertices.Count - 1;
			vertices.Add (vertices [rootVerts [1]]);
			nw = vertices.Count - 1;
			vertices.Add (vertices [rootVerts [0]]);
			sw = vertices.Count - 1;
			gridLocation = rootLoctaion + transform;
			if (bounds [0].gridLocation [0] < gridLocation [0]) {
				outOfBounds = true;
			}
			break;
		case 1:
			transform = Vector2.up;
			vertices.Add (vertices [rootVerts [1]]);
			se = vertices.Count - 1;
			vertices.Add (vertices [rootVerts [1]] + Vector3.up);
			ne = vertices.Count - 1;
			vertices.Add (vertices [rootVerts [2]] + Vector3.up);
			nw = vertices.Count - 1;
			vertices.Add (vertices [rootVerts [2]]);
			sw = vertices.Count - 1;
			gridLocation = rootLoctaion + transform;
			if (bounds [1].gridLocation [1] < gridLocation [1]) {
				outOfBounds = true;
			}
			break;
		case 2: 
			transform = Vector2.left;
			vertices.Add (vertices [rootVerts [3]]);
			se = vertices.Count - 1;
			vertices.Add (vertices [rootVerts [2]]);
			ne = vertices.Count - 1;
			vertices.Add (vertices [rootVerts [2]] + Vector3.left);
			nw = vertices.Count - 1;
			vertices.Add (vertices [rootVerts [3]] + Vector3.left);
			sw = vertices.Count - 1;
			gridLocation = rootLoctaion + transform;
			if (bounds [2].gridLocation [0] > gridLocation [0]) {
				outOfBounds = true;
			}
			break;
		case 3: 
			transform = Vector2.down;
			vertices.Add (vertices [rootVerts [0]] + Vector3.down);
			se = vertices.Count - 1;
			vertices.Add (vertices [rootVerts [0]]);
			ne = vertices.Count - 1;
			vertices.Add (vertices [rootVerts [3]]);
			nw = vertices.Count - 1;
			vertices.Add (vertices [rootVerts [3]] + Vector3.down);
			sw = vertices.Count - 1;
			gridLocation = rootLoctaion + transform;
			if (bounds [3].gridLocation [1] > gridLocation [1]) {
				outOfBounds = true;
			}
			break;
		}
		int[] square = new int[4];
		square [0] = se;
		square [1] = ne;
		square [2] = nw;
		square [3] = sw;

		BoxCollider2D coll = gameObject.AddComponent<BoxCollider2D> ();
		OldModule module = new OldModule (rootLoctaion + transform, direction, square, this.gameObject, ref coll,(GameObject)package[2]);
		indexedOldModules.Add (module);
		module.index = indexedOldModules.IndexOf(module);
		AddOldModuleRelations (module);

		foreach (Vector2 uv in module.GetUV ()) {UVs.Add (uv);}




		colliderDictionary.Add (module.collider, module);
		gridDictionary.Add (module.gridLocation, module);

		if (outOfBounds) {
			bounds [direction] = module;
		}

		triangles.Add (nw);
		triangles.Add (ne);
		triangles.Add (se);

		triangles.Add (nw);
		triangles.Add (se);
		triangles.Add (sw);

		GenerateMesh ();
	}
	void AddOldModuleRelations(OldModule module)
	{
		module.relations = new OldModule[4];
		OldModule relative;
		if (gridDictionary.ContainsKey (module.gridLocation + Vector2.right)) 
		{
			relative = gridDictionary [module.gridLocation + Vector2.right];
			module.relations[0] = relative;
			relative.relations[2] = module;
			if (module.baconNumber == -1 || relative.baconNumber < module.baconNumber) 
			{module.baconNumber = relative.baconNumber;}
		}
		if (gridDictionary.ContainsKey (module.gridLocation + Vector2.up)) 
		{
			relative = gridDictionary [module.gridLocation + Vector2.up];
			module.relations[1] = relative;
			relative.relations[3] = module;
			if (module.baconNumber == -1 || relative.baconNumber < module.baconNumber) 
			{module.baconNumber = relative.baconNumber;}
		}
		if (gridDictionary.ContainsKey (module.gridLocation + Vector2.left)) 
		{
			relative = gridDictionary [module.gridLocation + Vector2.left];
			module.relations[2] = relative;
			relative.relations[0] = module;
			if (module.baconNumber == -1 || relative.baconNumber < module.baconNumber) 
			{module.baconNumber = relative.baconNumber;}
		}
		if (gridDictionary.ContainsKey (module.gridLocation + Vector2.down)) 
		{		
			relative = gridDictionary [module.gridLocation + Vector2.down];
			module.relations[3] = relative;
			relative.relations[1] = module;
			if (module.baconNumber == -1 || relative.baconNumber < module.baconNumber) 
			{module.baconNumber = relative.baconNumber;}
		}
		module.baconNumber++;
		foreach (OldModule neighbour in module.relations){	if (neighbour != null && neighbour.baconNumber > module.baconNumber + 1) 
			{neighbour.UpdateBaconNumber (module.baconNumber + 1);}}
	}
	/*void AddOldModuleCornerRelations(OldModule module)
	{
		module.cornerRelations = new OldModule[4];
		if (gridDictionary.ContainsKey (module.gridLocation + new Vector2 (1, -1))) 
		{module.cornerRelations [0] = gridDictionary [module.gridLocation + new Vector2 (1, -1)];}
		if (gridDictionary.ContainsKey (module.gridLocation + new Vector2 (1, 1))) 
		{module.cornerRelations [1] = gridDictionary [module.gridLocation + new Vector2 (1, 1)];}
		if (gridDictionary.ContainsKey (module.gridLocation + new Vector2 (-1, 1))) 
		{module.cornerRelations [2] = gridDictionary [module.gridLocation + new Vector2 (-1, 1)];}
		if (gridDictionary.ContainsKey (module.gridLocation + new Vector2 (-1, -1))) 
		{module.cornerRelations [3] = gridDictionary [module.gridLocation + new Vector2 (-1, -1)];}
		/*for (int i = 0; i < 4; i++) 
			{
				if (relations [i] != null) 
				{
					OldModule neighbour = relations [i];
					if(neighbour.relations[Conversion.RoundInt(i-1)] != null)
						{cornerRelations [i] = neighbour.relations [Conversion.RoundInt (i - 1)];}
					if(neighbour.relations[Conversion.RoundInt(i+1)] != null)
						{cornerRelations[i + 1] = neighbour.relations [Conversion.RoundInt (i + 1)];}
				}
			}
	}*/
	void RemoveOldModule(OldModule module)
	{
		OldModule first = module;
		Queue<OldModule> toRemove = new Queue<OldModule> ();
		int[,] map = new int[width,height];
		int baconNumber = module.baconNumber;
		Vector2 mapLoc = module.gridLocation + centre;

		map [(int)mapLoc[0],(int)mapLoc[1]] = 1;
		toRemove.Enqueue (module);
		foreach(OldModule baseRelation in module.relations){	if (baseRelation != null && baseRelation.baconNumber > baconNumber) 
		{
			List<OldModule> foundOldModules = new List<OldModule> ();
			List<Vector2> foundLocations = new List<Vector2> ();
			Queue<OldModule> queue = new Queue<OldModule> ();

			mapLoc = baseRelation.gridLocation + centre;
			map [(int)mapLoc[0],(int)mapLoc[1]] = 1;
			foundOldModules.Add (baseRelation);
			foundLocations.Add (mapLoc);
			queue.Enqueue (baseRelation);

			while (queue.Count > 0) 
			{
				OldModule current = queue.Dequeue ();
				foreach (OldModule relation in current.relations){	if (relation != null) 
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
									foundOldModules.Clear ();
							break;
						}
						foundLocations.Add (mapLoc);
						if (relation.baconNumber <= baconNumber) 
						{
							foreach(Vector2 loc in foundLocations)
								{map [(int)loc[0],(int)loc[1]] = 2;}
							queue.Clear ();
									foundOldModules.Clear ();
							break;
						}
						map [(int)mapLoc [0], (int)mapLoc [1]] = 1;
								foundOldModules.Add (relation);
						queue.Enqueue (relation);

					}
				}}
			}
			foreach (OldModule foundOldModule in foundOldModules) 
				{toRemove.Enqueue (foundOldModule);}
		}}
		while (toRemove.Count > 0)
		{
			OldModule removedOldModule = toRemove.Dequeue();
			bool last = toRemove.Count == 1;
			//AddOldModuleCornerRelations (removedOldModule);

			triangles.RemoveRange (removedOldModule.index * 6 , 6);
			RemoveVertices(removedOldModule.Remove(first,last));
			RemoveIndex (removedOldModule.index);
			gridDictionary.Remove (removedOldModule.gridLocation);
			colliderDictionary.Remove(removedOldModule.collider);
			Destroy (removedOldModule.collider);
		}
		GenerateMesh ();
	}
	void RemoveVertices(int[] verticesToRemove)
	{
		foreach (int vertex in verticesToRemove) if(vertex != -1){
		{
			vertices.RemoveAt (vertex);
			UVs.RemoveAt (vertex);
			for (int i = 0; i < triangles.Count; i++) {if (triangles [i] >= vertex) 
				{triangles [i]--;}
			}
			foreach (OldModule module in indexedOldModules) 
				{module.UpdateVertices (vertex);}
		}}
	}
	void RemoveIndex(int index)
	{
		indexedOldModules.RemoveAt (index);
		for (int i = index; i < indexedOldModules.Count; i++)
		{indexedOldModules [i].index--;}
	}
	//void ReleasePickup(Vector2 gridLocation,int gridRotation, bool dragging)

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
				{RemoveOldModule(gridDictionary [GetLocalMouse()]);}
		}
	}
	/*void OnDrawGizmos() 
	{
		Component[] colliders;
		colliders = GetComponents(typeof(BoxCollider2D));
		Vector2 topleft = new Vector2 (-0.5f, 0.5f);
		foreach(BoxCollider2D box in colliders)
		{
			int[] boundsDebug = colliderDictionary[box].vertReference;
			if (colliderDictionary.ContainsKey(box))
			{Handles.Label (transform.TransformPoint (box.offset + topleft), colliderDictionary[box].baconNumber
				+ "\n" + boundsDebug[0]+" "+boundsDebug[1]+" "+boundsDebug[2]+" "+boundsDebug[3]);}

		}/*
		Vector3 moduleLocation = transform.TransformPoint (debugOldModule.gridLocation);
		foreach (OldModule relation in debugOldModule.relations) 
		{
			if (relation != null) 
			{Gizmos.DrawLine (moduleLocation, transform.TransformPoint (relation.gridLocation));}
		}
	} */ 
	void PrintArray()
	{
		string message = "";
		foreach (int vert in triangles) {
			message += vert + ", ";
		}
		print (message);
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	class OldModule
	{
		public GameObject pickup;
		public GameObject parentObject;
		public BoxCollider2D collider;
		public Vector2 gridLocation;
		public int gridRotation;
		public int[] vertReference;
		public OldModule[] relations;
		//public OldModule[] cornerRelations;
		public int baconNumber = -1;
		public int index;
		public int moduleCount = 2000;//this is a bad idea

		public OldModule(Vector2 pos, int rot, int[] verts, GameObject initiator, ref BoxCollider2D coll, GameObject source)
		{
			gridLocation = pos;
			gridRotation = rot;
			vertReference = verts; 

			collider = coll;
			collider.size = Vector2.one;
			collider.offset = gridLocation;

			pickup = source;
			parentObject = initiator;
		}
		public Vector2[] GetUV()
		{
			Vector2[] uvMap = new Vector2[4];
			uvMap [0] = new Vector2 (0.25f, 0);
			uvMap [1] = new Vector2 (0.25f, 0.25f);
			uvMap [2] = new Vector2 (0, 0.25f);
			uvMap [3] = new Vector2 (0, 0);
			return uvMap;
		}
		public void UpdateBaconNumber(int number)
		{
			baconNumber = number;
			foreach (OldModule neighbour in relations){	if (neighbour != null && neighbour.baconNumber > number + 1) 
				{neighbour.UpdateBaconNumber (number + 1);}}
		}
		public void BaconCrawl(ref int number, ref Queue<OldModule> queue)
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
		public void UpdateVertices(int vertex)
		{
			for(int i = 0; i < 4; i++)
			{if(vertReference[i] > vertex){vertReference[i]--;}}
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
			int number = baconNumber;
			Queue<OldModule> queue = new Queue<OldModule> ();
			if (baconSource) 
			{
				baconNumber = moduleCount;
				BaconCrawl (ref number,ref queue);
			}
		}
		public void ReleasePickup(OldModule first)
		{
			//GameObject newPickup = Instantiate (pickup);
			CtrlDrag script = pickup.GetComponent<CtrlDrag> ();
			pickup.transform.position = parentObject.transform.TransformPoint (new Vector3 (gridLocation [0], gridLocation [1], 0));
			pickup.transform.localEulerAngles = new Vector3(0,0,parentObject.transform.eulerAngles.z + gridRotation * 90 - 90);
			pickup.SetActive (true);
			if (first == this) 
			{	script.PickUp ();} 
			else {
				float force = Mathf.Sqrt (Vector2.SqrMagnitude ((gridLocation - first.gridLocation) * 20));
				float angle = Conversion.DegToRad (pickup.transform.localEulerAngles.z + 90);
				pickup.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (Mathf.Cos (angle), Mathf.Sin (angle) * -1) * force);
			}

		}
		public int[] Remove(OldModule first,bool last)
		{
			for (int i = 0;i < 4;i++) 
			{
				OldModule neighbour = relations[i];
				if (neighbour != null) 
					{neighbour.RemoveRelation (Conversion.FlipDirection(i),last);}
			}
			ReleasePickup (first);
			return vertReference;
		}
	}

}
