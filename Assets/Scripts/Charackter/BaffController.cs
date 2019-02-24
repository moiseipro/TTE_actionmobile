using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaffController : MonoBehaviour {

    HeartSystem hs;
    Move_Controller mc;

	// Use this for initialization
	void Start () {
        hs = GetComponent<HeartSystem>();
        mc = GetComponent<Move_Controller>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CreateBaff(int time, int value, int idBaff) {
        if (idBaff == 0) StartCoroutine(tikDamage(time, value));
        else if (idBaff == 1) StartCoroutine(tikHeal(time, value));
        else if (idBaff == 2) StartCoroutine(tikSpeedDown(time, value));
    }

    public void StaticBaff(int value, int idBaff)
    {
        if (idBaff == 2) mc.staticSpeedDebaf = mc.speed * value / 100;
    }

    IEnumerator tikDamage(int time, int damage)
    {
        for(int i = 0; i < time; i++)
        {
            hs.TakeDamage(-damage);
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator tikHeal(int time, int heal)
    {
        for (int i = 0; i < time; i++)
        {
            hs.TakeDamage(heal);
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator tikSpeedDown(int time, int speedDown)
    {
        float speedSub = mc.speed * speedDown / 100;
        speedSub = (float)System.Math.Round((decimal)speedSub, 2);
        Debug.Log("Дебаф скорости: "+ speedSub);
        mc.speedDebaf += speedSub;
        yield return new WaitForSeconds(time);
        mc.speedDebaf -= speedSub;
    }

}
