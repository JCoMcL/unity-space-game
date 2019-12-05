using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour 
{
	public static Lightning singleton;

	public delegate bool Condition();
	public int majorSegments;
	public int minorSegments;
	public float majorRefreshDelay;
	public float majorRefreshDelayNoise;
	[Range(0.0001f,1f)]	public float ratio;
	public float turbulence;
	public float turbulenceNoise;
	public float width;
	public float widthNoise;
	public bool lengthScaling;

	PoolChildManager manager;
	float time;
	bool refresh;

	void Start () 
	{
		manager = GetComponent<PoolChildManager> ();
		singleton = this;
	}
	/*void Update () 
	{
		Vector3 origin = transform.position;
		Vector3 target = Conversion.mouseToMetres2D ();
		refresh = false;
		time += Time.deltaTime;
		if (time >= majorRefreshRate) {refresh = true; time = 0;}
		if (lengthScaling) {
			float distance = Vector3.Distance (origin, target) / 5;
			turb = turblulence * distance;
			turbNoise = turbalanceNoise * distance;
			line.widthMultiplier = width * distance;
		} else 
		{
			turb = turblulence;
			line.widthMultiplier = width;
			turbNoise = turbalanceNoise;
		}
		target = Conversion.mouseToMetres2D ();
		positions [0] = origin;
		positions [positions.Length-1] =  target;

		float minorTurb = turb * ratio;
		float minorNoise = turbNoise * ratio;
		for (int i = 1; i <= majorSegments; i++) 
		{
			if (refresh) 
			{
				Vector3 position = (target - origin) * i / (majorSegments) + origin;
				position += RandomOffset (turb, turbNoise);
				positions [i * minorSegments] = position;
			}
			Vector3 minorOrigin = positions [(i - 1)*minorSegments];
			Vector3 minorTarget = positions [i*minorSegments];
			for (int a = 1; a <= minorSegments; a++) 
			{
				Vector3 minorPosition = (minorTarget-minorOrigin) * a / (minorSegments) + minorOrigin;
				minorPosition += RandomOffset(minorTurb,minorNoise);
				positions [(i - 1)*minorSegments + a] = minorPosition;
			}
		}
		line.positionCount = positions.Length;
		line.SetPositions (positions);
	}*/
	public void Strike(Vector3 origin, Vector3 target)
	{
		LightningData ld = GetPreset (-1);
		ld.SetPath (origin, target);
		ld.Render ();
	}
	public void Tether(Transform source, Transform destination, Condition condition)
	{
		LightningData ld = GetPreset (0);
		StartCoroutine (MaintainTether (source, destination, ld, condition));
		ld = GetPreset (1);
		StartCoroutine (MaintainTether (source, destination, ld, condition));
	}
	Vector3 RandomOffset(float magnitude, float noise)
	{
		float angle = Conversion.DegToRad (Random.Range (1, 360));
		Vector3 offset = new Vector3 (Mathf.Cos (angle), Mathf.Sin (angle), 0);
		offset *= (magnitude + magnitude * (Random.value * noise));
		return offset;
	}
	IEnumerator MaintainTether(Transform source, Transform destination, LightningData ld, Condition condition)
	{
		ld.SetPath (source.position, destination.position);
		IEnumerator minor = MinorNoise(ld);
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
		ld.renderer.reserved = false;
		yield return new WaitForEndOfFrame ();
	}
	IEnumerator MinorNoise(LightningData ld)
	{
		while (true) 
		{
			for (int i = 1; i < ld.positions.Length; i++) 
				{ld.positions[i] += ld.MinorRandomOffset();}
			yield return new WaitForEndOfFrame ();
		}
	}
	IEnumerator MajorNoise(Transform source, Transform destination,  LightningData ld)
	{
		while (true) 
		{
			ld.SetPath(source.position,destination.position);
			yield return new WaitForSeconds(ld.Delay ());
		}
	}
	LightningData GetPreset(int index)
	{
		LineChild renderer;
		LightningData ld;
		switch (index) 
		{
		case 0:
			renderer = (LineChild)manager.GetAvailableChild ();
			renderer.reserved = true;
			ld = new LightningData (renderer);
			ld.width = 0.15f;
			ld.widthNoise = 5f;
			ld.majorSegments = 2;
			ld.minorSegments = 2;
			ld.turbulence = 0.1f;
			ld.turbulenceNoise = 1.75f;
			ld.majorRefreshDelay = 0.1f;
			ld.majorRefreshDelayNoise = 1.6f;
			ld.ratio = 0.23f;
			return ld;
		case 1:
			renderer = (LineChild)manager.GetAvailableChild ();
			renderer.reserved = true;
			ld = new LightningData (renderer);
			ld.width = 0.1f;
			ld.widthNoise = 2f;
			ld.majorSegments = 2;
			ld.minorSegments = 2;
			ld.turbulence = 0.1f;
			ld.turbulenceNoise = 1.75f;
			ld.majorRefreshDelay = 0.1f;
			ld.majorRefreshDelayNoise = 1.6f;
			ld.ratio = 0.23f;
			return ld;
		default:
			renderer = (LineChild)manager.GetAvailableChild ();
			ld = new LightningData (renderer);
			return ld;
		}
	}

	class LightningData
	{
		public float width = singleton.width;
		public float widthNoise = singleton.widthNoise;
		public int majorSegments = singleton.majorSegments;
		public int minorSegments = singleton.minorSegments;
		public float turbulence = singleton.turbulence;
		public float turbulenceNoise = singleton.turbulenceNoise;		
		public float majorRefreshDelay = singleton.majorRefreshDelay;
		public float majorRefreshDelayNoise = singleton.majorRefreshDelayNoise;
		public float ratio = singleton.ratio;
		public bool lengthScaling = singleton.lengthScaling;
		public Vector3[] positions;
		public LineChild renderer;

		float minorTurbulence;
		float minorTurbulenceNoise;

		public LightningData(LineChild lineChild)
		{
			renderer = lineChild;
			minorTurbulence=turbulence*ratio;
            minorTurbulenceNoise = turbulenceNoise * ratio;
		}
		public void Reset()
		{
			width = singleton.width;
			widthNoise = singleton.widthNoise;
			majorSegments = singleton.majorSegments;
			minorSegments = singleton.minorSegments;
			turbulence = singleton.turbulence;
			turbulenceNoise = singleton.turbulenceNoise;		
			majorRefreshDelay = singleton.majorRefreshDelay;
			majorRefreshDelayNoise = singleton.majorRefreshDelayNoise;
			ratio = singleton.ratio;
			lengthScaling = singleton.lengthScaling;
			Refresh ();
		}
		public void Refresh()
		{
			minorTurbulence=turbulence*ratio;
			minorTurbulenceNoise=turbulenceNoise*ratio;
		}
		public Vector3 RandomOffset()
			{return singleton.RandomOffset (turbulence,turbulenceNoise);}
		public Vector3 MinorRandomOffset()
			{return singleton.RandomOffset (minorTurbulence,minorTurbulenceNoise);}
		public void Render()
		{renderer.DrawLine (positions,width+width*Random.value*widthNoise);}
		public float Delay()
			{return majorRefreshDelay + majorRefreshDelay * Random.value * majorRefreshDelayNoise;}
		public void SetPath(Vector3 origin, Vector3 target)
		{
			positions = new Vector3[majorSegments * minorSegments + 1];

			positions [0] = origin;

			for (int i = 1; i <= majorSegments; i++) 
			{
				Vector3 position = (target - origin) * i / (majorSegments) + origin;
				position += RandomOffset();
				positions [i * minorSegments] = position;
				Vector3 minorOrigin = positions [(i - 1)*minorSegments];
				Vector3 minorTarget = positions [i*minorSegments];
				for (int a = 1; a < minorSegments; a++) 
				{
					Vector3 minorPosition = (minorTarget-minorOrigin) * a / (minorSegments) + minorOrigin;
					minorPosition += MinorRandomOffset();
					positions [(i - 1)*minorSegments + a] = minorPosition;
				}
			}
			positions [positions.Length-1] =  target;
		}
	}
}
