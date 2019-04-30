using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementhalController : BossHeartController {

    bool isMove, isMadded;

    float skillKd = 2f;
    public float movSpeed = 10f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        BossFightStartRadius();
        UpdateHpContainers();
        if (isMadded == false && RadiusStartAtack(7))
        {
            isMadded = true;
            StartCoroutine(Move());
        }
    }

    IEnumerator Move()
    {
        isMove = true;
        while (isMove)
        {
            yield return new WaitForSeconds(0.1f);
            //transform.LookAt(Player.transform);
            //rb.AddForce(Player.transform.position-gameObject.transform.position, ForceMode.Impulse);
            //rb.AddForce((gameObject.transform.forward * movSpeed) + Vector3.up, ForceMode.Impulse);
            yield return new WaitForSeconds(0.5f);
        }
        //StartCoroutine(Preparing());
    }

    bool RadiusStartAtack(float radius)
    {
        foreach (Collider col in Physics.OverlapSphere(transform.position, radius))
        {
            if (col.tag == "Player")
            {
                Debug.Log("Возможна атака ифрита");
                return true;
            }
        }
        return false;
    }
}
