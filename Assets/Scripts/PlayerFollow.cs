using UnityEngine;
using System.Collections;

public class PlayerFollow : MonoBehaviour {
	public GameObject target;
	public int pixelsPerUnit = 216;
	public float fov;
	[Range(0.0f,1.0f)]public float laziness;
	private Camera cam;
	private Rigidbody2D rb;
	//PlayerModuleManager pmm;
	int mainLength = 1;
	void Start()
	{
	//	pmm = target.GetComponent<PlayerModuleManager> ();
		cam = GetComponent<Camera> ();
		rb = target.GetComponent<Rigidbody2D> ();
	}
	void Update()
	{
		/*if (pmm.width >= pmm.height)
		{mainLength = pmm.width;}
		else 	{mainLength = pmm.height;print (mainLength);}*/

	}
	void FixedUpdate () 
	{
		cam.orthographicSize = fov + mainLength + Vector3.Magnitude(rb.velocity)/10;
		transform.position = new Vector3(target.transform.position[0],target.transform.position[1],-10);
	}
}
