using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : BossHeartController {

    Rigidbody rb;
    public Mesh[] slimeMesh;


    bool isMadded = false,
         isMove = false,
         isPreparing = false;
    [HideInInspector] public bool isAbsorb = false;

    int predSkillUse = 0;
    float skillBossKd = 10f;
    float maxPreparingTime = 1f;
    public float movSpeed = 10f;

    // Use this for initialization
    void Start () {
        StartScript();
        maxArmor = health / 2.5f;
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        BossFightStartRadius();
        UpdateHpContainers();
        if (isMadded == false && RadiusStartAtack(7))
        {
            isMadded = true;
            StartCoroutine(Move());
        } else if (isPreparing == false && isMove == true && isMadded == true && RadiusStartAtack(5))
        {
            isMove = false;
            isPreparing = true;
        }
        if(isPreparing == false && rb.velocity.magnitude > 0.4f)
        {
            Vector3 movVec = Player.transform.position - gameObject.transform.position;
            Vector3 rotVec = Vector3.RotateTowards(transform.forward, movVec, 0.1f, 0f);
            rotVec.y = 0;
            transform.rotation = Quaternion.LookRotation(rotVec);
        } else if (isPreparing == true)
        {
            Vector3 movVec = Player.transform.position - gameObject.transform.position + GameObject.FindWithTag("Player").GetComponent<Move_Controller>().GetVectorMove() / 10f;
            Vector3 rotVec = Vector3.RotateTowards(transform.forward, movVec, 0.1f, 0f);
            rotVec.y = 0;
            transform.rotation = Quaternion.LookRotation(rotVec);
        }
    }

    IEnumerator Move()
    {
        isMove = true;
        while (isMove)
        {
            AnimationChoose(0);
            yield return new WaitForSeconds(1.5f);
            //transform.LookAt(Player.transform);
            //rb.AddForce(Player.transform.position-gameObject.transform.position, ForceMode.Impulse);
            rb.AddForce((gameObject.transform.forward * movSpeed) + Vector3.up, ForceMode.Impulse);
            AnimationChoose(1);
            yield return new WaitForSeconds(0.5f);
        }
        StartCoroutine(Preparing());
    }

    IEnumerator BoostMove()
    {
        //AnimationChoose(0);
        //yield return new WaitForSeconds(1.5f);
        //transform.LookAt(Player.transform);
        rb.AddForce((Player.transform.position - gameObject.transform.position)*1.4f + Vector3.up, ForceMode.Impulse);
        //rb.AddForce((gameObject.transform.forward * movSpeed) + Vector3.up, ForceMode.Impulse);
        AnimationChoose(1);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(Preparing());
    }

    IEnumerator Preparing()
    {
        AnimationChoose(3);
        yield return new WaitForSeconds(Random.Range(maxPreparingTime/5f,maxPreparingTime));
        int skillNumber = Random.Range(0, 4);
        if (predSkillUse == 1) skillNumber = 3;
        else if(predSkillUse == skillNumber) skillNumber = Random.Range(0, 3);
        predSkillUse = skillNumber;
        switch (skillNumber) {
            case 0:
                StartCoroutine(SpitSlime());
                break;
            case 1:
                StartCoroutine(ArmorBoom());
                break;
            case 2:
                StartCoroutine(BoostMove());
                break;
            case 3:
                StartCoroutine(AbsorbSlime());
                break;
        }
    }

    IEnumerator SpitSlime()
    {
        AnimationChoose(2);
        GameObject bullSlime = Instantiate(Resources.Load("Prefabs/Bosses/Slime/BossSlimeBullet") as GameObject, transform.position + Vector3.up/2f, transform.rotation);
        bullSlime.transform.Rotate(Vector3.right * -15);
        bullSlime.GetComponent<Bullet_Options>().speed = 6;
        bullSlime.GetComponent<Bullet_Options>().rotationSpeed = 30;
        bullSlime.GetComponent<Bullet_Options>().damage = 2;
        bullSlime.GetComponent<Bullet_Options>().type = -2;
        Destroy(bullSlime, 5f);
        Debug.Log("Слизь полетела");
        yield return new WaitForSeconds(0.2f);
        isPreparing = false;
        StartCoroutine(Move());
    }

    IEnumerator ArmorBoom()
    {
        AnimationChoose(4);
        armor = maxArmor;
        Debug.Log("Слизь разбушевалась");
        int valueSlimeSpawn = Random.Range(5,10), i = 0;
        while (armor > 1 && i < valueSlimeSpawn)
        {
            yield return new WaitForSeconds(1f);
            i++;
            GameObject bullSlime = Instantiate(Resources.Load("Prefabs/Bosses/Slime/BossSlimeBullet") as GameObject, transform.position + Vector3.up, Quaternion.identity);
            bullSlime.transform.Rotate(Vector3.up * Random.Range(0, 360f), Space.Self);
            bullSlime.transform.Rotate(Vector3.right * -25, Space.Self);
            bullSlime.GetComponent<Bullet_Options>().speed = 6;
            bullSlime.GetComponent<Bullet_Options>().rotationSpeed = 30;
            bullSlime.GetComponent<Bullet_Options>().damage = 2;
            bullSlime.GetComponent<Bullet_Options>().type = -2;
            Destroy(bullSlime, 5f);
        }
        yield return new WaitForSeconds(0.2f);
        if (i >= valueSlimeSpawn) {
            GameObject puddleSlime = Instantiate(Resources.Load("Prefabs/Bosses/Slime/SlimePuddle") as GameObject, transform.position, Quaternion.identity);
            Destroy(puddleSlime, 15f);
            armor = 0;
        }
        isPreparing = false;
        StartCoroutine(Move());
    }

    IEnumerator AbsorbSlime()
    {
        AnimationChoose(5);
        isAbsorb = true;
        float timeHealth = health;
        Debug.Log("Слизь втягивает");
        while (isAbsorb == true && timeHealth-health <30 + bossLevel*2f)
        {
            yield return new WaitForSeconds(0.01f);
            Vector3 pos = transform.position - transform.forward - Player.transform.position;
            Player.GetComponent<CharacterController>().Move(pos.normalized * 2.5f * Time.deltaTime);
            rb.AddForce((Player.transform.position - Player.transform.forward-transform.position + Vector3.up* 0.05f) * movSpeed/3f, ForceMode.Force);
        }
        yield return new WaitForSeconds(0.2f);
        isAbsorb = false;
        isPreparing = false;
        StartCoroutine(Move());
    }

    bool RadiusStartAtack(float radius)
    {
        foreach (Collider col in Physics.OverlapSphere(transform.position, radius))
        {
            if (col.tag == "Player")
            {
                Debug.Log("Возможна атака слизи");
                return true;
            }
        }
        return false;
    }

    public void AnimationChoose(int num)
    {
        if (num < slimeMesh.Length && num >=0)
        {
            GetComponent<MeshFilter>().sharedMesh = slimeMesh[num];
        }
        else
        {
            Debug.Log("Такой анимации нет");
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<HeartSystem>().TakeDamage(-damage);
            Vector3 pos = other.gameObject.transform.position - gameObject.transform.position;
            other.gameObject.GetComponent<CharacterController>().Move(pos.normalized * 1.5f);
            Debug.Log("Врезался в босса");
            isAbsorb = false;
        }
    }
}
