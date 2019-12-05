using System.Collections;

using UnityEngine;

public class BirthEmitter : MonoBehaviour {
	public GameObject prefab;
	void Awake () 
	{
		Instantiate (prefab,transform.position,transform.rotation);
	}
}
