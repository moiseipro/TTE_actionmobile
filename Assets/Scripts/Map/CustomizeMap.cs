using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class CustomizeMap : MonoBehaviour {

    [Header("Спец. объекты (префабы головоломок и прочего)")]
    public Color specialColor = Color.green;
    public GameObject[] specialObject;
    public Vector3[] spawnSpecialPoints;
    [Tooltip("Шанс(в процентах) появления специального объекта вместо остальных объектов")]
    [Range(0, 100)]
    public int spawnPercent;
    [Space(20)]
    [Header("Для статичных объектов")]
    public Color staticColor = Color.red;
    public Vector3[] spawnStaticPoints;
    [Header("Для разрушаемых и интерактивных объектов")]
    public Color destrColor = Color.yellow;
    public Vector3[] spawnDestrPoints;
    [Header("Для ловушек")]
    public Color trapColor = Color.blue;
    public Vector3[] spawnTrapPoints;
    [Header("Для сундуков и подобного")]
    public Color chestColor = Color.white;
    public Vector3[] spawnChestPoints;
    [Tooltip("Оцентка для появления лучшего сундука чем обычно")]
    [Range(0, 10)]
    public int spawnChestPercent;

    SceneController gameManager;
    //private float[] masAngle = { -90, -45, 0, 45, 90, 135, 180 };

    // Use this for initialization
    void Start () {
        gameManager = GameObject.FindWithTag("Manager").GetComponent<SceneController>();
        foreach (Vector3 vec in spawnStaticPoints)
        {
            GameObject obj = Instantiate(gameManager.objectPrefab[Random.Range(0, gameManager.objectPrefab.Length)], transform.TransformPoint(vec/50) + new Vector3(0, 5f, 0f), Quaternion.identity);
            obj.transform.rotation = Quaternion.AngleAxis(Random.Range(0,360), Vector3.up);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = staticColor;
        if (spawnStaticPoints != null)
        {
            foreach (Vector3 vec in spawnStaticPoints)
            {
                Gizmos.DrawSphere(transform.TransformPoint(vec / 50) + new Vector3(0, 5f, 0f), 1);
            }
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
