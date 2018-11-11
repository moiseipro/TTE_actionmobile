﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outline_Objects : MonoBehaviour {

    public float RadiusSphere = 1f;

    private void OnTriggerEnter(Collider other)
    {
        //Включает скрипт обводки, если объект касается тригера
        if (other.GetComponent<Outline>())
        {
            other.GetComponent<Outline>().enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Выключает скрипт обводки, если объект выходит из тригера
        if (other.GetComponent<Outline>())
        {
            other.GetComponent<Outline>().enabled = false;
        }
    }

    /* Находит объект попавший в сферу
    void DrawSphere() {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, RadiusSphere);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].tag == "Object") {
                VisionObject.Add(hitColliders[i].gameObject);
                Debug.Log(hitColliders[i].name + " найден");
            }
            i++;
        }

    }

      Рисует сферу вокруг точки
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1,1,0.015f,0.3f);
        Gizmos.DrawSphere(transform.position, RadiusSphere);
    }*/
}