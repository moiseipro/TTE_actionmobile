using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controller : MonoBehaviour {

    public GameObject Player; //Ссылка на объект персонажа
    public Vector3 offset;// Смещение камеры относительно персонажа
    public float camSpeed = 2f; // Скорость следования камеры за персонажем
	

    void LateUpdate(){
        transform.position = Vector3.Lerp(transform.position, Player.transform.position + offset, camSpeed * Time.deltaTime); //Плавное следование камеры за персонажем
        
    }
}
