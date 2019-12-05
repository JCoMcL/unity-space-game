using UnityEngine;
using System.Collections;
public class ShipComponent : MonoBehaviour, IPersistable<GameManager.ObjectConstructor>
{
	public ModuleManager manager;
	public CtrlDrag detached;
	public Module attached;
	public Rigidbody2D rb;
	public GameObject[] edges;
	public Collider2D attachedCollider;
	public ItemType type;

	void Awake()
	{
		if(detached == null)
			{detached = gameObject.GetComponent<CtrlDrag> ();}
		if(attached == null)
			{attached = gameObject.GetComponent<Module> ();}
		if(rb == null)
			{rb = gameObject.GetComponent<Rigidbody2D> ();}
		if(type == null)
			{type = gameObject.GetComponent<ItemType> ();}
		if(attachedCollider == null)
		{attachedCollider = gameObject.GetComponent<Collider2D> ();}

		attached.edges = edges;
		attached.main = this;
		attached.containingCollider = attachedCollider;
		attached.attachable = type.IsAttachable ();
	}
	bool AddRigidBody()
	{
		try
		{
			rb = gameObject.AddComponent<Rigidbody2D> ();
			rb.drag = 0.5f;
			rb.angularDrag = 0.05f;
			rb.useAutoMass = true;
			return true;
		}
		catch 
		{return false;}
	}
	public void AddModule(object[] package)
	{
		attached.enabled = true;
		detached.enabled = false;
		try{attached.Initialize((Collider2D)package[0], (int)package[1]);}
		catch
		{
			transform.SetParent ((Transform)package[0]);
			try
			{
				transform.localPosition = (Vector2)package [1];
				attached.gridLocation = (Vector2)package [1];
			} catch
			{	transform.localPosition = attached.gridLocation;}
		}
		foreach(GameObject edge in edges)	
		{edge.SetActive (true);}
		if(rb != null)	{Destroy (rb);}	
		manager = gameObject.GetComponentInParent<ModuleManager> ();
		manager.AddModule (attached);
		ShipController controller = gameObject.GetComponentInParent<ShipController> ();
		if (controller != null) {type.Integrate (controller);}
		//GameManager.singleton.SaveEvent -= Save;
	}
	public void Detach (Vector3 centre)
	{
		if (AddRigidBody ()) 
		{
			attached.enabled = false;
			detached.enabled = true;
			transform.parent = null;
			foreach (GameObject edge in edges) 
				{edge.SetActive (false);}
			manager = null;
			if (transform.position == centre) 
				{detached.PickUp ();}
			gameObject.layer = 10;
			rb.isKinematic = false;
			rb.AddForce ((transform.position - centre) * 20);
			type.Remove ();
		}
		//GameManager.singleton.SaveEvent += Save;
	}
	#region Persistance
	public GameManager.ObjectConstructor Save()
	{
		return new ShipComponentConstructor(this);
	}
	[System.Serializable]
	class ShipComponentConstructor: GameManager.ObjectConstructor
	{
		
	#region Module Variables
		float[] gridLocation;
		int gridRotation;
	#endregion
	#region ItemType Variables
		float health;
		int spawnIndex;
	#endregion
	#region GameObject Variables
		float[] position;
		float[] scale;
		float rotation;
	#endregion
		public ShipComponentConstructor(ShipComponent sc)
		{
			gridLocation = Vec2Encode (sc.attached.gridLocation);
			gridRotation = sc.attached.gridRotation;

			health = sc.type.health;
			spawnIndex = sc.type.spawnIndex;
			position = Vec3Encode (sc.transform.position);
			scale = Vec3Encode (sc.transform.localScale);
			rotation = sc.transform.eulerAngles.z;
		}
		public override GameObject Load ()
		{
			GameObject obj = GameManager.singleton.SpawnNew (spawnIndex);
			obj.transform.position = Vec3Parse (position);
			obj.transform.localScale = Vec3Parse (scale);
			obj.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, rotation));

			ShipComponent sc = obj.GetComponent<ShipComponent> ();
			sc.attached.gridLocation = Vec2Parse (gridLocation);
			sc.attached.gridRotation = gridRotation;

			sc.type.health = health;
			return obj;
		}
	}
	#endregion
}
