using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSlimeController : MobController {

    float jumpKd = 1f;
    public float heal = 10f;

    bool dead = false;

    SlimeController sc;
    BossHeartController bhc;
    Move_Controller playerMC;
    BaffController baffController;

	// Use this for initialization
	void Start () {
        SearchForCh();
        sc = Boss.GetComponent<SlimeController>();
        bhc = Boss.GetComponent<BossHeartController>();
        playerMC = Player.GetComponent<Move_Controller>();
        baffController = Player.GetComponent<BaffController>();
        health = maxHealth;
        StartCoroutine(Move());
	}

    void FixedUpdate()
    {
        Vector3 movVec = Player.transform.position - gameObject.transform.position + playerMC.GetVectorMove() / 10f;
        Vector3 rotVec = Vector3.RotateTowards(transform.forward, movVec, 0.1f, 0f);
        rotVec.y = 0;
        transform.rotation = Quaternion.LookRotation(rotVec);
        if(Boss != null && sc.isAbsorb == true)
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
            //Player.GetComponent<MonoBehaviour>().StartCoroutine(Player.GetComponent<Move_Controller>().SpeedDebaf(15f, 3f));
            baffController.CreateBaff(3,15,2);
            Player.GetComponent<HeartSystem>().TakeDamage(-damage);
            Destroy(gameObject);
        } else if (other.gameObject.tag == "Boss")
        {
            if (sc.isAbsorb == false)
            {
                rb.AddForce((gameObject.transform.position - Boss.transform.position + Vector3.up * 0.5f).normalized * movSpeed, ForceMode.Impulse);
                //AddDamage(maxHealth / 4f);
            } else
            {
                bhc.HealBoss(heal);
                Destroy(gameObject);
            }
        }
    }

}
