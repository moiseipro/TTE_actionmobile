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
        else if (rdrop > 6) rdrop = Random.Range(0, 5);
        else if (rdrop > 3) rdrop = Random.Range(0, 3);
        else rdrop = Random.Range(0, 2);
        for (int i = 0; i < rdrop; i++)
        {
            Vector3 ops = new Vector3(player.transform.position.x + Random.Range(-2f, 2f), 2f, player.transform.position.z + Random.Range(-2f,2f));
            GameObject itemDop = Instantiate(dic.DropItemChest(boostChance), gameObject.transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
            itemDop.GetComponent<Rigidbody>().AddForce((ops - gameObject.transform.position) * 1.5f, ForceMode.Impulse);
            yield return new WaitForSeconds(0.5f);
        }
        GameObject item = Instantiate(dic.DropItem(maxChance), gameObject.transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
        item.GetComponent<Rigidbody>().AddForce((player.transform.position - gameObject.transform.position) * 1.5f, ForceMode.Impulse);
    }
}
