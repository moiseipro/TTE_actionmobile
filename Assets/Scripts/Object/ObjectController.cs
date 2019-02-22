using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour {

    public float maxHealth = 4;
    private float health;

    SceneController gameManager;
    Animator anim;
    ParticleSystem ps;

    private void Start()
    {
        ps = GetComponentInChildren<ParticleSystem>();
        gameManager = GameObject.FindWithTag("Manager").GetComponent<SceneController>();
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
            anim.SetTrigger("Dead");
            ps.Play();
        }
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
