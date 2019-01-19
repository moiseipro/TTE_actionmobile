using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSlimeController : MobController {

    float jumpKd = 1f;

    bool dead = false;

	// Use this for initialization
	void Start () {
        health = maxHealth;
        rb = gameObject.GetComponent<Rigidbody>();
        Player = GameObject.FindWithTag("Player");
        StartCoroutine(Move());
	}

    void FixedUpdate()
    {
        Vector3 movVec = Player.transform.position - gameObject.transform.position + GameObject.FindWithTag("Player").GetComponent<Move_Controller>().GetVectorMove() / 10f;
        Vector3 rotVec = Vector3.RotateTowards(transform.forward, movVec, 0.1f, 0f);
        rotVec.y = 0;
        transform.rotation = Quaternion.LookRotation(rotVec);
    }

    IEnumerator Move()
    {
        while (!dead)
        {
            yield return new WaitForSeconds(1f);
            AnimationChoose(1);
            rb.AddForce((gameObject.transform.forward + Vector3.up)*movSpeed, ForceMode.Impulse);
            yield return new WaitForSeconds(1f);
            AnimationChoose(0);
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            dead = false;
            GameObject.FindWithTag("Player").GetComponent<HeartSystem>().TakeDamage(-damage);
            Destroy(this.gameObject, 0.1f);
        }
    }

}
