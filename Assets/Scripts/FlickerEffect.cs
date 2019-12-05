using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerEffect : MonoBehaviour {
	public Color color;
	public Flares flare;
	public float intensity;

	void Start()
		{flare = (Flares)FindObjectOfType (typeof(Flares));}

	void Update () 
		{flare.Flicker (transform.position,intensity,color);}
}
