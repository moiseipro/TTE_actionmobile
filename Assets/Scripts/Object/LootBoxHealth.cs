using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBoxHealth : MonoBehaviour {

    public Mesh mesh;

    Animator anim;
    MeshFilter meshfil;
    Rigidbody rg;
    BoxCollider bc;

    public void Start()
    {
        meshfil = gameObject.GetComponentInChildren<MeshFilter>();
        rg = gameObject.GetComponentInChildren<Rigidbody>();
        bc = gameObject.GetComponentInChildren<BoxCollider>();
        anim = gameObject.GetComponentInChildren<Animator>();
    }

    public void Unlock()
    {
        meshfil.sharedMesh = mesh;
    }

    public void BreakLock()
    {
        anim.enabled = false;
        rg.isKinematic = false;
        rg.AddForce(Random.Range(-1.5f, 1.5f), 2f, Random.Range(-1.5f, 1.5f), ForceMode.Impulse);
        rg.AddTorque(Random.Range(-3f, 3f), Random.Range(-3f, 3f), Random.Range(-3f, 3f), ForceMode.Impulse);
        Destroy(gameObject, 3f);
    }
}
