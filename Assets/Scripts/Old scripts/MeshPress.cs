using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshGenerNew))]

public class MeshPress : MonoBehaviour {

	public float maxDepress;
	public List<Vector3> oVertices;
	public List<Vector3> mVertices;
	public float OldPos;

	public GameObject Player;

	public MeshGenerNew plane;

	public void MeshRegenerated(){
		plane = GetComponent<MeshGenerNew> ();
		//plane.mesh.MarkDynamic ();
		oVertices = plane.mesh.vertices.ToList ();
		mVertices = plane.mesh.vertices.ToList ();
	}

	public void AddDepression(Vector3 Point, float radius){
		var worldOis = this.transform.worldToLocalMatrix * Point;
		var worldPos = new Vector3 (worldOis.x, worldOis.y, worldOis.z);
		for (int i = 0; i < mVertices.Count; i++) {
			var distance = (worldPos - (mVertices [i] + Vector3.down * maxDepress)).magnitude;
			OldPos = Player.transform.position.x;
			if (distance < radius) {
				var newVert = oVertices [i] + Vector3.down * (Random.Range(maxDepress-0.05f,maxDepress+0.05f));
				mVertices.RemoveAt (i);
				mVertices.Insert (i, newVert);
			}
		}
		plane.mesh.SetVertices (mVertices);
	}
}
