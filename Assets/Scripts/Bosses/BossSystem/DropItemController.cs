using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemController : MonoBehaviour {

    private GameObject[] dropItemForChar;
    public List<GameObject> availableArtifact;
    [HideInInspector] public SaveSystem saveSystem;
    [Header("Общие предметы для персонажей")]
    public GameObject[] dropItemHeal;
    public GameObject[] dropItemMoney;
    public GameObject[] dropItemKey;
    public GameObject[] dropItemAddHealth;

    private int artifactDropChance = 30;

    private const float minDropChance = 0.5f;
    private const float maxDropChance = 2f;

    private void Start()
    {
        if (PlayerPrefs.GetInt("PalyerCharackter") == 0)
        {

            dropItemForChar = Resources.LoadAll<GameObject>("Prefabs/Charackter/WarMan/Upgrade") as GameObject[];
            availableArtifact.AddRange(Resources.LoadAll<GameObject>("Prefabs/Charackter/WarMan/Artifact") as GameObject[]);
        } else if (PlayerPrefs.GetInt("PalyerCharackter") == 1)
        {
            dropItemForChar = Resources.LoadAll<GameObject>("Prefabs/Charackter/SummonerMan/Upgrade") as GameObject[];
            availableArtifact.AddRange(Resources.LoadAll<GameObject>("Prefabs/Charackter/SummonerMan/Artifact") as GameObject[]);
        }
        saveSystem = GetComponent<SaveSystem>();
        saveSystem.LoadFile();
        for(int i = saveSystem.sa.activeArtifact.Length - 1; i > -1 ; i--)
        {
            if (saveSystem.sa.activeArtifact[i] == false) availableArtifact.RemoveAt(i);
        }
    }

    public GameObject DropItem(float maxChance)
    {
        GameObject newItem = dropItemForChar[Random.Range(0, dropItemForChar.Length)];
        float dropChance = Random.Range(minDropChance, Random.Range(maxChance, maxDropChance));
        if (dropChance > 1.5f) dropChance = Random.Range(0.7f, dropChance);
        else if (dropChance > 1.2f) dropChance = Random.Range(0.6f, dropChance);
        else if (dropChance > 1f) dropChance = Random.Range(0.5f, dropChance);
        dropChance = (float)System.Math.Round((decimal)dropChance,2);

        if (dropChance == 2)
        {
            newItem.GetComponent<Outline>().OutlineColor = Color.red;
        }else if (dropChance > 1.5){
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

    public GameObject DropCertainItem(float rare)
    {
        GameObject newItem = dropItemForChar[Random.Range(0, dropItemForChar.Length)];
        float dropChance = rare;

        if (dropChance == 2)
        {
            newItem.GetComponent<Outline>().OutlineColor = Color.red;
        }
        else if (dropChance > 1.5)
        {
            newItem.GetComponent<Outline>().OutlineColor = Color.yellow;
        }
        else if (dropChance > 1.1)
        {
            newItem.GetComponent<Outline>().OutlineColor = Color.blue;
        }
        else if (dropChance > 0.7)
        {
            newItem.GetComponent<Outline>().OutlineColor = Color.green;
        }
        else if (dropChance >= 0.5)
        {
            newItem.GetComponent<Outline>().OutlineColor = Color.white;
        }
        newItem.GetComponent<CharackterItem>().Rang = dropChance;
        return newItem;
    }

    public GameObject DropArtifact()
    {
        int artifactChance = Random.Range(0,101);
        GameObject newArtifact;
        if (artifactChance < artifactDropChance)
        {
            if (availableArtifact.Count > 0) newArtifact = availableArtifact[Random.Range(0, availableArtifact.Count)];
            else newArtifact = DropItem(2f);
        } else newArtifact = DropItem(1.7f);

        return newArtifact;
    }

    public GameObject DropOnlyArtifact()
    {
        GameObject newArtifact;
        if (availableArtifact.Count > 0) newArtifact = availableArtifact[Random.Range(0, availableArtifact.Count)];
        else newArtifact = DropItem(2f);
        return newArtifact;
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

    public GameObject DropItemBox(int num)
    {
        GameObject newDopItem = dropItemAddHealth[Random.Range(0, dropItemAddHealth.Length)];

        if(num == 1) newDopItem = dropItemKey[Random.Range(0, dropItemKey.Length)];
        else if (num == 2) newDopItem = dropItemHeal[Random.Range(0, dropItemHeal.Length)];

        return newDopItem;
    }

}
