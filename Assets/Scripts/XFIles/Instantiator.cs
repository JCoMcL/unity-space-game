using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiator : MonoBehaviour {
	public GameObject obj;
	void Update () {
		if (Input.GetKey (KeyCode.Space)) {
			for (float f = 1; f <= 2; f += 0.1f) 
			{
				obj.transform.localScale = Vector3.one * f;
				Instantiate (obj);
			}
		}
	}
}
