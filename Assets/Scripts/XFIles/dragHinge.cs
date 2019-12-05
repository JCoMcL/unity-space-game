using UnityEngine;
using System.Collections;

public class dragHinge : MonoBehaviour
{
	//private Rigidbody2D rb;
	private HingeJoint2D hinge;
	private bool dragging = false;
	private Vector2 formerMousePosition;
	private Vector2 deltaMousePosition;
	public int multi;

	void Start () 
	{
		Physics2D.queriesHitTriggers = true;
		//rb = GetComponent<Rigidbody2D> ();
	}
	void Update () 
	{
		if (Input.GetMouseButtonUp (0)) 
		{
			Destroy(hinge);
			dragging = false;
			propagate.permit = true;
		}
	}
	void FixedUpdate ()
	{
		if (dragging) 
			{hinge.connectedAnchor = Conversion.mouseToMetres2D();}
	}
	void OnMouseOver (){
		if (Input.GetMouseButtonDown (0))
		{
			propagate.permit = false;
			hinge = gameObject.AddComponent<HingeJoint2D>();
			hinge.anchor = Conversion.MouseToObject(transform.position,transform.localScale,transform.eulerAngles.z);
			dragging = true;
			hinge.autoConfigureConnectedAnchor = false;
		}
	}
}