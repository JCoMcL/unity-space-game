using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineChild : PoolChild 
{
	LineRenderer line;

	void Awake()
	{
		line = GetComponent<LineRenderer>();
	}

	public void DrawLine(Vector3[] points, float width)
	{
		SetActive ();
		line.positionCount = points.Length;
		line.SetPositions (points);
		line.widthMultiplier = width;
	}
	protected override void FrameReset ()
	{
		base.FrameReset ();
		line.enabled = false;
		line.positionCount = 0;
	}
	protected override void SetActive ()
	{
		base.SetActive ();
		if (line == null) {line = GetComponent<LineRenderer> ();}
		line.enabled = true;
	}

}
