using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchOnAwake : MonoBehaviour 
{
	public float speed;
	Rigidbody2D rb;

	void Start () 
	{
		rb = GetComponent<Rigidbody2D> ();
		rb.AddRelativeForce (Vector2.up * speed);
	}
}
