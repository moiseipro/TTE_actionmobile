using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class SaveSystem : MonoBehaviour {

    public SaveArtifact sa = new SaveArtifact();

    private string path;

	// Use this for initialization
	void Start () {

        LoadFile();
    }

    public void LoadFile()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        path = Path.Combine(Application.persistentDataPath, "Save.json");
#else
        path = Path.Combine(Application.dataPath, "Save.json");
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
            SaveFile();
        }
    }

    public void SaveFile()
    {
        File.WriteAllText(path, JsonUtility.ToJson(sa));
    }

}


[Serializable]
public class SaveArtifact
{
    public bool[] activeArtifact = new bool[12];
}
