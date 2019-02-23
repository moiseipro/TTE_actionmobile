using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemController : MonoBehaviour {

    public GameObject[] dropItemWarMan;
    [Header("Общие предметы для персонажей")]
    public GameObject[] dropItemHeal;
    public GameObject[] dropItemMoney;
    public GameObject[] dropItemKey;
    public GameObject[] dropItemAddHealth;

    private const float minDropChance = 0.5f;
    private const float maxDropChance = 2f;

    public GameObject DropItem(float maxChance)
    {
        GameObject newItem = dropItemWarMan[Random.Range(0, dropItemWarMan.Length)];
        float dropChance = Random.Range(minDropChance, Random.Range(maxChance, maxDropChance));
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

    public GameObject DropItemChest(int num)
    {
        GameObject newDopItem = dropItemHeal[Random.Range(0, dropItemHeal.Length)];
        int dropChance = Random.Range(0, 101);
        if (dropChance > 99-num)
        {
            newDopItem = dropItemKey[Random.Range(0, dropItemKey.Length)];
        } else if (dropChance > 25)
        {
            int dropTwoLevel = Random.Range(0, 11);
            if(dropTwoLevel > 8)
            {
                newDopItem = dropItemAddHealth[Random.Range(0, dropItemAddHealth.Length)];
            } else {
                newDopItem = dropItemMoney[Random.Range(0, dropItemMoney.Length)];
            }
            
        }

        return newDopItem;
    }

}
