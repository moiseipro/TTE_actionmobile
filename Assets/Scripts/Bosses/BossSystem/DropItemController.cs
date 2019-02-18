using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemController : MonoBehaviour {

    public GameObject[] dropItemWarMan;


    public GameObject DropItem()
    {
        GameObject newItem = dropItemWarMan[Random.Range(0, dropItemWarMan.Length)];
        float dropChance = Random.Range(0.5f, 2f);
        if (dropChance > 1.5f) dropChance = Random.Range(0.7f, dropChance);
        else if (dropChance > 1.2f) dropChance = Random.Range(0.6f, dropChance);
        else if (dropChance > 1f) dropChance = Random.Range(0.5f, dropChance);
        dropChance = (float)System.Math.Round((decimal)dropChance,2);

        if (dropChance > 1.5){
            newItem.GetComponent<Outline>().OutlineColor = Color.yellow;
        } else if (dropChance > 1.1) {
            newItem.GetComponent<Outline>().OutlineColor = Color.blue;
        } else if (dropChance > 0.7) {
            newItem.GetComponent<Outline>().OutlineColor = Color.green;
        } else if (dropChance >= 0.5) {
            newItem.GetComponent<Outline>().OutlineColor = Color.white;
        }
        newItem.GetComponent<CharackterItem>().Rang = dropChance;
        return newItem;
    }

    //public GameObject DropItemPerChance(int )
    //{

    //}

}
