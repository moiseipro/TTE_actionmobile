using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHeartController : MonoBehaviour {

    public float maxHealth;
    private float health;

    public GameObject healthBadge;
    public Mesh[] meshesHeart;
    public GameObject[] partOfBoss;

    // Use this for initialization
    void Start () {
        health = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddDamage(float dam)
    {
        if (health > 1) {
            health -= dam;
            Debug.Log(health);
        }
        if (health < 1)
        {
            health = 0;
            Debug.Log("СМЭРТЬ");
            for(int i = 0; i < Random.Range(10,20); i++)
            {
                GameObject part = GameObject.Instantiate(partOfBoss[Random.Range(0, partOfBoss.Length)], gameObject.transform.position + Vector3.up, transform.rotation);
                part.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-5,5), Random.Range(1, 7), Random.Range(-5, 5)),ForceMode.Impulse);
                Destroy(part, Random.Range(5f, 15f));
            }
            Destroy(gameObject);
        }
        if (health / maxHealth * 100 > 70) healthBadge.GetComponentInChildren<MeshFilter>().sharedMesh = meshesHeart[0];
        else if(health / maxHealth * 100 > 50) healthBadge.GetComponentInChildren<MeshFilter>().sharedMesh = meshesHeart[1];
        else if (health / maxHealth * 100 > 30) healthBadge.GetComponentInChildren<MeshFilter>().sharedMesh = meshesHeart[2];
        else if(health / maxHealth * 100 <= 30 && health / maxHealth * 100 > 1) healthBadge.GetComponentInChildren<MeshFilter>().sharedMesh = meshesHeart[3];
        else healthBadge.GetComponentInChildren<MeshFilter>().sharedMesh = meshesHeart[4];
        healthBadge.GetComponent<Animator>().SetTrigger("HitTrigger");
    }
}
