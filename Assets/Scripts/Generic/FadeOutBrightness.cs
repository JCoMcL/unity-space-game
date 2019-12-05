using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutBrightness : FadeOut 
{
	public SpriteRenderer rend;
	protected override void Start ()
	{
		base.Start ();
		if (rend == null) {rend = GetComponent<SpriteRenderer> ();}
	}
	protected override void FadeUpdate (float opacity)
	{
		rend.color = new Color (opacity, opacity, opacity);
	}
}
