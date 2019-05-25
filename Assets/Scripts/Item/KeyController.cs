using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour {

    public int keyId;

    Rigidbody rg;
    SphereCollider sc;
    Animator anim;
    PlayerManager pm;

    // Use this for initialization
    void Start()
    {
        rg = GetComponent<Rigidbody>();
        sc = GetComponent<SphereCollider>();
        anim = GetComponent<Animator>();
        pm = GameObject.FindWithTag("Manager").GetComponent<PlayerManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Map" || collision.gameObject.tag == "Object" || collision.gameObject.tag == "Arena")
        {
            rg.isKinematic = true;
            sc.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            anim.SetTrigger("Add");
            pm.AddKey();
        }
    }

    public void DestroyHeart()
    {
        Destroy(gameObject);
    }
}
