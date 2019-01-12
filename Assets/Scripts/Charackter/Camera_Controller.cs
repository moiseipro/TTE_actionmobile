using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controller : MonoBehaviour {

    public GameObject Player; //Ссылка на объект персонажа
    public Vector3 offset;// Смещение камеры относительно персонажа
    private Vector3 NewPos;
    public float camSpeed = 2f; // Скорость следования камеры за персонажем
	

    void LateUpdate(){
        /*NewPos.y = offset.y;
        if (Player.transform.position.x < 12f && Player.transform.position.x > -12.5f)
        {
            NewPos.x = Player.transform.position.x + offset.x;
        }
        if (Player.transform.position.z < 12f && Player.transform.position.z > -10.5f) {
            NewPos.z = Player.transform.position.z + offset.z;
        }*/
        NewPos = new Vector3(Player.transform.position.x + offset.x, offset.y, Player.transform.position.z + offset.z);
        transform.position = Vector3.Lerp(transform.position, NewPos , camSpeed * Time.deltaTime); //Плавное следование камеры за персонажем
    }
}
