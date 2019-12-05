using UnityEngine;
using System.Collections;

public class propagate : MonoBehaviour {

	public static bool permit = true;
	public GameObject circle;
	public GameObject indicator;
	public int pixelsPerMetre = 216;

	private Vector2 init2; //initial mouse postion
	private bool mouseDown = false;
	private GameObject activeIndicator;
	private Indicator indicatorScript;

	void Update ()
	{
		if (Input.GetMouseButtonDown (0) && permit) 
		{
			init2 = new Vector2 (Input.mousePosition [0], Input.mousePosition [1]);
			activeIndicator = Instantiate (indicator);
			indicatorScript = activeIndicator.GetComponent<Indicator>();
			mouseDown = true;
		}
		if (!permit) 
		{
			mouseDown = false;
			Destroy (activeIndicator);
		}

		if (mouseDown) 
		{
			Vector3 pos = Conversion.pixelsToMetres2D (init2);
			activeIndicator.transform.position = pos;
			float radius = Vector2.Distance (init2, new Vector2 (Input.mousePosition [0], Input.mousePosition [1]));
			float size = 2 * radius * Camera.main.orthographicSize / pixelsPerMetre;
			Vector3 scale = new Vector3 (size, size, 1);
			activeIndicator.transform.localScale = scale;

			if (Input.GetMouseButtonUp (0)) 
			{
				Destroy (activeIndicator);
				mouseDown = false;
				if (indicatorScript.clear) 
				{
					circle.transform.position = pos;
					circle.transform.localScale = scale;
					Instantiate (circle);
				}


			}

		}
	}
}
