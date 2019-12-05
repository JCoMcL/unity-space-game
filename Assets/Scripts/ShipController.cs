using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour {

	public float thrustSpeed;
	public float turnSpeed;

	public delegate void Thrust(Rigidbody2D rb, float a);
	public event Thrust thrustUp;
	public event Thrust thrustDown;
	public event Thrust thrustLeft;
	public event Thrust thrustRight;
	public event Thrust thrustClockwise;
	public event Thrust thrustAnticlockwise;
	public delegate void Fire();
	public event Fire shootEvent;

	protected float moveHorizontal = 0;
	protected float moveVertical = 0;
	protected float moveAngular = 0;
	protected bool fire0 = false;

	Rigidbody2D rb;

	void Start()
	{
		rb = GetComponent<Rigidbody2D> ();
		turnSpeed *= -1 * Vector3.SqrMagnitude(transform.localScale);
	}
	void FixedUpdate()
	{
		if (moveVertical > 0)
			{if (thrustUp != null) {thrustUp (rb, moveVertical*thrustSpeed);}}
		else if (moveVertical < 0)
			{if (thrustDown != null) {thrustDown (rb, moveVertical*thrustSpeed * -1);}}
			
		if (moveHorizontal > 0)
			{if (thrustRight != null) {thrustRight (rb, moveHorizontal*thrustSpeed);}}
		else if (moveHorizontal < 0)
			{if (thrustLeft != null) {thrustLeft (rb, moveHorizontal*thrustSpeed * -1);}}

		if (moveAngular > 0)
			{if (thrustClockwise != null) {thrustClockwise (rb, moveAngular*thrustSpeed);}}
		else if (moveAngular < 0)
			{if (thrustClockwise != null) {thrustAnticlockwise (rb, moveAngular*thrustSpeed * -1);}}
	}
	protected virtual void Update()
	{
		if (shootEvent != null) 
		{
			if (fire0) 
			{shootEvent ();}
		} 
	}
}	
