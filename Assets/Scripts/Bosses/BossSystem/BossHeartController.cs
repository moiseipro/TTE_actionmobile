using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHeartController : MonoBehaviour {

    public float maxHealth;
    private float health;
    private bool dead = false;

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

    public IEnumerator AddDamage(float dam)
    {
        if (health > 1) {
            health -= dam;
            Debug.Log(health);
        }
        if (health < 1 && !dead)
        {
            health = 0;
            dead = true;
            Debug.Log("СМЭРТЬ");
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
            for (int i = 0; i < Random.Range(8, 15); i++)
            {
                GameObject part = Instantiate(partOfBoss[Random.Range(0, partOfBoss.Length)], gameObject.transform.position + Vector3.up, transform.rotation);
                float randomScale = Random.Range(100, 200);
                part.GetComponent<Transform>().localScale = new Vector3(randomScale, randomScale, randomScale);
                part.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-5, 5), Random.Range(1, 7), Random.Range(-5, 5)), ForceMode.Impulse);
                Destroy(part, Random.Range(5f, 15f));
                yield return new WaitForSeconds(0.2f);
            }
            GameObject Item = Instantiate(GameObject.FindWithTag("Manager").GetComponent<DropItemController>().DropItem(), gameObject.transform.position + Vector3.up, transform.rotation);
            Item.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-5, 5), Random.Range(1, 7), Random.Range(-5, 5)), ForceMode.Impulse);
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
