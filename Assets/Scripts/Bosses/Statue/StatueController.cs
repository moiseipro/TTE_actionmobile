﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueController : MonoBehaviour {

    public GameObject[] totems;
    private BossHeartController bossHeart;

    int maxTotems; // Максимальное количество тотемов
    int calledTotems = 0; //Вызвано тотемов

    float health;
    float reloadTimeTotems = 10f;

    //Проверка на вызов тотемов
    bool atackTotemReload = false,
    guardTotemReload = false,
    facesTotemReload = true,
    poisonTotemReload = false,
    skullTotemReload = false,
    tunderTotemReload = false,
    healTotemReload = false,
    lampTotemReload = false;

    void Start () {
        bossHeart = GetComponent<BossHeartController>();
        maxTotems = 3 + bossHeart.bossLevel;
        StartCoroutine(ReloadTotem(2));
    }
	
	void Update () {
        if (calledTotems < maxTotems)
        {
            if (facesTotemReload == false) CallTotem(2);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            if (calledTotems < maxTotems)
            {
                if (guardTotemReload == false && bossHeart.health < bossHeart.maxHealth / 1.5f) CallTotem(1);
                else if (atackTotemReload == false) CallTotem(0);
                else if (facesTotemReload == false) CallTotem(2);
            }
        }
    }

    IEnumerator ReloadTotem(int totemID)
    {
        yield return new WaitForSeconds(reloadTimeTotems);
        if (totemID == 0) atackTotemReload = false;
        else if (totemID == 1) guardTotemReload = false;
        else if (totemID == 2) facesTotemReload = false;
        else if (totemID == 3) poisonTotemReload = false;
        else if (totemID == 4) skullTotemReload = false;
        else if (totemID == 5) tunderTotemReload = false;
        else if (totemID == 6) healTotemReload = false;
        else if (totemID == 7) lampTotemReload = false;
    }

    public void CallTotem(int totemID)
    {
        GameObject totem = Instantiate(totems[totemID], GenerateTotemSpawn(), transform.rotation);
        calledTotems++;
        if(totemID == 0) atackTotemReload = true;
        else if(totemID == 1) guardTotemReload = true;
        else if (totemID == 2) facesTotemReload = true;
        else if (totemID == 3) poisonTotemReload = true;
        else if (totemID == 4) skullTotemReload = true;
        else if (totemID == 5) tunderTotemReload = true;
        else if (totemID == 6) healTotemReload = true;
        else if (totemID == 7) lampTotemReload = true;
        StartCoroutine(ReloadTotem(totemID));
    }

    Vector3 GenerateTotemSpawn()
    {
        Vector3 spawnVec;
        do
        {
            spawnVec = new Vector3(Random.Range(gameObject.transform.position.x - 7f, gameObject.transform.position.x + 7f), gameObject.transform.position.y, Random.Range(gameObject.transform.position.z - 7f, gameObject.transform.position.z + 7f));
        } while (spawnVec.magnitude < 2.5f);
        return spawnVec;
    }
}
