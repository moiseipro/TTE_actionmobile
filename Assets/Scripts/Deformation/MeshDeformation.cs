using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDeformation : MonoBehaviour {

	Mesh mesh;

	public float minVelocity = 5f;
	public float radiusForD = .5f;
	public float tormoz;

	// Use this for initialization
	void Start () {
		mesh = GetComponent<MeshFilter>().mesh;
	}

	void OnCollisionEnter(Collision collision){
		if (collision.relativeVelocity.magnitude > minVelocity) {
			Vector3[] vertices = mesh.vertices;
			for (int i = 0; i < mesh.vertexCount; i++) {
				for (int j = 0; j < collision.contacts.Length; j++) {
					Vector3 point = transform.InverseTransformPoint (collision.contacts [j].point);
					Vector3 velocity = transform.InverseTransformPoint (collision.relativeVelocity);
					float distance = Vector3.Distance (point, vertices [i]);
					if (distance < radiusForD) {
						Vector3 deformate = velocity * (radiusForD - distance);
						vertices [i] += deformate;
					}
				}
			}
				mesh.vertices = vertices;
				mesh.RecalculateNormals();
				mesh.RecalculateBounds();
				GetComponent<MeshCollider> ().sharedMesh = mesh;
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
