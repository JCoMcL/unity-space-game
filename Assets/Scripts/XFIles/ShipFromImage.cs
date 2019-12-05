using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipFromImage : MonoBehaviour 
{
	public Texture2D map;
	public Color[] keys;
	public GameObject[] values;
	Vector2 cp = new Vector2(0.8f,0.8f);
	Dictionary<Color,GameObject> dictionary = new Dictionary<Color, GameObject>();

	void Start () 
	{
		for (int i = 0; i < keys.Length; i++) 
		{	
			dictionary.Add (keys[i],values[i]);
		}
		print (corePosition ());
	}
	Vector2 corePosition()
	{
		if (cp.x == 0.8f) 
		{
			Vector2 centre = new Vector2(Mathf.Round ((map.height) / 2), Mathf.Round ((map.width) / 2));
			if (map.GetPixel ((int)centre.x, (int)centre.y) == Color.blue) 
			{
				cp = centre;
				return cp;
			}
			FloodFill ff = new FloodFill (map, centre, Color.clear);
			Pixel px = new Pixel();
			for (int i = map.height * map.width; i > 0; i--) 
			{
				px = ff.Next ();
				if (px.color == Color.blue) {break;}
			}
			cp = px.position;
		}
		return cp;
	}

	class FloodFill
	{
		Queue<Vector2> queue;
		HashSet<Vector2> discovered;
		Texture2D map;
		Color blank;
		public FloodFill(Texture2D texture, Vector2 start, Color exemption)
		{
			queue = new Queue<Vector2> ();
			discovered = new HashSet<Vector2>();
			queue.Enqueue (start);
			map = texture;
			blank = exemption;
		}
		public Pixel Next()
		{
			Vector2 cursor = queue.Dequeue ();
			discovered.Add (cursor);
			Color color = map.GetPixel ((int)cursor.x, (int)cursor.y);
			if (color != blank) 
			{
				for (int i = 0; i < 4; i++) 
				{
					Vector2 v2 = Conversion.IntToVector2 (i) + cursor;
					if (v2.x >= 0 && v2.x < map.width
						&& v2.y >=0 && v2.y < map.height
						&& !discovered.Contains (v2) 
						&& !queue.Contains (v2)) 
					{queue.Enqueue (v2);}
				}
			}
			Pixel pixel = new Pixel( cursor, color );
			return pixel;
		}
	}
	public struct Pixel
	{
		public Vector2 position;
		public Color color;
		public Pixel(Vector2 v, Color c)
		{
			position = v;
			color = c;
		}
	}
}
