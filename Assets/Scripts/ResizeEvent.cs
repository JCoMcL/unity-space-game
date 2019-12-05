using System.Collections;
using System;
using UnityEngine;

public class ResizeEvent : MonoBehaviour 
{
	public event Action OnResize;
	public event Action OnZoom;

	float width;
	float height;
	float ortho;
	Camera cam;
	void Start () 
	{
		cam = GetComponent<Camera> ();
		width = cam.pixelWidth;
		height = cam.pixelHeight;
		ortho = cam.orthographicSize;
	}
	void Update () 
	{
		if ((cam.pixelWidth != width || cam.pixelHeight != height) && OnResize != null) 
		{	
			OnResize ();
			width = cam.pixelWidth;
			height = cam.pixelHeight;
		}
		if (cam.orthographicSize != ortho && OnZoom != null) 
		{
			OnZoom ();
			ortho = cam.orthographicSize;
		}
	}
}
