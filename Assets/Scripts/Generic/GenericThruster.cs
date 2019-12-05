using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericThruster : MonoBehaviour {
	public float power = 1;
	public ParticleScaler exhaust;

	public void Thrust(Rigidbody2D rb, float throttle)
	{
		float netPower = throttle * power;
		exhaust.multiplier = netPower;
		float angle = Conversion.DegToRad (transform.eulerAngles.z * -1  + 90);
		Vector2 force = new Vector2 (Mathf.Cos (angle), Mathf.Sin (angle)) * netPower;
		Vector2 position = new Vector2 (transform.position [0], transform.position [1]);
		rb.AddForceAtPosition (force,position);
	}
}
