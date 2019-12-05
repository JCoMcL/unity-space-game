using UnityEngine;
using System.Collections;

public class Stick : MonoBehaviour {
	public Rigidbody2D rb;
	public FixedJoint2D joint;

	void start()
	{
		rb = GetComponent<Rigidbody2D> ();
	}
	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == gameObject.tag) 
		{
			joint = gameObject.AddComponent<FixedJoint2D> ();
			joint.connectedBody = coll.gameObject.GetComponent<Rigidbody2D>();
		}
	}
}
