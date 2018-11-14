using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Controller : MonoBehaviour {

    public GameObject Bullets;
    public GameObject Weapon;
    public float TimeReload = 0.5f; //Скорость стрельбы

    //Тип разброса
    private int TurnType = 0;

    private bool Reload = false; //Перезарядка

    void Start () {
		
	}
	

	void Update () {
        //При нажатии кнопки создает объект пули
        if ((Input.GetKey(KeyCode.F) || GetComponent<Move_Controller>().joystickFire.Horizontal !=0 || GetComponent<Move_Controller>().joystickFire.Vertical != 0) && !Reload){
            GameObject Bull = GameObject.Instantiate(Bullets, Weapon.transform.position - Weapon.transform.right * 1.2f, transform.rotation);
            Bull.transform.Rotate(-90f, 180f, 0);
            Destroy(Bull, 5f);
            Reload = true;
            StartCoroutine(ReloadWeapon());
        }
    }

    IEnumerator ReloadWeapon()
    {
        yield return new WaitForSeconds(TimeReload);
        Reload = false;
    }
}
