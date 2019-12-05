using UnityEngine;
using System.Collections;

public class PlayerController : ShipController 
{
	protected override void Update ()
	{
		moveHorizontal = Input.GetAxis ("Horizontal");
		moveVertical = Input.GetAxis ("Vertical");
		moveAngular = Input.GetAxis ("Angular");
		fire0 = Input.GetKey (KeyCode.Space);
		base.Update ();
	}
}
