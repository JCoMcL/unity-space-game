using UnityEngine;
using System.Collections;

public class followMouse : MonoBehaviour 
{
	public int pixelsPerMetre;

	void Update () 
	{
		transform.position = Conversion.mouseToMetres3D();
	}
}
