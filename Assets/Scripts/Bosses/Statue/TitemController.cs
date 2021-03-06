﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitemController : MonoBehaviour {

    public GameObject attributeTotems;
    GameObject bossG, playerTarget;

    public string totemName;
    public int idTotem;
    public float maxHealth;
    private float health;
    [HideInInspector]
    public int id;

    public int damage;
    public bool periodDamage = false;
    float laserRange = 30f;

    private LineRenderer linerender;

    // Use this for initialization
    void Start() {
        bossG = GameObject.FindWithTag("Boss");
        playerTarget = GameObject.FindWithTag("Player");
        health = maxHealth;
        if (totemName == "faces")
        {
            linerender = gameObject.AddComponent<LineRenderer>();
            linerender.widthMultiplier = 0.2f;
            linerender.material = Resources.Load("Materials/Statue/LaserFacesTotem") as Material;
            linerender.positionCount = 6;
            linerender.loop = true;
            StartCoroutine(facesTotemLaser());
        } else if (totemName == "heal")
        {
            linerender = gameObject.AddComponent<LineRenderer>();
            linerender.widthMultiplier = 0.2f;
            //linerender.numCornerVertices = 10;
            linerender.material = Resources.Load("Materials/Statue/LaserGuardTotem") as Material;
            linerender.positionCount = 2;
            linerender.SetPosition(0, gameObject.transform.position + Vector3.up);
            linerender.SetPosition(1, gameObject.transform.position + Vector3.up);
        } else if (totemName == "skull") {
            skullTotem();
        }
    }

    // Update is called once per frame
    void Update() {
        if (totemName == "faces" && periodDamage == false) {
            facesTotem();
        } else if (totemName == "heal") {
            healTotem();
        } else if (totemName == "poison" && periodDamage == false) {
            RenderSpherePoison();
        } else if (totemName == "thunder") {
            thunderTotem();
        } else if (totemName == "lamp") {
            lampTotem();
        }

    }

    public void atackTotem() {
        GameObject Bull = GameObject.Instantiate(attributeTotems, transform.position + Vector3.up, transform.rotation);
        Bull.transform.LookAt(playerTarget.GetComponent<Transform>());
        Bull.GetComponent<Bullet_Options>().speed = 10;
        Bull.GetComponent<Bullet_Options>().rotationSpeed = 0;
        Bull.GetComponent<Bullet_Options>().damage = damage;
        Bull.GetComponent<Bullet_Options>().type = -1;
        Destroy(Bull, 5f);
    }

    void facesTotem() {
        RayCastDirection(Vector3.forward);
        RayCastDirection(Vector3.left);
        RayCastDirection(Vector3.back);
        RayCastDirection(Vector3.right);
    }

    public IEnumerator facesTotemLaser()
    {
        while (bossG.GetComponent<BossHeartController>().dead == false && health > 1)
        {
            linerender.SetPosition(0, gameObject.transform.position + Vector3.up);
            linerender.SetPosition(1, LineRendererCastDirection(Vector3.forward));
            linerender.SetPosition(2, LineRendererCastDirection(Vector3.back));
            linerender.SetPosition(3, gameObject.transform.position + Vector3.up);
            linerender.SetPosition(4, LineRendererCastDirection(Vector3.right));
            linerender.SetPosition(5, LineRendererCastDirection(Vector3.left));
            yield return new WaitForSeconds(1f);
        }
    }

    void healTotem()
    {
        Vector3 vect = bossG.transform.position - transform.position;
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 1.8f, transform.TransformDirection(vect.normalized), out hit, Mathf.Infinity))
        {
            if(hit.transform.gameObject.tag == "Player")
            {
                linerender.SetPosition(0, attributeTotems.transform.position);
                linerender.SetPosition(1, Vector3.Lerp(linerender.GetPosition(1), new Vector3(hit.transform.position.x, attributeTotems.transform.position.y, hit.transform.position.z), Time.deltaTime * 20f));
            } else if(hit.transform.gameObject.tag == "Boss")
            {
                linerender.SetPosition(0, attributeTotems.transform.position);
                linerender.SetPosition(1, Vector3.Lerp(linerender.GetPosition(1), new Vector3(hit.transform.position.x, attributeTotems.transform.position.y, hit.transform.position.z), Time.deltaTime * 20f));
                if (periodDamage == false)
                {
                    //Debug.Log("Босс хилится");
                    periodDamage = true;
                    hit.transform.GetComponent<BossHeartController>().HealBoss(5f);
                    StartCoroutine(PlayerDamagePerTime(1f));
                }
            }
        }
    }

    void skullTotem()
    {
        playerTarget.GetComponent<Move_Controller>().StaticSpeedDebaf(17f);
    }

    void thunderTotem()
    {
        Vector3 pos = gameObject.transform.position - playerTarget.transform.position;
        playerTarget.GetComponent<CharacterController>().Move(pos.normalized*1.5f * Time.deltaTime);
    }

    void lampTotem()
    {
        if (periodDamage == false)
        {
            periodDamage = true;
            GameObject muha = Instantiate(attributeTotems, transform.position + Vector3.up/2f, Quaternion.identity);
            muha.GetComponent<FlyController>().damage = damage;
            StartCoroutine(PlayerDamagePerTime(4f));
        }
    }

    void RayCastDirection(Vector3 dir) {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 1.3f, transform.TransformDirection(dir), out hit, Mathf.Infinity) && hit.transform.gameObject.tag == "Player")
        {
            periodDamage = true;
            Debug.Log("Жжется");
            hit.transform.gameObject.GetComponent<HeartSystem>().TakeDamage(-damage);
            StartCoroutine(PlayerDamagePerTime(1f));
        }
    }
    public Vector3 LineRendererCastDirection(Vector3 dir)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 1.3f, transform.TransformDirection(dir), out hit, Mathf.Infinity) && (hit.transform.gameObject.tag == "Object" || hit.transform.gameObject.tag == "Enemy" || hit.transform.gameObject.tag == "Map" || hit.transform.gameObject.tag == "Boss"))
        {
            if (dir == Vector3.left || dir == Vector3.right) return new Vector3(hit.point.x, transform.position.y + 1f, transform.position.z);
            else if(dir == Vector3.forward || dir == Vector3.back) return new Vector3(transform.position.x, transform.position.y + 1f, hit.point.z);
        }
        return dir * laserRange + gameObject.transform.position + Vector3.up;
    }

    void RenderSpherePoison()
    {
        foreach(Collider col in Physics.OverlapSphere(transform.position, 3f))
        {
            if(col.tag == "Player")
            {
                periodDamage = true;
                Debug.Log("Отрава");
                col.GetComponent<HeartSystem>().TakeDamage(-damage);
                StartCoroutine(PlayerDamagePerTime(1.5f));
            }
        }
    }

    public IEnumerator PlayerDamagePerTime(float time)
    {
        yield return new WaitForSeconds(time);
        periodDamage = false;
    }

    public void AddDamage(float dam)
    {
        Debug.Log(health);
        if (health > 0)
        {
            health -= dam;
            Debug.Log(health);
        }
        if (health < 1)
        {
            health = 0;
            Debug.Log("тотем СМЭРТЬ");
            bossG.GetComponent<StatueController>().DeliteTotem(id);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (totemName == "guard") bossG.GetComponent<StatueController>().immortality = false;
        else if (totemName == "skull") playerTarget.GetComponent<Move_Controller>().StaticSpeedDebaf(0f);
        else if (totemName == "faces") StopAllCoroutines();
    }
}
