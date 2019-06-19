using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class PlayerManager : MonoBehaviour {

    private GameObject player,gameGUI;
    private HeartSystem hs;
    private SaveSystem ss;
    private Text key, nugget, nuggetFinish, keyFinish, levelFinish, xpCount, curLevel;
    private Image levelProgressBar, pauseImage;
    [HideInInspector]public int moneyValue, keyValue,
        playerLevel;
    float pV1,pV2,pV3;
    private bool isPause = false;
    private RectTransform artifactPanel, pause, statsPanel, gameOver;
    private float speedUI = 150f;

    [HideInInspector]
    public int levelGame = 0;
    private bool deathLevelUp;
    private float predXp, curSavedXp, maxSavedXp;

    // Use this for initialization
    void Start () {
        GUIspawn();
        moneyValue = PlayerPrefs.GetInt("MyNuggets");
        keyValue = PlayerPrefs.GetInt("playKeys"); pV2 = keyValue;
        levelGame = PlayerPrefs.GetInt("Level");
        nugget.text = moneyValue.ToString();
        key.text = (keyValue + PlayerPrefs.GetInt("MyKeys")).ToString();
    }

    public void GetAchivement(string id)
    {
        Social.ReportProgress(id, 100.0f, (bool success) => {
            if (success) Debug.Log("Получено достижение: " + id);
        });
    }

    public void GUIspawn()
    {
        player = GameObject.FindWithTag("Player");
        gameGUI = GameObject.FindWithTag("GUI");
        hs = player.GetComponent<HeartSystem>();
        key = GameObject.Find("KeyPlate").GetComponentInChildren<Text>();
        nugget = GameObject.Find("NuggetPlate").GetComponentInChildren<Text>();
        nuggetFinish = GameObject.Find("NuggetFinish").GetComponentInChildren<Text>();
        keyFinish = GameObject.Find("KeyFinish").GetComponentInChildren<Text>();
        levelFinish = GameObject.Find("WorldFinish").GetComponentInChildren<Text>();
        xpCount = GameObject.Find("XpCounter").GetComponentInChildren<Text>();
        curLevel = GameObject.Find("curLevel").GetComponentInChildren<Text>();
        nugget.text = moneyValue.ToString();
        key.text = keyValue.ToString();
        artifactPanel = GameObject.Find("ArtifactPanel").GetComponent<RectTransform>();
        pause = GameObject.Find("Pause").GetComponent<RectTransform>();
        pauseImage = GameObject.Find("Pause").GetComponent<Image>();
        statsPanel = GameObject.Find("StatsPanel").GetComponent<RectTransform>();
        gameOver = GameObject.Find("GameOver").GetComponent<RectTransform>();
        levelProgressBar = GameObject.Find("LevelProgressBar").GetComponent<Image>();
        
        ss = GetComponent<SaveSystem>();
    }
	
    public void AddMoney(int val)
    {
        moneyValue += val;
        nugget.text = moneyValue.ToString();
    }

    public void AddKey()
    {
        keyValue++; 
        key.text = (keyValue + PlayerPrefs.GetInt("MyKeys")).ToString();
    }


    private void FixedUpdate()
    {
        if (isPause == true)
        {
            artifactPanel.anchoredPosition = Vector2.MoveTowards(artifactPanel.anchoredPosition, new Vector2(-225, 0), speedUI);
            pause.anchoredPosition = Vector2.MoveTowards(pause.anchoredPosition, new Vector2(-645, -645), speedUI);
            pause.sizeDelta = Vector2.Lerp(pause.sizeDelta, new Vector2(320, 100), 2);
            statsPanel.anchoredPosition = Vector2.MoveTowards(statsPanel.anchoredPosition, new Vector2(225, -115), speedUI);
        } else if(isPause == false)
        {
            artifactPanel.anchoredPosition = Vector2.MoveTowards(artifactPanel.anchoredPosition, new Vector2(225, 0), speedUI);
            pause.anchoredPosition = Vector2.MoveTowards(pause.anchoredPosition, new Vector2(-60, -225), speedUI);
            pause.sizeDelta = Vector2.Lerp(pause.sizeDelta, new Vector2(100, 100), 2);
            statsPanel.anchoredPosition = Vector2.MoveTowards(statsPanel.anchoredPosition, new Vector2(-225, -115), speedUI);
        }
        if(!deathLevelUp && hs.isDead)
        {
            deathLevelUp = true;
            ss.LoadFile();
            curLevel.text = ss.sa.level.ToString();
            predXp = ss.sa.curXp;
            ss.sa.curXp += moneyValue + levelGame * 5 + keyValue * 10;
            curSavedXp = ss.sa.curXp;
            maxSavedXp = ss.sa.maxXp;
            if (ss.sa.curXp >= ss.sa.maxXp)
            {
                int xpBox = ss.sa.curXp - ss.sa.maxXp;
                ss.sa.maxXp = (int)(ss.sa.maxXp * 1.2f);
                ss.sa.curXp = xpBox;
                ss.sa.level++;
            }
            ss.SaveFile();
            PlayerPrefs.SetInt("Level", 0);
            PlayerPrefs.SetInt("MyNuggets", 0);
            PlayerPrefs.SetInt("MyKeys", PlayerPrefs.GetInt("MyKeys") + keyValue);
        }
        if (hs.isDead)
        {
            pause.gameObject.SetActive(false);
            gameOver.anchoredPosition = Vector2.MoveTowards(gameOver.anchoredPosition, new Vector2(0, 10), speedUI);
            
            pV1 = Mathf.Lerp(pV1, moneyValue, 1.5f * Time.deltaTime);
            nuggetFinish.text = (Math.Ceiling(pV1)).ToString();
            pV2 = Mathf.Lerp(pV2, keyValue, 1.5f * Time.deltaTime);
            keyFinish.text = (Math.Ceiling(pV2)).ToString();
            pV3 = Mathf.Lerp(pV3, levelGame, 1.5f * Time.deltaTime);
            levelFinish.text = (Math.Ceiling(pV3)).ToString();
            predXp = Mathf.Lerp(predXp, curSavedXp, 1.5f * Time.deltaTime);
            levelProgressBar.fillAmount = predXp / maxSavedXp;
            if(levelProgressBar.fillAmount >= 0.99)
            {
                UpdateLevel();
            }
            xpCount.text = Math.Ceiling(predXp).ToString() + " / " + ss.sa.maxXp;
        } else
        {
            pause.gameObject.SetActive(true);
        }
    }

    private void UpdateLevel()
    {
        predXp = 0;
        curSavedXp = ss.sa.curXp;
        maxSavedXp = ss.sa.maxXp;
        curLevel.text = ss.sa.level.ToString();
    }

    public void Pause()
    {
        if (isPause == false)
        {
            isPause = true;
            pauseImage.sprite = Resources.Load<Sprite>("Sprites/UI/GameMenu/PausePlay");
            StartCoroutine(StartPause());
        } else if(isPause == true)
        {
            isPause = false;
            pauseImage.sprite = Resources.Load<Sprite>("Sprites/UI/GameMenu/PauseStop");
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
        PlayerPrefs.SetInt("MyNuggets", moneyValue);
        PlayerPrefs.SetInt("playKeys", keyValue);
        SceneManager.LoadScene("Game");
    }

    public void MainMenu()
    {
        Destroy(player);
        Destroy(gameGUI);
        SceneManager.LoadScene("Menu");
    }

    public void DeathPlayer()
    {
        Pause();
        GameObject.FindWithTag("Player").GetComponent<HeartSystem>().TakeDamage(-100);
    }
}
