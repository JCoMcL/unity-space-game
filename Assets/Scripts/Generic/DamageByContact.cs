using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageByContact : MonoBehaviour 
{
	protected List<Collider2D> hitExemptions;
	public float damage = 2;
	public Collider2D colld;
	public ContactFilter2D filter;

	public void AddExemption(Collider2D exemption) 
	{
		if(hitExemptions == null)	{hitExemptions = new List<Collider2D> ();}
		hitExemptions.Add (exemption);
	}
	void OnTriggerEnter2D(Collider2D coll)	
	{
		if (!coll.isTrigger && coll.gameObject.layer != 13 && hitExemptions != null && !hitExemptions.Contains (coll))
		{
			IDamageable<float> damagable = coll.GetComponent<IDamageable<float>> ();
			if (damagable != null) {damagable.Damage (damage);}
			BroadcastMessage ("Hit");
		}
	}
}
