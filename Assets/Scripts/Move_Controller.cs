using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Controller : MonoBehaviour {

    //Характеристики движения
    public float speed;
    public float jump;
    private float gravity;

    private Vector3 moveVector; // Вектор направления движения

    //Ссылки на компоненты
    private CharacterController ch_controller;

    //Ссылки на объекты
    public Joystick joystick;


    void Start () {
        ch_controller = GetComponent<CharacterController>();
	}
	
	void Update () {
        GamingGravity();
        MovePlayer();
	}

    //Метод перемещения персонажа
    private void MovePlayer() {
        //Перемещение персонажа по осям
        moveVector = Vector3.zero;
        moveVector.x = Input.GetAxis("Horizontal") * speed;
        moveVector.z = Input.GetAxis("Vertical") * speed;
        if (moveVector.x == 0 && moveVector.z == 0)
        {
            moveVector.x = joystick.Horizontal * speed;
            moveVector.z = joystick.Vertical * speed;
        }

        //Повороты персонажа в сторону движения
        if (Vector3.Angle(Vector3.forward, moveVector) > 1f || Vector3.Angle(Vector3.forward, moveVector) == 0) {
            Vector3 direct = Vector3.RotateTowards(transform.forward, moveVector, speed, 0.0f);
            transform.rotation = Quaternion.LookRotation(direct);
        }

        moveVector.y = gravity;
        ch_controller.Move(moveVector * Time.deltaTime); //Движения по направлению
    }

    //Метод гравитации
    private void GamingGravity() {
        if (!ch_controller.isGrounded) gravity -= 15f * Time.deltaTime;
        else gravity = -1f;
        if (Input.GetKeyDown(KeyCode.Space) && ch_controller.isGrounded) gravity = jump; //Используется для тестирования.
    }
}
