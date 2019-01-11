using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitemController : MonoBehaviour {

    public GameObject attributeTotems;

    public string totemName;
    public float maxHealth;
    private float health;
    [HideInInspector]
    public int id;

    public int damage;
    public bool periodDamage = false;
    float laserRange = 30f;

    private LineRenderer linerender;

    // Use this for initialization
    void Start () {
        health = maxHealth;
        if (totemName == "faces")
        {
            linerender = gameObject.AddComponent<LineRenderer>();
            linerender.widthMultiplier = 0.2f;
            linerender.material = Resources.Load("Materials/Statue/LaserFacesTotem") as Material;
            linerender.positionCount = 6;
            linerender.loop = true;
            linerender.SetPosition(0, gameObject.transform.position + Vector3.up);
            linerender.SetPosition(1, LineRendererCastDirection(Vector3.forward));
            linerender.SetPosition(2, LineRendererCastDirection(Vector3.back));
            linerender.SetPosition(3, gameObject.transform.position + Vector3.up);
            linerender.SetPosition(4, LineRendererCastDirection(Vector3.right));
            linerender.SetPosition(5, LineRendererCastDirection(Vector3.left));
        } else if (totemName == "guard")
        {
            linerender = gameObject.AddComponent<LineRenderer>();
            linerender.widthMultiplier = 0.2f;
            linerender.numCornerVertices = 10;
            linerender.material = Resources.Load("Materials/Statue/LaserGuardTotem") as Material;
            linerender.positionCount = 3;
            linerender.SetPosition(0, gameObject.transform.position + Vector3.up);
            linerender.SetPosition(1, gameObject.transform.position + Vector3.up);
            linerender.SetPosition(2, gameObject.transform.position + Vector3.up);
        }
    }
	
	// Update is called once per frame
	void Update () {
		if(totemName == "faces") {
            facesTotem();
        } else if (totemName == "guard") {
            guardTotem();
        }
	}

    public void atackTotem() {
        GameObject Bull = GameObject.Instantiate(attributeTotems, transform.position + Vector3.up, transform.rotation);
        Bull.transform.LookAt(GameObject.FindWithTag("Player").transform);
        Bull.GetComponent<Bullet_Options>().speed = 10;
        Bull.GetComponent<Bullet_Options>().rotationSpeed = 0;
        Bull.GetComponent<Bullet_Options>().damage = damage;
        Bull.GetComponent<Bullet_Options>().type = -1;
        Destroy(Bull, 5f);
    }

    public void facesTotem() {
        RayCastDirection(Vector3.forward);
        RayCastDirection(Vector3.left);
        RayCastDirection(Vector3.back);
        RayCastDirection(Vector3.right);
    }

    public void guardTotem()
    {
        linerender.SetPosition(0, gameObject.transform.position + Vector3.up);
        linerender.SetPosition(1, Vector3.Lerp(linerender.GetPosition(1), gameObject.transform.position + new Vector3(0, GameObject.Find("HPcontainer").transform.position.y, 0), Time.deltaTime * 20f));
        linerender.SetPosition(2, Vector3.Lerp(linerender.GetPosition(2), GameObject.Find("HPcontainer").transform.position, Time.deltaTime * 20f));
    }

    public void RayCastDirection(Vector3 dir) {
        RaycastHit hit;
        if (periodDamage == false && Physics.Raycast(transform.position + Vector3.up * 1.8f, transform.TransformDirection(dir), out hit, Mathf.Infinity) && hit.transform.gameObject.tag == "Player")
        {
            periodDamage = true;
            Debug.Log("Жжется");
            hit.transform.gameObject.GetComponent<HeartSystem>().TakeDamage(-damage);
            StartCoroutine(PlayerDamagePerTime());
        }
    }
    public Vector3 LineRendererCastDirection(Vector3 dir)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 1.8f, transform.TransformDirection(dir), out hit, Mathf.Infinity) && (hit.transform.gameObject.tag == "Object" || hit.transform.gameObject.tag == "Enemy" || hit.transform.gameObject.tag == "Map"))
        {
            if (dir == Vector3.left || dir == Vector3.right) return new Vector3(hit.point.x, transform.position.y + 1f, transform.position.z);
            else if(dir == Vector3.forward || dir == Vector3.back) return new Vector3(transform.position.x, transform.position.y + 1f, hit.point.z);
        }
        return dir * -laserRange + gameObject.transform.position + Vector3.up;
    }

    void OnTriggerStay(Collider other)
    {
        if(totemName == "poison" && other.tag == "Player" && periodDamage == false)
        {
            periodDamage = true;
            Debug.Log("Отрава");
            other.GetComponent<HeartSystem>().TakeDamage(-damage);
            StartCoroutine(PlayerDamagePerTime());
        }
    }

        public IEnumerator PlayerDamagePerTime()
    {
        yield return new WaitForSeconds(1f);
        periodDamage = false;
    }

    public void AddDamage(float dam)
    {
        Debug.Log(health);
        if (health > 0)
        {
            health -= dam;
            Debug.Log(health);
        }
        if (health < 1)
        {
            health = 0;
            Debug.Log("СМЭРТЬ");
            GameObject.Find("TheStatueAnswers").GetComponent<StatueController>().DeliteTotem(id);
            if(totemName == "guard") GameObject.Find("TheStatueAnswers").GetComponent<StatueController>().bossHeart.immortality = false;
            Destroy(gameObject);
        }
    }
}
