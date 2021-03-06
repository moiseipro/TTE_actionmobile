﻿using System.Collections;
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
    [Header("Для освещения")]
    public Color lightColor = Color.white;
    public Vector3[] spawnLightPoints;
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
    public Color chestColor = Color.magenta;
    public Vector3[] spawnChestPoints;
    [Tooltip("Оцентка для появления лучшего сундука чем обычно")]
    [Range(0, 8)]
    public int spawnChestPercent;

    SceneController gameManager;
    //private float[] masAngle = { -90, -45, 0, 45, 90, 135, 180 };

    // Use this for initialization
    void Start () {
        gameManager = GameObject.FindWithTag("Manager").GetComponent<SceneController>();
        int generateSpawnPersent = Random.Range(0, 100);
        if (generateSpawnPersent >= spawnPercent)
        {
            if (spawnStaticPoints != null)
            {
                int spawnStaticObj = Random.Range(0, spawnStaticPoints.Length);
                for (int i = 0; i < spawnStaticObj; i++)
                {
                    GameObject obj = Instantiate(gameManager.objectPrefab[Random.Range(0, gameManager.objectPrefab.Length)], transform.TransformPoint(spawnStaticPoints[i] / 50) + new Vector3(0, 5f, 0f), Quaternion.identity);
                    obj.transform.rotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up);
                }
            }

            if (spawnDestrPoints != null)
            {
                int spawnDestrObj = Random.Range(0, spawnDestrPoints.Length);
                for (int i = 0; i < spawnDestrObj; i++)
                {
                    GameObject obj = Instantiate(gameManager.objectInterPrefab[Random.Range(0, gameManager.objectInterPrefab.Length)], transform.TransformPoint(spawnDestrPoints[i] / 50) + new Vector3(0, 5f, 0f), Quaternion.identity);
                    obj.transform.rotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up);
                }
            }

            if (spawnTrapPoints != null)
            {
                foreach (Vector3 vec in spawnTrapPoints)
                {
                    GameObject obj = Instantiate(gameManager.objectTrapPrefab[Random.Range(0, gameManager.objectTrapPrefab.Length)], transform.TransformPoint(vec / 50) + new Vector3(0, 5f, 0f), Quaternion.identity);
                    //obj.transform.rotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up);
                }
            }

            if (spawnChestPoints != null)
            {
                foreach (Vector3 vec in spawnChestPoints)
                {
                    GameObject obj = Instantiate(gameManager.objectChestPrefab[Random.Range(spawnChestPercent, gameManager.objectChestPrefab.Length)], transform.TransformPoint(vec / 50) + new Vector3(0, 5f, 0f), Quaternion.identity);
                    obj.transform.rotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up);
                }
            }

            if (spawnLightPoints != null)
            {
                foreach (Vector3 vec in spawnLightPoints)
                {
                    GameObject obj = Instantiate(gameManager.objectLightPrefab[Random.Range(0, gameManager.objectLightPrefab.Length)], transform.TransformPoint(vec / 50) + new Vector3(0, 5f, 0f), Quaternion.identity);
                    obj.transform.rotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up);
                }
            }
        } else
        {
            if (specialObject != null && spawnSpecialPoints != null)
            {
                int specialObjectId = Random.Range(0, specialObject.Length);
                GameObject obj = Instantiate(specialObject[specialObjectId], transform.TransformPoint(spawnSpecialPoints[specialObjectId] / 50) + new Vector3(0, 5f, 0f), Quaternion.identity, gameObject.transform);
                obj.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = specialColor;
        if (spawnSpecialPoints != null)
        {
            foreach (Vector3 vec in spawnSpecialPoints)
            {
                Gizmos.DrawSphere(transform.TransformPoint(vec / 50) + new Vector3(0, 5f, 0f), 1);
            }
        }

        Gizmos.color = staticColor;
        if (spawnStaticPoints != null)
        {
            foreach (Vector3 vec in spawnStaticPoints)
            {
                Gizmos.DrawSphere(transform.TransformPoint(vec / 50) + new Vector3(0, 5f, 0f), 1);
            }
        }

        Gizmos.color = destrColor;
        if (spawnDestrPoints != null)
        {
            foreach (Vector3 vec in spawnDestrPoints)
            {
                Gizmos.DrawSphere(transform.TransformPoint(vec / 50) + new Vector3(0, 5f, 0f), 1);
            }
        }

        Gizmos.color = trapColor;
        if (spawnTrapPoints != null)
        {
            foreach (Vector3 vec in spawnTrapPoints)
            {
                Gizmos.DrawSphere(transform.TransformPoint(vec / 50) + new Vector3(0, 5f, 0f), 1);
            }
        }

        Gizmos.color = chestColor;
        if (spawnChestPoints != null)
        {
            foreach (Vector3 vec in spawnChestPoints)
            {
                Gizmos.DrawSphere(transform.TransformPoint(vec / 50) + new Vector3(0, 5f, 0f), 1);
            }
        }

        Gizmos.color = lightColor;
        if (spawnLightPoints != null)
        {
            foreach (Vector3 vec in spawnLightPoints)
            {
                Gizmos.DrawSphere(transform.TransformPoint(vec / 50) + new Vector3(0, 5f, 0f), 1);
            }
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
