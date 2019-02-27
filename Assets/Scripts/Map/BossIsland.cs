using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIsland : MonoBehaviour {

    SceneController gameManager;
    public GameObject gates;

    // Use this for initialization
    void Start () {
        gameManager = GameObject.FindWithTag("Manager").GetComponent<SceneController>();

        GameObject boss = Instantiate(gameManager.bossPrefabs[Random.Range(0, gameManager.bossPrefabs.Length)], gameObject.transform.position + new Vector3(0, 5f, 0f), Quaternion.identity);
        boss.transform.rotation = Quaternion.AngleAxis(Random.Range(180, 270), Vector3.up);
    }

    public void BossFightStart()
    {
        gates.GetComponent<Animator>().SetTrigger("StartFight");
    }

    public void BossFightStop()
    {
        gates.GetComponent<Animator>().SetTrigger("StopFight");
    }
}
