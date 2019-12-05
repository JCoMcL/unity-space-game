using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ImageConstantSize : MonoBehaviour 
{
	public Image img;
	Camera cam;
	void Awake () 
	{
		cam = Camera.main;
		cam.GetComponent<ResizeEvent> ().OnResize += OnResize;
		cam.GetComponent<ResizeEvent> ().OnZoom += OnResize;
		OnResize ();
	}
	void OnResize () 
	{
		float size = cam.WorldToScreenPoint (Vector2.zero).y;
		size -= cam.WorldToScreenPoint (Vector2.down).y;
		if (size < 0) {size = Mathf.Abs (size);}
		img.rectTransform.sizeDelta = new Vector2 (size, size);
	}
}
