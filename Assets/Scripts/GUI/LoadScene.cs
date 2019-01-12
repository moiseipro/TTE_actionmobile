using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {

    public int sceneID;

    public Text progressText;

	// Use this for initialization
	void Start () {
        StartCoroutine(AsyncLoad());
    }
	
    IEnumerator AsyncLoad()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneID);
        while (!operation.isDone)
        {
            float progress = operation.progress / 0.9f;
            progressText.text = string.Format("{0:0}%", progress*100f);
            yield return null;
        }
    }

}
