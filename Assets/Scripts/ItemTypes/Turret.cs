using UnityEngine;
using System.Collections;

public class Turret: ItemType
{
	public Collider2D parentCollider;
	public GameObject bullet;
	public GameObject muzzleEffect;
	public float delay = 0.05f;
	protected Vector2 bulletSpawn = new Vector2(0,0.5f);
	public bool armed = true;
	bool firing;

	public override Vector2 GetTextureCoordinates ()
	{
		textureCoords = new Vector2 (2, 0);
		return base.GetTextureCoordinates ();
	}
	public override bool IsAttachable ()	{return false;}

	public override void Integrate (ShipController controller)
	{
		shipController = controller;
		shipController.shootEvent += Shoot;
		removalEvent += ClearSubscribtion;
	}
	protected void Shoot()
	{if (!firing && armed) 	{StartCoroutine (ShootRoutine ());}}

	protected IEnumerator ShootRoutine()
	{
		firing = true;
		GameObject newBullet = Instantiate (bullet,transform.TransformPoint (bulletSpawn),transform.rotation);
		newBullet.GetComponent<Bullet> ().AddExemption (GetCollider());
		GameObject newEffect = Instantiate (muzzleEffect,transform);
		newEffect.transform.localPosition = bulletSpawn;
		yield return new WaitForSeconds (delay);
		firing = false;
	}
	Collider2D GetCollider()
	{
		if (parentCollider == null || !parentCollider.enabled) 
			{parentCollider = GetComponent<Module> ().containingCollider;}
		return parentCollider;
	}
	void ClearSubscribtion()	{shipController.shootEvent -= Shoot;}
}
