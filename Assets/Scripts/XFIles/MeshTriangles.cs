using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshTriangles : MonoBehaviour {
	public MeshFilter mesh;

	List<Vector3> vertices;
	List<int> triangles;
	int count;
	int subcount;


	void Start () 
	{
		vertices = new List<Vector3>();
		triangles = new List<int>();
	}
	void GenerateMesh()
	{
		Mesh mesh = new Mesh ();
		GetComponent<MeshFilter> ().mesh = mesh;
		mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles.ToArray();
	}
	void Update () 
	{
		if (Input.GetMouseButtonDown (0)) 
		{
			vertices.Add (Conversion.mouseToMetres3D());
			subcount++;
			count++;
			print ("subcount: " + subcount + ", count: " + count);
			if (subcount == 3) 
			{
				triangles.Add (count-1);
				triangles.Add (count-2);
				triangles.Add (count-3);
				subcount = 0;
				GenerateMesh ();
			}
		}
	}
}
