using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flares : MonoBehaviour 
{
	public static Flares singleton;
	public delegate bool Condition();

	public AnimationCurve intensityNoise;
	public AnimationCurve HueNoise;

	FlareData[] presets = new FlareData[1];

	PoolChildManager manager;

	void Start () 
	{
		manager = GetComponent<PoolChildManager> ();
		singleton = this;
		FlareData fd = new FlareData (singleton);
		presets [0] = fd;
	}
	public void Flicker(Vector3 position, float intensity, Color flareColor, float size = 1)
	{
		if (manager == null) {manager = GetComponent<PoolChildManager> ();}
		ImageChild image = (ImageChild)manager.GetAvailableChild ();
		//IEnumerator flicker = MaintainFlicker (flickerTransform, image);
		//StartCoroutine (flicker);
		flareColor = presets[0].NoiseColor (flareColor);
		float remainerIntensity = flareColor.grayscale + intensity - 1;
		if(remainerIntensity > 0)	{image.SetSize (size+remainerIntensity);}
		flareColor *= new Color (intensity, intensity, intensity);
		image.SetPosition (Conversion.MetresToPixels (position));
		image.SetColor (flareColor);
	}
	IEnumerator MaintainFlicker(Transform source, ImageChild image)
	{
		/*IEnumerator minor = MinorNoise(ld);
		StartCoroutine (minor);
		IEnumerator major = MajorNoise(source, destination, ld);
		StartCoroutine (major);
		while (condition()) 
		{
			ld.positions [0] = source.position;
			ld.positions [ld.positions.Length - 1] = destination.position;
			ld.Render ();
			yield return null;
		}
		StopCoroutine (minor);
		StopCoroutine (major);
		ld.renderer.reserved = false;*/
		while (true) 
		{
			//image.SetPosition (Conversion.MetresToPixels (source.position));
			//image.SetColor (fd.NoiseColor ());
			yield return new WaitForEndOfFrame ();
		}
	}
	class FlareData
	{
		Flares singleton;
		public AnimationCurve intensityNoise;
		public AnimationCurve HueNoise;

		public FlareData(Flares flares)
		{
			singleton = flares;
			intensityNoise = singleton.intensityNoise;
			HueNoise = singleton.HueNoise;
		}
		public Color NoiseColor(Color color)
		{
			float r = color.r;
			float g = color.g;
			float b = color.b;

			float noise = intensityNoise.Evaluate(Random.value) * color.grayscale;
			r += noise;
			g += noise;
			b += noise;

			r += HueNoise.Evaluate(Random.value);
			g += HueNoise.Evaluate(Random.value);
			b += HueNoise.Evaluate(Random.value);

			return new Color (r, g, b);
		}
	}
}
