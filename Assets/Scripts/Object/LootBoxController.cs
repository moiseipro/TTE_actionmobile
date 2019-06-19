using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBoxController : MonoBehaviour {

    public Animator lockanim;
    [Range(0, 10)]
    public int boostChance;
    [Range(0.5f, 2f)]
    public float maxChance = 0.6f;

    Animator anim;
    bool opened = false;
    DropItemController dic;
    GameObject player;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        dic = GameObject.FindWithTag("Manager").GetComponent<DropItemController>();
        player = GameObject.FindWithTag("Player");
    }

    public void OpenChest()
    {
        if (opened == false)
        {
            anim.SetTrigger("Open");
            lockanim.SetTrigger("Break");
            opened = true;
        }
    }

    public void GetLoot()
    {
        StartCoroutine(DropTimer());
    }

    IEnumerator DropTimer()
    {
        int rdrop = Random.Range(0, boostChance);
        if (rdrop > 9) rdrop = Random.Range(2, 5);
        else if (rdrop > 6) rdrop = Random.Range(1, 5);
        else if (rdrop > 3) rdrop = Random.Range(0, 3);
        else rdrop = Random.Range(0, 2);
        Vector3 ops = Vector3.zero;
        float xmin, xmax, ymin ,ymax;
        int randomSpawn;
        for (int i = 0; i < rdrop; i++)
        {
            xmin = Random.Range(-2.8f, -2f);
            xmax = Random.Range(2f, 2.8f);
            ymin = Random.Range(-2.8f, -2f);
            ymax = Random.Range(2f, 2.8f);
            randomSpawn = Random.Range(0, 11);
            if (randomSpawn > 7) ops = new Vector3(xmin, 2f, ymin);
            else if (randomSpawn > 5) ops = new Vector3(xmin, 2f, ymax);
            else if (randomSpawn > 3) ops = new Vector3(xmax, 2f, ymin);
            else ops = new Vector3(xmax, 2f, ymax);
            GameObject itemDop = Instantiate(dic.DropItemChest(boostChance), gameObject.transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
            itemDop.GetComponent<Rigidbody>().AddForce(ops, ForceMode.Impulse);
            yield return new WaitForSeconds(0.5f);
        }
        xmin = Random.Range(-2.8f, -2f);
        xmax = Random.Range(2f, 2.8f);
        ymin = Random.Range(-2.8f, -2f);
        ymax = Random.Range(2f, 2.8f);
        randomSpawn = Random.Range(0, 11);
        if (randomSpawn > 7) ops = new Vector3(xmin, 2f, ymin);
        else if (randomSpawn > 5) ops = new Vector3(xmin, 2f, ymax);
        else if (randomSpawn > 3) ops = new Vector3(xmax, 2f, ymin);
        else ops = new Vector3(xmax, 2f, ymax);
        GameObject item = Instantiate(dic.DropItem(maxChance), gameObject.transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
        item.GetComponent<Rigidbody>().AddForce(ops, ForceMode.Impulse);
    }
}
