using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {
	Camera cam;
	public float damping = 0.001f;
	void Start()
		{cam = Camera.main;}
	void Update()
		{transform.localScale = new Vector3 (cam.orthographicSize, cam.orthographicSize, 1) * 4;}
	void FixedUpdate () 
		{transform.position = new Vector3 (cam.transform.position [0], cam.transform.position [1], 1);}
}
