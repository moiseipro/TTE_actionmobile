using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour {

    public float maxHealth = 4;
    private float health;

    SceneController gameManager;
    Animator anim;
    ParticleSystem ps;
    DropItemController dic;

    private void Start()
    {
        ps = GetComponentInChildren<ParticleSystem>();
        gameManager = GameObject.FindWithTag("Manager").GetComponent<SceneController>();
        dic = GameObject.FindWithTag("Manager").GetComponent<DropItemController>();
        anim = GetComponent<Animator>();
        health = maxHealth;
    }

    public void TakeDamage()
    {
        if (health > 0)
        {
            anim.SetTrigger("Damage");
            health--;
        }
        else if (health == 0)
        {
            health = -1;
            anim.SetTrigger("Dead");
            ps.Play();
        }
    }

    public void DestroyObject()
    {
        int dropChanse = Random.Range(0, 11);
        if (dropChanse > 6)
        {
            GameObject itemDop = Instantiate(dic.DropItemChest(1), gameObject.transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
            itemDop.GetComponent<Rigidbody>().AddForce(new Vector3(0, 3f, 0), ForceMode.Impulse);
        }
        Destroy(gameObject);
    }
}
