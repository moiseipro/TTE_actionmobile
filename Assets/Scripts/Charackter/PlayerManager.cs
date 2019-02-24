using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour {

    private GameObject player;
    private int moneyValue, keyValue,
        playerLevel;

    [HideInInspector]
    public int levelGame = 0;

    // Use this for initialization
    void Start () {
        levelGame = PlayerPrefs.GetInt("Level");
        player = GameObject.FindWithTag("Player");
    }
	
    public void AddMoney(int val)
    {
        moneyValue += val;
    }

    public void AddKey()
    {
        moneyValue++;
    }


    public void ReloadLevel()
    {
        Destroy(player);
        //player.GetComponent<HeartSystem>().HealAll();
        PlayerPrefs.SetInt("Level", 0);
        SceneManager.LoadScene("Game");
    }

    public void NextLevel()
    {
        levelGame++;
        PlayerPrefs.SetInt("Level", levelGame);
        SceneManager.LoadScene("Game");
    }
}
