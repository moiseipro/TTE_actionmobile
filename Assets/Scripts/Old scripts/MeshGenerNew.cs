using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerNew : MonoBehaviour {
	public float size = 1f;
	public int SizeX;
	public float Yup;
	public float Ydown;

	List<int> triangels = new List<int>();
	List<Vector3> vertices = new List<Vector3>();
	List<Vector2> uvs = new List<Vector2>();
	List<Vector3> normals = new List<Vector3>();

	private MeshFilter filter;
	public MeshPress Press;

	public Mesh mesh{
		get{return filter.mesh;}
	}

	// Use this for initialization
	void Start () {
		filter = GetComponent<MeshFilter> ();
		filter.mesh = GenerateMesh();
		Press.MeshRegenerated ();
	}

	Mesh GenerateMesh(){
		Mesh mesh = new Mesh ();

		for (int z = 0; z < SizeX+1; z++) {
			for (int x = 0; x < SizeX+1; x++) {
				vertices.Add (new Vector3 (-size * 0.5f + size * (x / ((float)SizeX)), Random.Range(Yup,Ydown), -size * 0.5f + size * (z / ((float)SizeX))));
				normals.Add (Vector3.up);
				uvs.Add (new Vector2 (x / (float)SizeX, z / (float)SizeX));
			}
		}

		int vertCount = SizeX + 1;
		for (int i = 0; i < vertCount * vertCount - vertCount; i++) {
			if ((i + 1) % vertCount == 0) {
				continue;
			}
			triangels.AddRange (new List<int> () {
				i, i + vertCount, i + 1+ vertCount,
				i + vertCount + 1, i + 1, i
			});
		}


		mesh.SetVertices (vertices);
		mesh.SetNormals (normals);
		mesh.SetUVs (0, uvs);
		mesh.SetTriangles (triangels, 0);

		//GetComponent<MeshFilter> ().mesh = mesh;
		//GetComponent<MeshCollider> ().sharedMesh = mesh;
		return mesh;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
