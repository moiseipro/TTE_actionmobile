using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class CustomizeMap : MonoBehaviour {

	public Vector3[] spawnPoints;

    SceneController gameManager;
    private float[] masAngle = { -90, 0, 90, 180 };

    // Use this for initialization
    void Start () {
        gameManager = GameObject.FindWithTag("Manager").GetComponent<SceneController>();
        foreach (Vector3 vec in spawnPoints)
        {
            GameObject obj = Instantiate(gameManager.objectPrefab[Random.Range(0, gameManager.objectPrefab.Length)], transform.TransformPoint(vec/50) + new Vector3(0, 5f, 0f), Quaternion.identity);
            obj.transform.rotation = Quaternion.AngleAxis(masAngle[Random.Range(0,4)], Vector3.up);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (Vector3 vec in spawnPoints) {
            Gizmos.DrawSphere(transform.TransformPoint(vec/50) + new Vector3(0, 5f, 0f), 1);
        }

    }

    // Update is called once per frame
    void Update () {
		
	}
}
