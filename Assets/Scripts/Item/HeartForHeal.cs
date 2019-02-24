using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartForHeal : MonoBehaviour {

    public int healVal;

    Rigidbody rg;
    SphereCollider sc;
    Animator anim;
    HeartSystem hs;

	// Use this for initialization
	void Start () {
        rg = GetComponent<Rigidbody>();
        sc = GetComponent<SphereCollider>();
        anim = GetComponent<Animator>();
        hs = GameObject.FindWithTag("Player").GetComponent<HeartSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Map" || collision.gameObject.tag == "Object")
        {
            rg.isKinematic = true;
            sc.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (hs.CheckFull())
            {
                anim.SetTrigger("Add");
                hs.TakeDamage(healVal);
            }
        }
    }

    public void DestroyHeart()
    {
        Destroy(gameObject);
    }
}
