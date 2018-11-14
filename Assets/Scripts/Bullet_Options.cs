using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Options : MonoBehaviour {

    [Header("Тип пули")]
    public int Type;
    [Space(20)]
    public float Damage = 5f;
    public float RadiusDamage = 0f;
    public float Speed = 8f;
    public int HpBullet = 1; //Количество жизней пули

	
	void Start () {
		
	}
	
	
	void Update () {
        Move();
    }

    //Полет пули
    void Move() {
        gameObject.transform.Translate(gameObject.transform.forward * Speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("entered");
        Destroy(this.gameObject);
    }
}
