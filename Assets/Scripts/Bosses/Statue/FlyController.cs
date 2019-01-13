﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyController : MonoBehaviour {

    public float maxHealth;
    private float health;

    [HideInInspector] public int damage;

	// Use this for initialization
	void Start () {
        health = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 movVec = GameObject.FindWithTag("Player").transform.position - gameObject.transform.position + GameObject.FindWithTag("Player").GetComponent<Move_Controller>().GetVectorMove()/10f;
        Vector3 rotVec = Vector3.RotateTowards(transform.forward, movVec.normalized, 10f, 2f);
        movVec.y = 0;
        rotVec.y = 0;
        transform.Translate(-transform.forward * 2f * Time.deltaTime,Space.World);
        transform.rotation = Quaternion.LookRotation(-rotVec);
	}

    public void AddDamage(float damage)
    {
        health -= damage;
        health = Mathf.Clamp(health,0,maxHealth);
        if (health < 1)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameObject.FindWithTag("Player").GetComponent<HeartSystem>().TakeDamage(-damage);
            Destroy(this.gameObject, 0.1f);
        }
    }
}