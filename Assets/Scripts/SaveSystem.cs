using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class SaveSystem : MonoBehaviour {

    public SaveArtifact sa = new SaveArtifact();

    private string path;
    private string path2;

    // Use this for initialization
    void Start () {
        LoadFile();
    }

    public void LoadFile()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        path = Path.Combine(Application.persistentDataPath, "Save"+ PlayerPrefs.GetInt("PalyerCharackter") +".json");
#else
        path = Path.Combine(Application.dataPath, "Save"+ PlayerPrefs.GetInt("PalyerCharackter") +".json");
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
            SaveFile();
        }
    }

    public void SaveFile()
    {
        File.WriteAllText(path, JsonUtility.ToJson(sa));
    }

    public bool SaveFileOpenCh(int idChar)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        path2 = Path.Combine(Application.persistentDataPath, "Save"+ idChar +".json");
#else
        path2 = Path.Combine(Application.dataPath, "Save" + idChar + ".json");
#endif
        if (File.Exists(path2))
        {
            sa = JsonUtility.FromJson<SaveArtifact>(File.ReadAllText(path2));
            if (sa.open == true) return true;
            else sa.open = true;
        }
        else
        {
            for (int i = 0; i < sa.activeArtifact.Length; i++)
            {

                sa.activeArtifact[i] = false;
            }
            sa.open = true;
            sa.maxXp = 1000;
            sa.level = 1;
        }

        File.WriteAllText(path2, JsonUtility.ToJson(sa));
        return false;
    }

}


[Serializable]
public class SaveArtifact
{
    public bool open;
    public bool[] activeArtifact = new bool[6];
    public int level;
    public int curXp;
    public int maxXp;
    public int keys;
}
