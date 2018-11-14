using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Controller : MonoBehaviour {

    //Характеристики движения
    public float speed;
    public float jump;
    private float gravity;

    private Vector3 moveVector; // Вектор направления движения
    private Vector3 rotVector;

    //Ссылки на компоненты
    private CharacterController ch_controller;
    private Animator ch_animator;

    //Ссылки на объекты
    public Joystick joystickMove;
    public Joystick joystickFire;


    void Start () {
        ch_controller = GetComponent<CharacterController>();
        ch_animator = GetComponent<Animator>();
	}
	
	void Update () {
        GamingGravity();
        MovePlayer();
	}

    //Метод перемещения персонажа
    private void MovePlayer() {
        //Перемещение персонажа по осям
        moveVector = Vector3.zero;
        rotVector = Vector3.zero;
        moveVector.x = Input.GetAxis("Horizontal") * speed;
        moveVector.z = Input.GetAxis("Vertical") * speed;
        rotVector.x = joystickFire.Horizontal;
        rotVector.z = joystickFire.Vertical;
        if (moveVector.x == 0 && moveVector.z == 0)
        {
            moveVector.x = joystickMove.Horizontal * speed;
            moveVector.z = joystickMove.Vertical * speed;
        }

        //Повороты персонажа в сторону движения
        if (Vector3.Angle(Vector3.forward, rotVector) > 1f || Vector3.Angle(Vector3.forward, rotVector) == 0) {
            Vector3 direct = Vector3.RotateTowards(transform.forward, rotVector, speed, 0.0f);
            transform.rotation = Quaternion.LookRotation(direct);
        }

        moveVector.y = gravity;

        ch_animator.SetFloat("Direction", moveVector.magnitude); // Управление анимацией бега через длину вектора
        //Debug.Log(ch_animator.GetFloat("Direction"));
        ch_controller.Move(moveVector * Time.deltaTime); //Движения по направлению
    }

    //Метод гравитации
    private void GamingGravity() {
        if (!ch_controller.isGrounded) gravity -= 15f * Time.deltaTime;
        else gravity = -1f;
        if (Input.GetKeyDown(KeyCode.Space) && ch_controller.isGrounded) gravity = jump; //Используется для тестирования.
    }
}
