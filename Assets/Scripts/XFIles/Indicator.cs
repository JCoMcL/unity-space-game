using UnityEngine;
using System.Collections;

public class Indicator : MonoBehaviour {

	public SpriteRenderer rend;
	public double minSize = 0.1;
	public bool clear = false;
	private int contacts = 0;

	void Update()
	{
		if (transform.localScale [0] >= minSize && contacts == 0) {
			clear = true;
			rend.color = new Color (1f, 1f, 1f, 0.4f);
		} else {
			clear = false;
			rend.color = new Color (1f, 0f, 0f, 0.4f);
		}
	}
	void OnTriggerEnter2D(Collider2D coll)	{contacts++;}
	void OnTriggerExit2D(Collider2D coll) 	{contacts--;}
}
