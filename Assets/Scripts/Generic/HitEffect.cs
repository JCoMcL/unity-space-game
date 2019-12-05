using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour {

	public GameObject effect;
	void Start () 
	{
		if (effect == null) {print ("error: no effect asssigned");}
	}
	void Hit()
	{
		Instantiate (effect,transform.TransformPoint (new Vector2 (0,0.2f)),transform.rotation);
	}
}
