using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Library : MonoBehaviour
{
	//SubLibrary[] sub.Libraries = new SubLibrary[1];
	//[SerializeField]
	public class ArContainer
		{public SubLibrary[] Libraries = new SubLibrary[1];}
	ArContainer sub = new ArContainer();
	public SubLibrary Initiate()
	{
		if (sub.Libraries[0] != null)
		{return sub.Libraries[0];}
		SubLibrary sl = new SubLibrary("Main", 0, sub);
		sub.Libraries[0] = sl;
		return sl;
	}
	public void Wipe()
	{
		sub.Libraries = new SubLibrary[1];
	}
	public GameObject GetObjectByID(int id)
	{
		return sub.Libraries[0].GetBranchContents()[id]; //optimise
	}
	public GameObject GetObjectByName(string name)
	{
		GameObject[] goAr = sub.Libraries[0].GetBranchContents();
		foreach(GameObject go in goAr)
		{ if (go.name == name) 
			{return go;}
		}
		Debug.LogError("No item in Library with name "+name);
		return null;
	}

	public class SubLibrary
    {
		public string name;
		public bool terminal;
		public int depth;
		List<SubLibrary> contents = new List<SubLibrary>();
		public ArContainer sub;

		public SubLibrary(string initName, int initDepth, ArContainer initsub)
		{
			name = initName;
			depth = initDepth;
			sub = initsub;
		}
		public virtual SubLibrary[] GetContents()
			{return terminal ? null : contents.ToArray();}
		public SubLibrary NewLibrary(string initName = "")
		{
			SubLibrary sl = new SubLibrary(
				initName == "" ? "directory" + sub.Libraries.Length : initName, 
				depth + 1, 
				sub
			);
			AddLibrary(sl);
			return sl;
		}
		public virtual SubLibrary NewGameObject(GameObject initObject, string initName = "")
		{
			SubLibrary sl = new SubLibraryTerminal(
				initName == "" ? "directory" + sub.Libraries.Length : initName,
				sub,
				depth+1,
				initObject
			);
			AddLibrary(sl);
			return sl;
		}
		void AddLibrary(SubLibrary sl)
		{
			int index = -1;
			SubLibrary slp = GetLastItem();
			SubLibrary[] ar = new SubLibrary[sub.Libraries.Length + 1];
			for (int i = 0; i < sub.Libraries.Length && index == -1; i++)
			{
				ar[i] = sub.Libraries[i];
				if (sub.Libraries[i] == slp)
				{
					index = i + 1;
					ar[index] = sl;
				}
			}
			for (int i = index; i < sub.Libraries.Length; i++)
			{
				ar[i + 1] =
			   sub.Libraries[i];
			}
			sub.Libraries = ar;
			contents.Add(sl);
		}
		public SubLibrary GetLastItem()
		{
			if (contents.Count == 0) { return this; }
			return contents[contents.Count - 1].terminal ? 
				 contents[contents.Count - 1] : 
				 contents[contents.Count - 1].GetLastItem();
		}
		public virtual GameObject[] GetBranchContents()
        {
            List<GameObject> branchContents = new List<GameObject>();
            for (int i = 0; i < contents.Count; i++)
            {
                GameObject[] branchContent = contents[i].GetBranchContents();
                foreach(GameObject go in branchContent)
                    {branchContents.Add(go);}
            }
            return branchContents.ToArray();
        }
    }
    public class SubLibraryTerminal : SubLibrary
    {
		List<GameObject> contents = new List<GameObject>();

		public SubLibraryTerminal(string initName, ArContainer initsub, int initDepth, GameObject initObject) 
			: base(initName, initDepth, initsub)
		{
			terminal = true;
			contents.Add(initObject);
		}
		public override SubLibrary NewGameObject(GameObject initObject, string initName = "")
		{
			contents.Add(initObject);
			return null;
		}
        public override GameObject[] GetBranchContents()
		{
			return contents.ToArray();
		}
    }
}
