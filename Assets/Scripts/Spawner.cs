using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour 
{
	[SerializeField]
	GameObject[] spawnPool;
	void Start()
	{
		for (int i = 0; i < 500; i++) 
		{
			GameObject newPickup = Instantiate (spawnPool[Random.Range (0, spawnPool.Length)]);

			float randAngle = Conversion.DegToRad (Random.Range (0, 360));
			newPickup.transform.position = new Vector3 (Mathf.Cos (randAngle), Mathf.Sin (randAngle), 0) * Random.Range (2,200);
			newPickup.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, Random.Range (0, 360)));
		}
	}
}
