using UnityEngine;
using System.Collections;

public class Conversion : MonoBehaviour 
{
	public static Vector2 pixelsToMetres2D (Vector2 px)
	{
		Vector3 pos = new Vector3 (px [0], px [1], 0);
		Vector3 retrn = Camera.main.ScreenToWorldPoint(pos);
		return new Vector2 (retrn [0], retrn [1]);
	}
	public static Vector3 pixelsToMetres3D (Vector2 px)
	{
		Vector3 pos = new Vector3 (px [0], px [1], 0);
		return Camera.main.ScreenToWorldPoint (pos);
	}
	public static Vector2 MetresToPixels(Vector3 metres)
	{
		Vector3 camSize = new Vector3 (Camera.main.pixelWidth, Camera.main.pixelHeight);
		return Camera.main.WorldToScreenPoint (metres) - camSize / 2;
	}
	public static Vector2 mouseToMetres2D ()
	{
		Vector3 mouse = new Vector3(Input.mousePosition[0],Input.mousePosition[1],0);
		Vector3 retrn = Camera.main.ScreenToWorldPoint(mouse);
		return new Vector2 (retrn [0], retrn [1]);

	}
	public static Vector3 mouseToMetres3D ()
	{
		Vector3 mouse = new Vector3(Input.mousePosition[0],Input.mousePosition[1],0);
		return Camera.main.ScreenToWorldPoint (mouse);
	}
	public static float DegToRad (float angle)
	{
		angle *= -2 * Mathf.PI / 360;
		return angle;
	}
	public static float RadToDeg (float angle)
	{
		angle *= 180 / Mathf.PI;
		return angle;
	}
	public static Vector2 MouseToObject(Vector3 pos, Vector3 scale, float angle)
	{
		Vector2 localPos = Conversion.mouseToMetres2D() - new Vector2 (pos [0], pos [1]);
		localPos = new Vector2(localPos[0] / scale [0],localPos[1] / scale [1]);
		angle = Conversion.DegToRad(angle);
		float cos = Mathf.Cos (angle);
		float sin = Mathf.Sin (angle);
		return new Vector2(localPos[0] * cos - localPos[1] * sin, localPos[0] * sin + localPos[1] * cos);
	}
	public static int RoundAngle(float angle)
	{
		while(angle < 0)	{angle += 360;}
		while(angle >= 360)	{angle -= 360;}

		if (angle >= 90) {
			if (angle >= 180) {
				if (angle >= 270) {
					return (0);
				} else {
					return (1);
				}
			} else {
				return (2);
			}
		}
		else 
		{
			return (3);
		}
	}
	public static int RoundInt(int i)
	{
		while(i < 0)	{i += 4;}
		while(i >= 4)	{i -= 4;}

		return i;
	}
	public static Vector2 IntToVector2(int i)
	{
		switch (i) 
		{
		case 0:		return new Vector2 (1, 0);	
		case 1:		return new Vector2 (0, 1);	
		case 2:		return new Vector2 (-1, 0);	
		case 3:		return new Vector2 (0, -1);	
		default:	return new Vector2 (0, 0);	
		}
	}
	public static Vector3 IntToVector3(int i)
	{
		switch (i) 
		{
		case 0:		return new Vector3 (1, 0, 0);	
		case 1:		return new Vector3 (0, 1, 0);	
		case 2:		return new Vector3 (-1, 0, 0);	
		case 3:		return new Vector3 (0, -1, 0);	
		default:	return new Vector3 (0, 0, 0);	
		}
	}
	public static int Vector2ToInt(Vector2 vector)
	{
		if(vector[0] > 0){return 0;}
		else if(vector[0] < 0){return 2;}
		else if(vector[1] > 0){return 1;}
		else if(vector[1] < 0){return 3;}
		else {return -1;}
	}
	public static int FlipDirection(int direction)
	{
		if (direction + 2 < 4) {direction+=2;} 
		else {direction-=2;}
		return direction;
	}
	public static float GetHue(Color color)
	{
		float r = color.r;
		float g = color.g;
		float b = color.b;
		float[] array = new float[]{ r, g, b };
		int max = 0;
		float maxValue = 0;
		float minValue = 1;
		for (int i = 0; i < 3; i++) 
		{
			if (array [i] > maxValue) 
			{
				maxValue = array [i];
				max = i;
			}
			if (array [i] < minValue) 
			{	minValue = array [i];}
		}
		if (maxValue != minValue) 
		{
			float hue = 1 / (maxValue - minValue);
			switch (max) 
			{
				case 0:hue *= (g - b);			break;	
				case 1:hue = 2f + (b - r) * hue;break;	
				case 2:hue = 4f + (r - g) * hue;break;	
			}
			if(hue < 0)	{hue += 6f;}
			return hue / 6;
		} 
		return 0;
	}
	public static int GetMax(float[] array)
	{
		float maxValue = 0;
		int maxIndex = 0;
		for (int i = 0; i < array.Length; i++) 
		{
			if (array [i] > maxValue) 
			{
				maxValue = array [i];
				maxIndex = i;
			}
		}
		return maxIndex;
	}
}
/*
 	 public static int pixelsPerUnit = 216;
	public static Vector2 pixelsToMetres2D (Vector2 px)
	{
		Vector2 cam2d = new Vector2 (Camera.main.transform.position [0], Camera.main.transform.position [1]);
		Vector2 camsize = new Vector2 (Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize);
		Vector2 centred = new Vector2(px[0],px[1]) - new Vector2(Screen.width / 2, Screen.height /2);
		return new Vector2(centred [0] * camsize [0], centred [1] * camsize [1]) / pixelsPerUnit + cam2d;
	}
	public static Vector3 pixelsToMetres3D (Vector2 px)
	{
		Vector3 cam3d = new Vector3 (Camera.main.transform.position [0], Camera.main.transform.position [1],0);
		Vector3 camsize = new Vector2 (Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize);
		Vector3 centred = new Vector3(px[0],px[1],0) - new Vector3(Screen.width / 2, Screen.height /2,0);
		return new Vector3 (centred [0] * camsize [0], centred [1] * camsize [1], 0) / pixelsPerUnit + cam3d;
	}
	public static Vector2 mouseToMetres2D ()
	{
		Vector2 cam2d = new Vector2 (Camera.main.transform.position [0], Camera.main.transform.position [1]);
		Vector2 camsize = new Vector2 (Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize);
		Vector2 centred = new Vector2(Input.mousePosition[0],Input.mousePosition[1]) - new Vector2(Screen.width / 2, Screen.height /2);
		return new Vector2(centred [0] * camsize [0], centred [1] * camsize [1]) / pixelsPerUnit + cam2d;

	}
	public static Vector3 mouseToMetres3D ()
	{
		Vector3 cam3d = new Vector3 (Camera.main.transform.position [0], Camera.main.transform.position [1],0);	
		Vector3 camsize = new Vector2 (Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize);
		Vector3 centred = (new Vector3(Input.mousePosition[0],Input.mousePosition[1],0) - new Vector3(Screen.width / 2, Screen.height /2,0));
		return new Vector3 (centred [0] * camsize [0], centred [1] * camsize [1], 0) / pixelsPerUnit + cam3d;
	}
*/