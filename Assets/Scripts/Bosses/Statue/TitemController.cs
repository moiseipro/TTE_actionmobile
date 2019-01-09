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

    public float damage;

    private LineRenderer linerender;

    // Use this for initialization
    void Start () {
        health = maxHealth;
        if (totemName == "faces")
        {
            linerender = gameObject.AddComponent<LineRenderer>();
            linerender.widthMultiplier = 0.2f;
            linerender.material = Resources.Load("Materials/Statue/LaserFacesTotem") as Material;
            linerender.SetPosition(0, gameObject.transform.position + Vector3.up);
            linerender.SetPosition(1, gameObject.transform.position + Vector3.up);
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
        if(!RayCastDirection(Vector3.forward) && !RayCastDirection(Vector3.left) && !RayCastDirection(Vector3.back) && !RayCastDirection(Vector3.right))
        {
            linerender.SetPosition(0, gameObject.transform.position + Vector3.up);
            linerender.SetPosition(1, Vector3.Lerp(linerender.GetPosition(1), linerender.GetPosition(0), Time.deltaTime * 20f));
        }
    }

    public void guardTotem()
    {
        linerender.SetPosition(0, gameObject.transform.position + Vector3.up);
        linerender.SetPosition(1, Vector3.Lerp(linerender.GetPosition(1), gameObject.transform.position + new Vector3(0, GameObject.Find("HPcontainer").transform.position.y, 0), Time.deltaTime * 20f));
        linerender.SetPosition(2, Vector3.Lerp(linerender.GetPosition(2), GameObject.Find("HPcontainer").transform.position, Time.deltaTime * 20f));
    }

    public bool RayCastDirection(Vector3 dir) {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, transform.TransformDirection(dir), out hit, Mathf.Infinity) && hit.transform.gameObject.tag == "Player")
        {
            Debug.Log("Did Hit: " + hit.transform.gameObject.tag);
            linerender.SetPosition(0, gameObject.transform.position + Vector3.up);
            linerender.SetPosition(1, Vector3.Lerp(linerender.GetPosition(1),hit.point, Time.deltaTime * 20f));
            return true;
        }
        return false;
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
