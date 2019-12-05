using UnityEngine;
using System.Collections;

public class DragSpring : MonoBehaviour 
{
	//private Rigidbody2D rb;
	public SpringJoint2D spring;
	private bool dragging = false;
	private Vector2 formerMousePosition;
	private Vector2 deltaMousePosition;

	void Start () 
	{
		Physics2D.queriesHitTriggers = true;
		//rb = GetComponent<Rigidbody2D> ();
	}
	void Update () 
	{
		if (Input.GetMouseButtonUp (0)) 
		{
			Destroy(spring);
			dragging = false;
			propagate.permit = true;
		}
	}
	void FixedUpdate ()
	{
		if (dragging) 
		{
			spring.connectedAnchor = Conversion.mouseToMetres2D();
			spring.distance = 0;
		}
	}
	void OnMouseOver (){
		if (Input.GetMouseButtonDown (0))
		{
			propagate.permit = false;
			spring = gameObject.AddComponent<SpringJoint2D>();
			spring.anchor = Conversion.MouseToObject(transform.position,transform.localScale,transform.eulerAngles.z);
			dragging = true;
			spring.dampingRatio = 0;
			spring.autoConfigureConnectedAnchor = false;
		}
	}
}