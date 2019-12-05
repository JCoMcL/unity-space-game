using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : DamageByContact {

	void Hit() 
	{
		Destroy (gameObject);
		GetComponent<Collider2D> ().enabled = false;
	}
}
