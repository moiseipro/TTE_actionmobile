using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour {

    private GameObject player;
    private Text key, nugget;
    private int moneyValue, keyValue,
        playerLevel;
    private bool isPause = false;
    private RectTransform artifactPanel, pause, statsPanel;
    private float speedUI = 150f;

    [HideInInspector]
    public int levelGame = 0;

    // Use this for initialization
    void Start () {
        levelGame = PlayerPrefs.GetInt("Level");
        
    }

    public void GUIspawn()
    {
        player = GameObject.FindWithTag("Player");
        key = GameObject.Find("KeyPlate").GetComponentInChildren<Text>();
        nugget = GameObject.Find("NuggetPlate").GetComponentInChildren<Text>();
        nugget.text = moneyValue.ToString();
        key.text = keyValue.ToString();
        artifactPanel = GameObject.Find("ArtifactPanel").GetComponent<RectTransform>();
        pause = GameObject.Find("Pause").GetComponent<RectTransform>();
        statsPanel = GameObject.Find("StatsPanel").GetComponent<RectTransform>();
    }
	
    public void AddMoney(int val)
    {
        moneyValue += val;
        nugget.text = moneyValue.ToString();
    }

    public void AddKey()
    {
        keyValue++;
        key.text = keyValue.ToString();
    }


    private void FixedUpdate()
    {
        if (isPause == true)
        {
            artifactPanel.anchoredPosition = Vector2.MoveTowards(artifactPanel.anchoredPosition, new Vector2(-225, 0), speedUI);
            pause.anchoredPosition = Vector2.MoveTowards(pause.anchoredPosition, new Vector2(645, -645), speedUI);
            statsPanel.anchoredPosition = Vector2.MoveTowards(statsPanel.anchoredPosition, new Vector2(225, -115), speedUI);
        } else if(isPause == false)
        {
            artifactPanel.anchoredPosition = Vector2.MoveTowards(artifactPanel.anchoredPosition, new Vector2(225, 0), speedUI);
            pause.anchoredPosition = Vector2.MoveTowards(pause.anchoredPosition, new Vector2(720, -75), speedUI);
            statsPanel.anchoredPosition = Vector2.MoveTowards(statsPanel.anchoredPosition, new Vector2(-225, -115), speedUI);
        }
    }

    public void Pause()
    {
        if (isPause == false)
        {
            isPause = true;
            StartCoroutine(StartPause());
        } else if(isPause == true)
        {
            isPause = false;
            Time.timeScale = 1;
        }
    }

    IEnumerator StartPause()
    {
        yield return new WaitForSeconds(0.3f);
        Time.timeScale = 0;
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
