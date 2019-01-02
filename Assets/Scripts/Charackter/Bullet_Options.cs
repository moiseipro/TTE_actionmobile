using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Options : MonoBehaviour {

    [Header("Тип пули")]
    public int type;
    [Space(20)]
    public float damage = 5f;
    public float radiusForDamage = 0f;
    public float speed = 6f;
    public float rotationSpeed = 6f;
    public int hpBullet = 0; //Количество жизней пули

	
	void Start () {
		
	}
	
	
	void Update () {
        Move();
    }

    //Полет пули
    void Move() {
        gameObject.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        gameObject.transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("entered " + other);
        if (other.tag == "Object") Destroy(this.gameObject,0.1f);
        else if (other.tag == "Enemy" ){
            if (hpBullet < 1) Destroy(this.gameObject);
            else hpBullet -= 1;
            other.SendMessage("AddDamage", damage);
        }
    }
}
