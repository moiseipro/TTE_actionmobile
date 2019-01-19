using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : BossHeartController {

    Rigidbody rb;
    public Mesh[] slimeMesh;
    public GameObject[] bossAttribute;


    bool isMadded = false,
         isMove = false,
         isPreparing = false;

    float skillBossKd = 10f;
    float maxPreparingTime = 5f;
    public float movSpeed = 10f;

    // Use this for initialization
    void Start () {
        StartScript();
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        UpdateHpContainers();
        if (isMadded == false && RadiusStartAtack(7))
        {
            isMadded = true;
            StartCoroutine(Move());
        } else if (isPreparing == false && isMove == true && isMadded == true && RadiusStartAtack(4))
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
            yield return new WaitForSeconds(2f);
            //transform.LookAt(Player.transform);
            //rb.AddForce(Player.transform.position-gameObject.transform.position, ForceMode.Impulse);
            rb.AddForce((gameObject.transform.forward * movSpeed) + Vector3.up, ForceMode.Impulse);
            AnimationChoose(1);
            yield return new WaitForSeconds(0.5f);
        }
        StartCoroutine(Preparing());
    }

    IEnumerator Preparing()
    {
        AnimationChoose(3);
        yield return new WaitForSeconds(Random.Range(maxPreparingTime/2f,maxPreparingTime));
        StartCoroutine(SpitSlime());
    }

    IEnumerator SpitSlime()
    {
        AnimationChoose(2);
        GameObject bullSlime = GameObject.Instantiate(bossAttribute[0], transform.position + Vector3.up/2f, transform.rotation);
        bullSlime.transform.Rotate(Vector3.right * -15);
        bullSlime.GetComponent<Bullet_Options>().speed = 6;
        bullSlime.GetComponent<Bullet_Options>().rotationSpeed = 30;
        bullSlime.GetComponent<Bullet_Options>().damage = 2;
        bullSlime.GetComponent<Bullet_Options>().type = -2;
        Destroy(bullSlime, 5f);
        Debug.Log("Слизь полетела");
        yield return new WaitForSeconds(1f);
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
}
