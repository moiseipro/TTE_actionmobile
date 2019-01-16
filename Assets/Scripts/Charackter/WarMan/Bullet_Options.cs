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
        if (type == 3)
        {
            gameObject.transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
            Vector3 rotVec = new Vector3(GameObject.FindWithTag("Weapon").transform.position.x + GameObject.FindWithTag("Weapon").transform.forward.x * speed, gameObject.transform.position.y * rotationSpeed * Time.deltaTime, GameObject.FindWithTag("Weapon").transform.position.z + GameObject.FindWithTag("Weapon").transform.forward.z * speed);
            transform.LookAt(rotVec);
            //rotVec.y = gameObject.transform.rotation.y * rotationSpeed * Time.deltaTime;
            //transform.rotation = Quaternion.LookRotation(GameObject.FindWithTag("Weapon").transform.forward.normalized);
            //gameObject.transform.Rotate(GameObject.FindWithTag("Weapon").transform.right * rotationSpeed * Time.deltaTime, Space.Self);
        }
        else {
            gameObject.transform.Translate(Vector3.forward * speed * Time.deltaTime);
            gameObject.transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("entered " + other);

        if (other.tag == "Object" || other.tag == "Map") {
            if (type == 2)
            {
                for (int i = 0; i < hpBullet; i++) {
                    GameObject bull = Instantiate(gameObject, gameObject.transform.position + Vector3.up / 10f, Quaternion.identity);
                    bull.transform.Rotate(Vector3.up * Random.Range(0, 360f));
                    bull.transform.Rotate(Vector3.right * -30);
                    bull.GetComponent<Bullet_Options>().hpBullet = 0;
                    bull.GetComponent<Bullet_Options>().damage *= 0.6f;
                    bull.GetComponent<Bullet_Options>().rotationSpeed = rotationSpeed * 4f;
                    bull.transform.localScale *= 0.9f; 
                }
            }
            Destroy(this.gameObject, 0.1f);
        }
        else if ((other.tag == "Enemy" || other.tag == "Boss") && type != -1)
        {
            if (hpBullet < 1) Destroy(this.gameObject);
            else
            {
                hpBullet -= 1;
                if (type == 4)
                {
                    gameObject.transform.position = other.transform.position + Vector3.up;
                    gameObject.transform.Rotate(Vector3.up * Random.Range(0, 360f));
                    gameObject.transform.Rotate(Vector3.right * -10);
                }
            }
            other.SendMessage("AddDamage", damage);
        }
        else if (other.tag == "Player" && type == -1)
        {
            other.GetComponent<HeartSystem>().TakeDamage(-(int)damage);
            Destroy(this.gameObject);
        }
        
    }
}
