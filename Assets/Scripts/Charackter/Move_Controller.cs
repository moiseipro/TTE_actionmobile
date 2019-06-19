using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Controller : MonoBehaviour {

    //Характеристики движения
    public float speed;
    public float minSpeed ,maxSpeed;
    [HideInInspector] public float speedDebaf, staticSpeedDebaf;
    public float jump;
    private float gravity;

    private Vector3 moveVector; // Вектор направления движения
    [HideInInspector] public Vector3 rotVector;

    //Ссылки на компоненты
    private CharacterController ch_controller;
    private HeartSystem hs;
    private Animator ch_animator;
    private PlayerManager playerManager;

    //Ссылки на объекты
    public Joystick joystickMove;
    public Joystick joystickFire;
    public GameObject manager;


    void Start () {
        ch_controller = GetComponent<CharacterController>();
        ch_animator = GetComponent<Animator>();
        hs = GetComponent<HeartSystem>();
        playerManager = manager.GetComponent<PlayerManager>();
    }
	
	void Update () {
        if(hs.isDead == false)
        {
            GamingGravity();
            MovePlayer();
            if (gameObject.transform.position.y < -8)
            {
                hs.TakeDamage(-100);
            }
        }
	}

    public void StaticSpeedDebaf(float persent)
    {
        staticSpeedDebaf = speed * persent / 100;
    }

    //Метод перемещения персонажа
    private void MovePlayer() {
        //Перемещение персонажа по осям
        moveVector = Vector3.zero;
        rotVector = Vector3.zero;
        float curSpeed = speed - (speedDebaf + staticSpeedDebaf);
        curSpeed = Mathf.Clamp(curSpeed, minSpeed, maxSpeed);
        moveVector.x = Input.GetAxis("Horizontal") * curSpeed;
        moveVector.z = Input.GetAxis("Vertical") * curSpeed;
        rotVector.x = joystickFire.Horizontal;
        rotVector.z = joystickFire.Vertical;
        if (moveVector.x == 0 && moveVector.z == 0)
        {
            moveVector.x = joystickMove.Horizontal * (speed - (speedDebaf + staticSpeedDebaf));
            moveVector.z = joystickMove.Vertical* (speed - (speedDebaf + staticSpeedDebaf));
        }
        //Повороты персонажа в сторону стрельбы
        if(manager !=null) rotVector = manager.transform.TransformVector(rotVector);

        if (Vector3.Angle(Vector3.forward, rotVector) > 1f || Vector3.Angle(Vector3.forward, rotVector) == 0) {
            Vector3 direct = Vector3.RotateTowards(transform.forward, rotVector, speed, 0.0f);
            transform.rotation = Quaternion.LookRotation(direct);
        }

        moveVector.y = gravity;

        ch_animator.SetFloat("Direction", moveVector.magnitude); // Управление анимацией бега через длину вектора
        if (!hs.isDead) ch_controller.Move((manager.transform.TransformVector(moveVector)) * Time.deltaTime); //Движения по направлению
    }

    //Метод гравитации
    private void GamingGravity() {
        if (hs.isDead) gravity = 0;
        else
        {
            if (!ch_controller.isGrounded) gravity -= 15f * Time.deltaTime;
            else gravity = -1f;
            if (Input.GetKeyDown(KeyCode.Space) && ch_controller.isGrounded) gravity = jump; //Используется для тестирования
        }
    }

    public Vector3 GetVectorMove()
    {
        return moveVector;
    }
}
