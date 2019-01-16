﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour {

    public GameObject[] bossPrefabs;
    public GameObject[] mapPrefabs;
    public GameObject[] objectPrefab;
    public GameObject[] playerPrefabs;

    public Camera mainCamera;
    GameObject player;

    // Use this for initialization
    void Start () {
        foreach(GameObject item in GameObject.FindGameObjectsWithTag("Item"))
        {
            if (item.GetComponent<GUIcontroller>().ObjectEquipt == false) Destroy(item);
        }

        GenerationMap();
        GameObject boss = Instantiate(bossPrefabs[0], new Vector3(0, 0, 0), Quaternion.AngleAxis(180,Vector3.up));
        if (!GameObject.FindWithTag("Player"))
        {
            player = Instantiate(playerPrefabs[0], new Vector3(0, 0, -9f), Quaternion.identity);
        }
        else
        {
            player = GameObject.FindWithTag("Player");
        }
        player.GetComponent<HeartSystem>().heartImages = GameObject.Find("HealthBar").transform.GetComponentsInChildren<Image>();
        player.GetComponent<HeartSystem>().CheckHealthAmount();
        player.GetComponent<Move_Controller>().joystickMove = GameObject.Find("MovePlayer").GetComponent<Joystick>();
        player.GetComponent<Move_Controller>().joystickFire = GameObject.Find("FirePlayer").GetComponent<Joystick>();
        mainCamera.GetComponent<Camera_Controller>().Player = player;
        player.transform.position = new Vector3(0,0,-27);
    }

    void GenerationMap()
    {
        int x = 0, z = 0;
        for(int i = 0; i < 5; i++)
        {
            if (i == 0) {
                GameObject map = Instantiate(mapPrefabs[Random.Range(0, 3)], new Vector3(0, -5f, 0), Quaternion.identity);
            } else if (i == 1) {
                GameObject map = Instantiate(mapPrefabs[Random.Range(0, 3)], new Vector3(27, -5f, 0), Quaternion.identity);
            } else if (i == 2) {
                GameObject map = Instantiate(mapPrefabs[Random.Range(0, 3)], new Vector3(-27, -5f, 0), Quaternion.identity);
            } else if (i == 3) {
                GameObject map = Instantiate(mapPrefabs[Random.Range(0, 3)], new Vector3(0, -5f, 27), Quaternion.identity);
            } else if (i == 4) {
                GameObject map = Instantiate(mapPrefabs[Random.Range(0, 3)], new Vector3(0, -5f, -27), Quaternion.identity);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ReloadLevel()
    {
        SceneManager.LoadScene("Game");
    }

    public void NextLevel()
    {
        SceneManager.LoadScene("Game");
        //PlayerPrefs.SetFloat("");
    }
}
