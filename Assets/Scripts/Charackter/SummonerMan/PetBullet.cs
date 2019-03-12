using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetBullet : MonoBehaviour {

    public int id;
    public float damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Object") other.SendMessage("TakeDamage");
        if (other.tag == "Object" || other.tag == "Map" || other.tag == "Arena" || other.tag == "LootBox")
        {
            if (id == 1)
            {
                foreach (Collider col in Physics.OverlapSphere(transform.position, 2.6f))
                {
                    if (col.tag == "Enemy" || col.tag == "Boss")
                    {
                        col.SendMessage("AddDamage", damage);
                    }
                    else if (col.tag == "Object")
                    {
                        col.SendMessage("TakeDamage");
                    }
                }
            }
            Destroy(this.gameObject, 0.05f);
        }
        else if ((other.tag == "Enemy" || other.tag == "Boss"))
        {
            if (id == 0)
            {
                other.SendMessage("AddDamage", damage);
            }
        }
    }

    private void OnDestroy()
    {
        
    }

    // Update is called once per frame
    void Update () {
        if (id == 0)
        {
            gameObject.transform.Translate(Vector3.forward * 5 * Time.deltaTime);
            gameObject.transform.Rotate(Vector3.right * 10 * Time.deltaTime);
        } 
    }
}
