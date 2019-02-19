using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBoxController : MonoBehaviour {

    public Animator lockanim;
    [Range(0, 30)]
    public int boostChance;

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
        int rdrop = Random.Range(0, 5);
        for(int i = 0; i < rdrop; i++)
        {
            GameObject itemDop = Instantiate(dic.DropItemChest(boostChance), gameObject.transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
            itemDop.GetComponent<Rigidbody>().AddForce(Random.Range(-2f, 2f), 3f, Random.Range(-2f, 2f), ForceMode.Impulse);
        }
        GameObject item = Instantiate(dic.DropItem(), gameObject.transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
        item.GetComponent<Rigidbody>().AddForce((player.transform.position-gameObject.transform.position)*1.5f,ForceMode.Impulse);
    }
}
