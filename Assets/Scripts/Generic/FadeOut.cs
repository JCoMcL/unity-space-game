using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : DestroyByTime {
	[Range(0.0001f,1f)]public float fadeStart;
	float fade;
	float factor;

	protected virtual void Start () 
	{
		factor = 1 / (1 - fadeStart);
	}
	protected override void Update () 
	{
		base.Update ();
		if (lifeProgress >= fadeStart) 
		{
			fade = (lifeProgress - fadeStart) * factor;
			FadeUpdate (1 - fade);
		}
	}
	protected virtual void FadeUpdate(float opacity)
	{
		
	}
}
