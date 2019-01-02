using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outline_Objects : MonoBehaviour {

    public float RadiusSphere = 1f;
    

    /*private void OnTriggerEnter(Collider other)
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
    }*/

    private void Update()
    {
    }

    Transform PredItem = null;

    private void OnTriggerEnter(Collider hitCol)
    {
        if (hitCol.tag == "Item" && hitCol.GetComponent<GUIcontroller>().ObjectEquipt == false && hitCol.GetComponent<GUIcontroller>().ObjectIsSee == false)
        {
            PredItem = hitCol.GetComponent<Transform>();
            Debug.Log(hitCol.name + " найден");
            hitCol.GetComponent<GUIcontroller>().ObjectSee(PredItem);
        }
    }
    private void OnTriggerExit(Collider hitCol)
    {
        if (hitCol.tag == "Item" && hitCol.GetComponent<GUIcontroller>().ObjectEquipt == false && hitCol.GetComponent<GUIcontroller>().ObjectIsSee == true)
        {
            Debug.Log(hitCol.name + " потерян");
            hitCol.GetComponent<GUIcontroller>().ObjectIsSee = false;
        }
    }

    //Рисует сферу вокруг точки
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1,1,0.015f,0.3f);
        Gizmos.DrawSphere(transform.position, RadiusSphere);
    }
}
