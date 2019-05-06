using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementhalController : BossHeartController {

    bool isMove, isMadded, isStalk, isRage, isThrow;

    float skillKd = 2f;
    public float movSpeed = 1f;
    public float rotSpeed = 0.5f;

    float speedSkillCast = 1f;
    int rage = 0;

    Animator animator;
    Move_Controller mc;

    public Transform targetSplit;

	// Use this for initialization
	void Start () {
        StartScript();
        animator = GetComponentInChildren<Animator>();
        mc = Player.GetComponent<Move_Controller>();
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
        if (isStalk)
        {
            Vector3 dir = Player.transform.position - transform.position;
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, rotSpeed/100f);
        }
        if (isMove)
        {
            Vector3 vec = mc.GetVectorMove();
            vec.y = 0;
            Vector3 atackDist = Player.transform.position - transform.position + vec;
            //Debug.Log(atackDist.magnitude);
            if(atackDist.magnitude > 1) transform.Translate(atackDist * movSpeed/100f, Space.World);
        }
        if (isThrow)
        {
            if (targetSplit == null)
            {
                RadiusForSplit(7);
            }
            if (targetSplit != null)
            {
                Vector3 dir = targetSplit.transform.position - transform.position;
                Quaternion rot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Lerp(transform.rotation, rot, rotSpeed / 10f);
                if (dir.magnitude > 1) transform.Translate(dir * movSpeed / 25f, Space.World);
                else
                {
                    isThrow = false;
                    StartCoroutine(MeteoriteTake());
                }
            } else {
                isThrow = false;
                StartCoroutine(Move());
            }
        }
    }

    IEnumerator Move()
    {
        isStalk = true;
        isMove = true;
        int skill = Random.Range(0, 2);
        if (skill == 0) animator.SetTrigger("Move");
        else animator.SetTrigger("MoveAtack");
        Debug.Log("Ифрит идет");
        yield return new WaitForSeconds(Random.Range(1.5f, 3f));
        skill = Random.Range(0, 11);
        if (/*rage >= 100 &&*/ skill < 3 && isRage == false) StartCoroutine(SkillCast());
        else if (skill < 6) StartCoroutine(EarthFault());
        else if (skill < 8) StartCoroutine(MeteoriteMove());
        else StartCoroutine(Move());
    }

    IEnumerator SkillCast()
    {
        yield return new WaitForSeconds(0.1f);
        isStalk = false;
        isMove = false;
        animator.SetTrigger("SkillCast");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + 0.1f);
        StartCoroutine(Rage());
        StartCoroutine(Move());
    }

    IEnumerator Rage()
    {
        Debug.Log("Ифрит в ярости");
        isRage = true;
        animator.speed = 2;
        movSpeed += 0.5f;
        rotSpeed += 5f;
        yield return new WaitForSeconds(Random.Range(3f, 8f));
        movSpeed -= 0.5f;
        rotSpeed -= 5f;
        animator.speed = 1;
        isRage = false;
    }

    IEnumerator EarthFault()
    {
        yield return new WaitForSeconds(0.1f);
        isStalk = true;
        isMove = false;
        animator.SetTrigger("Fault");
        Debug.Log("Ифрит бьет по земле");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        StartCoroutine(Move());
    }

    IEnumerator MeteoriteMove()
    {
        yield return new WaitForSeconds(0.1f);
        isStalk = false;
        isMove = false;
        isThrow = true;
        animator.SetTrigger("Move");
        Debug.Log("Ифрит идет к разлому");
    }

    IEnumerator MeteoriteTake()
    {
        yield return new WaitForSeconds(0.1f);
        animator.SetTrigger("Take");
        isStalk = false;
        Debug.Log("Ифрит берет камень");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        StartCoroutine(MeteoriteRain());
    }

    IEnumerator MeteoriteRain()
    {
        yield return new WaitForSeconds(0.1f);
        animator.SetTrigger("Throw");
        isStalk = true;
        Debug.Log("Ифрит кидает камень");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        int skill = Random.Range(0, 11);
        if(skill < 7) isThrow = true;
        else StartCoroutine(Move());
    }

    bool RadiusForSplit(float radius)
    {
        foreach (Collider col in Physics.OverlapSphere(transform.position, radius))
        {
            if (col.tag == "Trap")
            {
                targetSplit = col.transform;
                return true;
            }
        }
        return false;
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
