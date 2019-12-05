using UnityEngine;
using System.Collections;

public class Thruster: ItemType
{
	public GenericThruster thruster;

	void Thrust(Rigidbody2D rb, float throttle)
		{thruster.Thrust (rb, throttle);}
	public override Vector2 GetTextureCoordinates ()
	{
		textureCoords = new Vector2 (1, 0);
		return base.GetTextureCoordinates ();
	}
	public override bool IsAttachable ()
		{return false;}
	public override void Integrate (ShipController controller)
	{
		shipController = controller;
		switch (Conversion.RoundAngle (transform.localEulerAngles.z)) 
		{
		case 0:	
			AddLeft ();
			if (transform.localPosition [1] < 0) 
				{AddClockwise ();}
			else if (transform.localPosition [1] > 0) 
				{AddAnticlockwise ();}
			break;
		case 1:	
			AddUp ();
			if (transform.localPosition [0] < 0) 
				{AddClockwise ();}
			else if (transform.localPosition [0] > 0) 
				{AddAnticlockwise ();}
			break;
		case 2:	
			AddRight ();
			if (transform.localPosition [1] > 0) 
				{AddClockwise ();}
			else if (transform.localPosition [1] < 0) 
				{AddAnticlockwise ();}
			break;
		case 3:	
			AddDown ();
			if (transform.localPosition [0] > 0) 
			{AddClockwise ();}
			else if (transform.localPosition [0] < 0) 
			{AddAnticlockwise ();}
			break;
		default:	
			break;
		}
	}
	void AddUp()				{shipController.thrustUp += Thrust;				removalEvent += ClearUp;}
	void AddDown()				{shipController.thrustDown += Thrust;			removalEvent += ClearDown;}
	void AddLeft()				{shipController.thrustLeft += Thrust;			removalEvent += ClearLeft;}
	void AddRight()				{shipController.thrustRight += Thrust;			removalEvent += ClearRight;}
	void AddClockwise()			{shipController.thrustClockwise += Thrust;		removalEvent += ClearClockwise;}
	void AddAnticlockwise()		{shipController.thrustAnticlockwise += Thrust;	removalEvent += ClearAnticlockwise;}

	void ClearUp()				{shipController.thrustUp -= Thrust;				removalEvent -= ClearUp;}
	void ClearDown()			{shipController.thrustDown -= Thrust;			removalEvent -= ClearDown;}
	void ClearLeft()			{shipController.thrustLeft -= Thrust;			removalEvent -= ClearLeft;}
	void ClearRight()			{shipController.thrustRight -= Thrust;			removalEvent -= ClearRight;}
	void ClearClockwise()		{shipController.thrustClockwise -= Thrust;		removalEvent -= ClearClockwise;}
	void ClearAnticlockwise()	{shipController.thrustAnticlockwise -= Thrust;	removalEvent -= ClearAnticlockwise;}
}
