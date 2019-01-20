using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSlimeController : MobController {

    float jumpKd = 1f;
    public float heal = 10f;

    bool dead = false;

	// Use this for initialization
	void Start () {
        health = maxHealth;
        rb = gameObject.GetComponent<Rigidbody>();
        Player = GameObject.FindWithTag("Player");
        Boss = GameObject.FindWithTag("Boss");
        StartCoroutine(Move());
	}

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Vector3 movVec = Player.transform.position - gameObject.transform.position + GameObject.FindWithTag("Player").GetComponent<Move_Controller>().GetVectorMove() / 10f;
        Vector3 rotVec = Vector3.RotateTowards(transform.forward, movVec, 0.1f, 0f);
        rotVec.y = 0;
        transform.rotation = Quaternion.LookRotation(rotVec);
        if(Boss != null && Boss.GetComponent<SlimeController>().isAbsorb == true)
        {
            rb.AddForce((Boss.transform.position - gameObject.transform.position + Vector3.up * 0.01f) * movSpeed, ForceMode.Force);
        }
    }

    IEnumerator Move()
    {
        while (!dead)
        {
            yield return new WaitForSeconds(1f);
            AnimationChoose(1);
            rb.AddForce((gameObject.transform.forward + Vector3.up*0.8f)*movSpeed, ForceMode.Impulse);
            yield return new WaitForSeconds(1f);
            AnimationChoose(0);
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            dead = true;
            Player.GetComponent<HeartSystem>().TakeDamage(-damage);
            Destroy(this.gameObject);
        } else if (other.gameObject.tag == "Boss")
        {
            if (Boss.GetComponent<SlimeController>().isAbsorb == false)
            {
                rb.AddForce((gameObject.transform.position - Boss.transform.position + Vector3.up * 0.5f).normalized * movSpeed, ForceMode.Impulse);
                //AddDamage(maxHealth / 4f);
            } else
            {
                Boss.GetComponent<BossHeartController>().HealBoss(heal);
                Destroy(gameObject);
            }
        }
    }

}
