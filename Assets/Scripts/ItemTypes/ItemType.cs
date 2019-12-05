using UnityEngine;
using System.Collections;

public class ItemType : MonoBehaviour, IDamageable<float>
{
	Vector2 gridSize = new Vector2 (4, 4);
	float fringe=0.005f;

	public int spawnIndex;
	public float health = 10;
	protected Vector2 textureCoords;
	protected delegate void OnRemoval();
	protected event OnRemoval removalEvent;
	protected ShipController shipController;

	public virtual Vector2 GetTextureCoordinates()
	{
		return new Vector2 (textureCoords[0] / gridSize[0]+fringe, textureCoords[1] / gridSize[1]+fringe);
	}
	public void Damage(float damage)
	{
		health -= damage;
		if(health <= 0){Kill ();}
	}
	protected void Kill()
	{
		SendMessage ("OnKill");
		Destroy (gameObject);
	}
	public virtual void Integrate(ShipController controller)
	{
	}
	public virtual void Remove()
	{
		if (removalEvent != null) {removalEvent ();}
	}
	public virtual bool IsAttachable()
	{
		return true;
	}
}

