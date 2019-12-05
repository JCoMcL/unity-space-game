using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour 
{
	public float lifeTime;
	protected float lifeProgress;
	protected float time = 0;

	void Awake(){lifeProgress = 0;}

	protected virtual void Update ()
	{
		time += Time.deltaTime;
		lifeProgress = time / lifeTime;
		if (lifeProgress >= 1) 
			{Destroy (gameObject);}
	}
}
