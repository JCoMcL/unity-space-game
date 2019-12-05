using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System;

public class GameManager : MonoBehaviour 
{
	public static GameManager singleton;
	//public GameObject[] prefabs;
	[SerializeField]
	Library library;
	//GameObject[] prefabs;//Sorted;
	//List<IPersistable<ObjectConstructor>> activeObjects = new List<IPersistable<ObjectConstructor>>();
	void Awake()
	{
		if (GameManager.singleton == null) 
		{
			DontDestroyOnLoad (gameObject);
			GameManager.singleton = this;
		} 
		else if (GameManager.singleton != this) 
		{	Destroy (gameObject);}
	}

#region Data Persistance
	public event Func<ObjectConstructor> SaveEvent;
	public GameObject SpawnNew(int index, string name = "")
	{
		return Instantiate(name=="" ?
			library.GetObjectByID(index) : 
			library.GetObjectByName("name"));
	}
	[System.Serializable]
	public abstract class ObjectConstructor
	{
		public abstract GameObject Load ();

		protected Vector3 Vec3Parse(float[] array)
		{
			Vector3 vec = Vector3.zero;
			for (int i = 0; i < 3 && i < array.Length; i++) 
			{	vec [i] = array [i];}
			return vec;
		}
		protected float[] Vec3Encode(Vector3 vec)
		{	
			return new float[]{ vec [0], vec [1], vec [2] };
		}
		protected Vector2 Vec2Parse(float[] array)
		{
			Vector2 vec = Vector2.zero;
			for (int i = 0; i < 2 && i < array.Length; i++) 
			{	vec [i] = array [i];}
			return vec;
		}
		protected float[] Vec2Encode(Vector2 vec)
		{	
			return new float[]{ vec [0], vec [1]};
		}

	}
	public void SaveAll()
	{
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/Test.dat");
		List<ObjectConstructor> constructors = new List<ObjectConstructor> ();
		constructors.Add (SaveEvent ());
		bf.Serialize (file, new MultiConstructor(constructors.ToArray ()));

		file.Close ();
	}
	public void Save(IPersistable<ObjectConstructor> obj, string path)
	{
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/" + path);
		bf.Serialize (file, obj.Save ());
		file.Close ();
	}
	public ObjectConstructor Load(string path)
	{
		if (File.Exists (Application.persistentDataPath + "/" + path)) 
		{
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath+ "/" + path, FileMode.Open);
			ObjectConstructor data = (ObjectConstructor)bf.Deserialize (file);
			file.Close ();
			return data;
		}
		return null;
	}
	public String[] LoadMulti(string path)
	{
		string fullPath = Application.persistentDataPath + "/" + path;
		string[] files = Directory.GetFiles (fullPath);
		int pathLength = fullPath.Length + 1;
		List<string> validFiles = new List<string> ();
		foreach (string file in files) 
		{
			if (file.EndsWith (".dat")) 
			{validFiles.Add (file.Remove (0,pathLength));}
		}
		return validFiles.ToArray ();
	}

	[System.Serializable]
	class MultiConstructor : ObjectConstructor
	{
		ObjectConstructor[] children;
		public MultiConstructor(ObjectConstructor[] objects)
			{children = objects;}
		public override GameObject Load ()
		{
			for(int i =0; i < children.Length-1; i++)
			{children[i].Load ();}
			return children[children.Length].Load ();
		}
	}
#endregion
}