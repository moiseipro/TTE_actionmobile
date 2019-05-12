﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public Button achievementsBut;
    public Button playBut;

    public SaveArtifact sa = new SaveArtifact();

    private string path;

    
    void Start()
    {

        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate((bool success) => {
            if (success)
            {
                Debug.Log("Удачный вход!");
                achievementsBut.enabled = true;
                PlayGamesPlatform.Instance.LoadAchievements((IAchievement[] allAchiev) => {
                    foreach (IAchievement achiev in allAchiev)
                    {
                        if (achiev.percentCompleted == 100.0)
                        {
                            if (achiev.id == "CgkIxpeq_8sQEAIQAA")
                            {
                                sa.activeArtifact[0] = true;
                                sa.activeArtifact[1] = true;
                            }
                            else
                            {
                                sa.activeArtifact[0] = false;
                                sa.activeArtifact[1] = false;
                            }
                        }
                    }
                });
            }
            else
            {
                Debug.Log("Неудачный вход!");
                achievementsBut.enabled = false;
            }
        });

        
    }

    public void LoadFile()
    {

#if UNITY_ANDROID && !UNITY_EDITOR
        path = Path.Combine(Application.persistentDataPath, "Save"+ PlayerPrefs.GetInt("PalyerCharackter") +".json");
#else
        path = Path.Combine(Application.dataPath, "Save" + PlayerPrefs.GetInt("PalyerCharackter") + ".json");
#endif

        if (File.Exists(path))
        {
            sa = JsonUtility.FromJson<SaveArtifact>(File.ReadAllText(path));
        }
        else
        {
            for (int i = 0; i < sa.activeArtifact.Length; i++)
            {

                sa.activeArtifact[i] = false;
            }
            sa.maxXp = 1000;
            sa.level = 1;
            if (PlayerPrefs.GetInt("PalyerCharackter") == 0) sa.open = true;
            else sa.open = false;
            //if (PlayerPrefs.GetInt("PalyerCharackter") == 1) sa.open = true;
            SaveFile();
        }

        if (sa.open) playBut.enabled = true;
        else playBut.enabled = false;
    }

    public void SaveFile()
    {
        File.WriteAllText(path, JsonUtility.ToJson(sa));
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("LoadScene");
    }
    
}
