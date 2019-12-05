using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnTest : MonoBehaviour 
{
	[SerializeField]
	AbsUIContentPanel loadMenu;
	[SerializeField]
	GameObject buttonPrefab;
	GameObject asteroid;
	public void NewAsteroid()
	{
		Delete ();
		asteroid = GameManager.singleton.SpawnNew(6);
		asteroid.transform.position = transform.position;
		asteroid.transform.rotation = transform.rotation;
	}
	public void SaveAsteroid(string name)
	{
		int itemCount = GameManager.singleton.LoadMulti ("Test").Length;
		GameManager.singleton.Save (
			asteroid.GetComponent<IPersistable<GameManager.ObjectConstructor>> (),
			"Test/" + name + ".dat"
		);
	}
	public void LoadAsteroid(string name)
	{
		GameManager.ObjectConstructor rock = GameManager.singleton.Load ("Test/" + name +".dat");
		Delete ();
		if (rock != null) 
			{asteroid = rock.Load ();}
		else
			{Debug.LogError ("Save is Invalid");}
	}
	public void ShowLoadMenu()
	{
		loadMenu.Show ();
		string[] files = GameManager.singleton.LoadMulti ("Test");
		foreach(string file in files)
		{
			string[] subStrings = file.Split (new char[]{'.'});
			string name = subStrings[0];
			for (int i = 1; i < subStrings.Length - 1; i++) 
				{name += "." + subStrings [i];}
			GameObject button = Instantiate (buttonPrefab);
			button.GetComponentInChildren<Text> ().text = name;
			Button buttonComponent = button.GetComponent<Button> ();
			buttonComponent.onClick.AddListener (delegate{LoadAsteroid(name);});
			buttonComponent.onClick.AddListener (loadMenu.Hide);
			loadMenu.AddContent (button);
		}
	}
	public void Delete()
	{
		if (asteroid != null) 
			{Destroy (asteroid);}
	}

}
