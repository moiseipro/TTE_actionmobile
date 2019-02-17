using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour {

    public float maxHealth = 4;
    private float health;

    SceneController gameManager;
    Animator anim;

    private void Start()
    {
        gameManager = GameObject.FindWithTag("Manager").GetComponent<SceneController>();
        anim = GetComponent<Animator>();
        health = maxHealth;
    }

    public void TakeDamage()
    {
        if (health > 0) health--;
        else if (health == 0) Destroy(gameObject,0.2f);
        anim.SetTrigger("Damage");
    }
}
