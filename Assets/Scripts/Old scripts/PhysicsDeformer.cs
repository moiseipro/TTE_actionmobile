using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsDeformer : MonoBehaviour {

	public MeshPress deformableMesh;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionStay(Collision collision){
		if (((int)(deformableMesh.OldPos * 100) / 100f) != ((int)(deformableMesh.Player.transform.position.x * 100) / 100f) ||
			((int)(deformableMesh.OldPos * 100) / 100f) != ((int)(deformableMesh.Player.transform.position.z * 100) / 100f)) {
			foreach (var contact in collision.contacts) {
				deformableMesh.AddDepression (contact.point, 0.3f);
			}
		}
	}
}
