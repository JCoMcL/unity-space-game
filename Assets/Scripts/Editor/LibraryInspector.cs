using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Library))]
public class LibraryInspector : Editor 
{
	Dictionary<Library.SubLibrary, SubLibraryInspector> inspectors = new Dictionary<Library.SubLibrary, SubLibraryInspector>();
	public override void OnInspectorGUI()
	{
		Library library = ((Library)target);
		if(GUILayout.Button("Wipe All"))
		{
			library.Wipe();
			inspectors = new Dictionary<Library.SubLibrary, SubLibraryInspector>();
			Debug.ClearDeveloperConsole();	
		}
		Library.SubLibrary sl = library.Initiate();
		try
		{
			SubLibraryInspector sli = new SubLibraryInspector(sl, inspectors);
			inspectors.Add(sl,sli);
			sli.UpdateContents();
			sli.Display();

		}catch{
			//Debug.Log(inspectors.Count);
			inspectors[sl].Display();
		}
	}

	class SubLibraryInspector
	{
		protected Library.SubLibrary targetSL;
		protected bool active;
		protected Dictionary<Library.SubLibrary,SubLibraryInspector> inspectors;
		List<SubLibraryInspector> contents = new List<SubLibraryInspector>();

		public SubLibraryInspector(
			Library.SubLibrary initTargetSL,
		    Dictionary<Library.SubLibrary, SubLibraryInspector> initInspectors
		){
			targetSL = initTargetSL;
			inspectors = initInspectors;
		}
		public void Display()
		{
			GUILayout.BeginHorizontal();
				GUILayout.Space(targetSL.depth*10);
				if (GUILayout.Button(active ? "-" : "+",GUILayout.ExpandWidth(false)))
					{active = !active;}
				targetSL.name = GUILayout.TextField(targetSL.name);
			GUILayout.EndHorizontal();
			if(active){DisplayContents();}
		}
		void NewLibrary(string title)
		{
			Library.SubLibrary sl = targetSL.NewLibrary(title);
			SubLibraryInspector sli =new SubLibraryInspector(sl, inspectors);
			inspectors.Add(sl,sli);
			contents.Add(sli);
		}
		void NewGameObject(GameObject go)//delegate?
		{
			Library.SubLibrary sl = targetSL.NewGameObject(go);
			SubLibraryInspector sli = new SubLibraryTerminalInspector(sl, inspectors);
			inspectors.Add(sl, sli);
			contents.Add(sli);
		}
		protected virtual void DisplayContents()
		{
			foreach (SubLibraryInspector sli in contents)
			{ sli.Display(); }
			GUILayout.BeginHorizontal();

			GUILayout.Space((targetSL.depth+1) * 10);

			string newTitle = GUILayout.TextField("");
			if (newTitle != ""){NewLibrary(newTitle);}

			GameObject newObj = (GameObject)EditorGUILayout.ObjectField(null, typeof(GameObject), false);
			if(newObj != null){NewGameObject(newObj);}

			GUILayout.EndHorizontal();
		}
		public void UpdateContents()
		{
			Library.SubLibrary[] slAr = targetSL.GetContents();
			List<SubLibraryInspector> sliL = new List<SubLibraryInspector>();
			for (int i = 0; i < slAr.Length; i++)
			{sliL.Add(inspectors[slAr[i]]);}
			contents = sliL;
		}
	}
	class SubLibraryTerminalInspector : SubLibraryInspector
	{
		public SubLibraryTerminalInspector(
			Library.SubLibrary initTargetSL,
			Dictionary<Library.SubLibrary, SubLibraryInspector> initInspectors
		) : base(initTargetSL,initInspectors){}

		protected override void DisplayContents()
		{
			GameObject[] contents = (targetSL).GetBranchContents();
			foreach (GameObject go in contents) 
			{
				GUILayout.BeginHorizontal();
				GUILayout.Space((targetSL.depth + 1) * 10);
				EditorGUILayout.ObjectField(go, typeof(GameObject), false);
				GUILayout.EndHorizontal();
			}
			GUILayout.BeginHorizontal();
			GUILayout.Space((targetSL.depth + 1) * 10);
			GameObject newObj = (GameObject)EditorGUILayout.ObjectField(null, typeof(GameObject), false);
			if (newObj != null) { targetSL.NewGameObject(newObj); }
			GUILayout.EndHorizontal();
		}
	}
}
