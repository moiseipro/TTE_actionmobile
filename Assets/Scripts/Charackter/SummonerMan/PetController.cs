using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetController : MonoBehaviour {

    public bool isBattle = false;
    public int type;
    public int level;
    public int maxTarget;
    public List<GameObject> target;
    public float damage;
    public float timeReload;
    private bool reload = false;
    public GameObject petBullet;

    public GameObject[] evolution;
    private LineRenderer line;

	// Use this for initialization
	void Start () {
        if (type == 1) line = GetComponent<LineRenderer>();
	}

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Есть контакт");
        if (target.Count < maxTarget)
        {
            if (other.tag == "LootBox")
            {
                other.SendMessage("OpenChest");
            }
            if (other.tag == "Enemy" || other.tag == "Boss" || other.tag == "Object")
            {
                Debug.Log("Враг найден");
                target.Add(other.gameObject);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy" || other.tag == "Boss" || other.tag == "Object") target.Remove(other.gameObject);
    }

    private void Update()
    {
        if (target.Count > 0 && isBattle == true)
        {
            if (reload == false && type == 0)
            {
                for (int i = 0; i < target.Count; i++)
                {
                    GameObject bull = Instantiate(petBullet, transform.position, transform.rotation);
                    bull.GetComponent<PetBullet>().damage = damage;
                    bull.transform.localScale = new Vector3(50, 50, 50);
                    if (target[i] != null) bull.transform.LookAt(target[i].transform);
                    else target.RemoveAt(i);
                    Destroy(bull, 5f);
                }
                reload = true;
                StartCoroutine(ReloadAtack());
            }
            else if (type == 1)
            {
                line.positionCount = target.Count + 1;
                line.SetPosition(line.positionCount-1, transform.position);
                for (int i = target.Count-1; i > -1; i--)
                {
                    if (target[i] == null)
                    {
                        target.RemoveAt(i);
                        break;
                    }
                    line.SetPosition(i, target[i].transform.position + Vector3.up/2f);
                    if (reload == false)
                    {
                        if (target[i].tag == "Object") target[i].SendMessage("TakeDamage");
                        else if (target[i].tag == "Enemy" || target[i].tag == "Boss") target[i].SendMessage("AddDamage", damage);
                    }
                }
                if (reload == false)
                {
                    reload = true;
                    StartCoroutine(ReloadAtack());
                }
            } else if (reload == false && type == 2)
            {
                for (int i = 0; i < target.Count; i++)
                {
                    GameObject bull = Instantiate(petBullet, transform.position, transform.rotation);
                    bull.GetComponent<PetBullet>().damage = damage;
                    if (target[i] != null) bull.GetComponent<Rigidbody>().AddForce((target[i].transform.position - gameObject.transform.position)/3f + Vector3.up * 5f, ForceMode.Impulse);
                    else target.RemoveAt(i);
                    Destroy(bull, 5f);
                }
                reload = true;
                StartCoroutine(ReloadAtack());
            }
        } else
        {
            if (type == 1) line.positionCount = 0;
        }  
    }

    IEnumerator ReloadAtack()
    {
        yield return new WaitForSeconds(timeReload);
        reload = false;
    }
}
