using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageChild : PoolChild 
{
	Image img;
	RectTransform rt;
	Camera cam;

	public void SetSprite(Sprite sprite)
	{
		SetActive ();
		img.sprite = sprite;
	}
	public void SetSize(float size)
	{
		SetActive ();
		rt.localScale = new Vector3(size,size,size);
	}
	public void SetPosition(Vector2 position)
	{
		SetActive ();
		rt.anchoredPosition = position;
	}
	public void SetColor(Color color)
	{
		SetActive ();
		img.color = color;
	}

	protected override void FrameReset ()
	{
		base.FrameReset ();
		img.enabled = false;
	}
	protected override void SetActive ()
	{
		base.SetActive ();
		GetImage ().enabled = true;
		GetCam ();
		GetRect ();
	}

	Camera GetCam()
	{
		if (cam == null) {cam = Camera.main;}
		return cam;
	}
	Image GetImage()
	{
		if (img == null) {img = GetComponent<Image> ();}
		return img;
	}
	RectTransform GetRect()
	{
		if (rt == null) {rt = GetImage ().rectTransform;}
		return rt;
	}
}
