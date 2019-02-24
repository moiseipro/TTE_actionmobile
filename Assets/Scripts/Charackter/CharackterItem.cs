using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharackterItem : MonoBehaviour {

    [HideInInspector]
    public GameObject Player;

    [Header("Тип предмета")]
    public string upgradeType;
    [Header("Название предмета")]
    public string upgradeName;
    [Header("Редкость предмета")]
    [Range(0.5f, 2f)]
    public float Rang;

    private void Start()
    {
        Player = GameObject.FindWithTag("Player");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Object" || collision.gameObject.tag == "Map" || collision.gameObject.tag == "Arena")
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
            if (Rang > 1.5)
            {
                GetComponent<Outline>().OutlineColor = Color.yellow;
            }
            else if (Rang > 1.1)
            {
                GetComponent<Outline>().OutlineColor = Color.blue;
            }
            else if (Rang > 0.7)
            {
                GetComponent<Outline>().OutlineColor = Color.green;
            }
            else if (Rang >= 0.5)
            {
                GetComponent<Outline>().OutlineColor = Color.white;
            }
        }
    }
}
