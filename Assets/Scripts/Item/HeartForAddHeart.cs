using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartForAddHeart : MonoBehaviour {

    public int addHeartVal;

    Rigidbody rg;
    SphereCollider sc;
    Animator anim;
    HeartSystem hs;

    // Use this for initialization
    void Start()
    {
        rg = GetComponent<Rigidbody>();
        sc = GetComponent<SphereCollider>();
        anim = GetComponent<Animator>();
        hs = GameObject.FindWithTag("Player").GetComponent<HeartSystem>();
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
            hs.AddHeartContainer(addHeartVal);
        }
    }

    public void DestroyHeart()
    {
        Destroy(gameObject);
    }
}
