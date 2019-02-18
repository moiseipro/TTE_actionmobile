using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBoxController : MonoBehaviour {

    public Animator lockanim;
    public int idChest;

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
        GameObject item = Instantiate(dic.DropItem(), gameObject.transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
        item.GetComponent<Rigidbody>().AddForce((player.transform.position-gameObject.transform.position)*1.5f,ForceMode.Impulse);
    }
}
