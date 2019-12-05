using UnityEngine;
using System.Collections;

public class Core : Turret
{
	public GenericThruster[] thrusters; 
	public override Vector2 GetTextureCoordinates ()
	{
		textureCoords = new Vector2 (3, 3);
		return base.GetTextureCoordinates ();
	}
	void Start()
	{
		shipController = GetComponentInParent<ShipController> ();
		shipController.thrustUp += thrusters[4].Thrust;
		shipController.thrustUp += thrusters[5].Thrust;
		shipController.thrustDown += thrusters[1].Thrust;
		shipController.thrustDown += thrusters[2].Thrust;
		shipController.thrustLeft += thrusters[0].Thrust;
		shipController.thrustRight += thrusters[3].Thrust;
		shipController.thrustClockwise += thrusters[4].Thrust;
		shipController.thrustClockwise += thrusters[1].Thrust;
		shipController.thrustAnticlockwise += thrusters[2].Thrust;
		shipController.thrustAnticlockwise += thrusters[5].Thrust;
		shipController.shootEvent += Shoot;
	}
}