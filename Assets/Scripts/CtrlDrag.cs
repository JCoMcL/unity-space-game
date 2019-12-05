using UnityEngine;
//using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class CtrlDrag : MonoBehaviour 
{
	public ShipComponent main;
	public LayerMask mask;
	[Range(0f,1f)]public float laziness;
	public int raycastAccuracy;
	public float raycastDistance;
	public float turnSpeed = 20;
	public Transform attachPoint;

	Rigidbody2D rb;
	Collider2D coll;
	Lightning lightning;
	bool dragging = false;

	bool contact;
	float contactDist;
	Collider2D contactCollider;
	float contactAngle;
	Vector2 contactPoint;
	ConditionManager contactCondition;

	Vector2 move;

	void Start () 
	{
		Physics2D.queriesHitTriggers = true;
		rb = GetComponent<Rigidbody2D> ();
		coll = GetComponent<Collider2D> ();
		main = GetComponent<ShipComponent> ();
		lightning = GameObject.Find ("LineManager").GetComponent<Lightning> ();
		contactCondition = new ConditionManager ();
	}
	void Update () 
	{
		if (dragging) 
		{
			move = (new Vector2 (transform.position [0], transform.position [1]) * laziness + Conversion.mouseToMetres2D () * (1 - laziness)); //refresh rate dependant
			if (Input.GetMouseButtonUp (0)) {Drop ();}
		}
	}
	void FixedUpdate ()
	{
		if (dragging) 
		{
			List<RaycastHit2D> contacts = new List<RaycastHit2D> ();
			contactDist = raycastDistance;

			for(float i =0; i <= raycastAccuracy; i++)
			{
				RaycastHit2D[] hits = new RaycastHit2D[1];
				float angle = Mathf.PI * 2 * (i / raycastAccuracy); 
				Vector2 direction = new Vector2(Mathf.Cos(angle),Mathf.Sin(angle));

				int hit = coll.Raycast(direction, hits, contactDist, mask.value);
				if (hit > 0) 
				{	
					contactDist = hits [0].distance;
					contacts.Add (hits[0]);
					contactAngle = angle;
				}
			}
			if (contacts.Count > 0) 
			{
				RaycastHit2D lastHit = contacts[contacts.Count-1];
				contact = true;
				contactDist = lastHit.distance;
				contactPoint = lastHit.point;
				if (contactCollider == null || contactCollider.gameObject != lastHit.collider.gameObject) 
				{	
					contactCondition.contact = false;
					contactCondition = new ConditionManager ();
					contactCollider = lastHit.collider;
					lightning.Tether (attachPoint,contactCollider.transform,contactCondition.Condition);
				}
				Vector2 target = contactPoint;
				Vector2 position = new Vector2 (transform.position [0], transform.position [1]);
				Vector2 force = (position - target) * turnSpeed * Time.fixedDeltaTime;
				position += new Vector2 (Mathf.Sin (Conversion.DegToRad (transform.eulerAngles.z)), Mathf.Cos (Conversion.DegToRad (transform.eulerAngles.z)));
				rb.AddForceAtPosition (force, position);
			}
			else
			{
				contact = false;
				contactCondition.contact = false;
				contactCollider = null;
			}
			rb.MovePosition (move);
		}
	}
	void OnMouseOver ()
	{
		if (Input.GetMouseButtonDown (0)){PickUp ();}
	}
	/*void OnTriggerEnter2D()
	{
		if (dragging) {dragging = false;}
		if (contact) {Stick ();}
	}*/
	public void PickUp()
	{
		if (rb == null) 
		{
			rb = GetComponent<Rigidbody2D> ();
			coll = GetComponent<Collider2D> ();
		}
		dragging = true; 
		rb.angularDrag = 5f; 
		coll.isTrigger = true;	
		transform.position -= Vector3.forward;
	}
	void Drop()
	{
		transform.position += Vector3.forward;
		rb.angularDrag = 0.05f;
		dragging = false;
		coll.isTrigger = false;
		if (contact)	{Stick ();}
		contactCollider = null;
	}
	void Stick()
	{
		contact = false;
		contactCondition.contact = false;
		//change
		int direction = Conversion.RoundAngle(Conversion.RadToDeg (contactAngle)*-1 - 225 + contactCollider.transform.parent.parent.eulerAngles.z);
		object[] package = new object[2];
		package[0] = contactCollider;
		package [1] = direction;
		contactCollider = null;
		main.AddModule(package);
	}

	class ConditionManager
	{
		public bool contact = true;
		public bool Condition()	{return contact;}
	}
}
