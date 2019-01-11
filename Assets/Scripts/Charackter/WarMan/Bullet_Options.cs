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
        if(type == 3) gameObject.transform.Translate((Vector3.forward+new Vector3(Random.Range(-1,2),0, Random.Range(0, 2))) * speed * Time.deltaTime);
        else gameObject.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        gameObject.transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("entered " + other);
        if (other.tag == "Object" || other.tag == "Map") Destroy(this.gameObject,0.1f);
        else if (other.tag == "Enemy" && type!=-1){
            if (hpBullet < 1) Destroy(this.gameObject);
            else {
                hpBullet -= 1;
                if (type == 4) {
                    gameObject.transform.position = other.transform.position + Vector3.up;
                    gameObject.transform.Rotate(Vector3.up * Random.Range(0, 360f));
                    gameObject.transform.Rotate(Vector3.right * -10);
                }
            }
            other.SendMessage("AddDamage", damage);
        } else if(other.tag == "Player" && type == -1)
        {
            other.GetComponent<HeartSystem>().TakeDamage(-(int)damage);
            Destroy(this.gameObject);
        }
    }
}
