using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outline_Objects : MonoBehaviour {

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
}
