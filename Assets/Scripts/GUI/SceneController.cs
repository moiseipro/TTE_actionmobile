using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

    public GameObject[] bossPrefabs;
    public GameObject[] mapPrefabs;
    public GameObject[] objectPrefab;
    public GameObject[] playerPrefabs;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ReloadLevel()
    {
        SceneManager.LoadScene(0);
    }
}
