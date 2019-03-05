using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHeartController : MonoBehaviour {

    [Header("Жизни и дополнительная броня")]
    public float maxHealth;
    [HideInInspector] public float health;
    public float maxArmor;
    [HideInInspector] public float armor;
    [HideInInspector] public bool immortality = false;
    [HideInInspector] public bool dead = false;
    public int bossLevel = 0;

    [Header("Дополнительные атрибуты")]
    public int damage = 1;
    [Range(0.5f, 2f)]
    public GameObject healthBadge;
    public Mesh[] meshesHeart;
    public GameObject[] partOfBoss;

    [HideInInspector] public GameObject Player;
    [HideInInspector] public BossIsland bossIsland;
    private DropItemController dropItemController;
    protected bool BossFight = false;

    public void BossFightStartRadius()
    {
        if (BossFight == false)
        {
            foreach (Collider col in Physics.OverlapSphere(transform.position, 8f))
            {
                if (col.tag == "Player")
                {
                    Debug.Log("Бой с боссом начался");
                    BossFight = true;
                    bossIsland.BossFightStart();
                    break;
                }
            }
        }
    }

    public void StartScript()
    {
        Player = GameObject.FindWithTag("Player");
        dropItemController = GameObject.FindWithTag("Manager").GetComponent<DropItemController>();
        health = maxHealth + bossLevel * 20;
        bossIsland = GameObject.FindWithTag("Arena").GetComponent<BossIsland>();
    }

    public void AddDamage(float dam)
    {
        if (immortality == false)
        {
            if(armor > 0)
            {
                armor -= dam;
                Debug.Log(armor);
            } else if (health > 0) {
                health -= dam;
                Debug.Log(health);
            }
        }
        if (health < 1 && !dead)
        {
            health = Mathf.Clamp(health, 0, maxHealth);
            dead = true;
            Debug.Log("СМЭРТЬ");
            bossIsland.BossFightStop();
            StartCoroutine(DropLoot());
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            GameObject.FindWithTag("GUI").GetComponent<SaveSystem>().sa.activeArtifact[0] = true; // для тестов сохранения артефактов
            GameObject.FindWithTag("GUI").GetComponent<SaveSystem>().SaveFile();
        }
        healthBadge.GetComponent<Animator>().SetTrigger("HitTrigger");
    }

    IEnumerator DropLoot()
    {
        for (int i = 0; i < Random.Range(8, 15); i++)
        {
            GameObject part = Instantiate(partOfBoss[Random.Range(0, partOfBoss.Length)], gameObject.transform.position + Vector3.up, transform.rotation);
            float randomScale = Random.Range(100, 200);
            part.GetComponent<Transform>().localScale = new Vector3(randomScale, randomScale, randomScale);
            part.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-5, 5), Random.Range(1, 7), Random.Range(-5, 5)), ForceMode.Impulse);
            Destroy(part, Random.Range(5f, 10f));
            yield return new WaitForSeconds(0.2f);
        }
        GameObject Item = Instantiate(dropItemController.DropArtifact(), gameObject.transform.position + Vector3.up, transform.rotation);
        Item.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-3, 3), Random.Range(1, 7), Random.Range(-3, 3)), ForceMode.Impulse);
        Destroy(gameObject);
    }

    public void UpdateHpContainers()
    {
        if (immortality == true)
        {
            healthBadge.GetComponentInChildren<MeshFilter>().sharedMesh = meshesHeart[8];
        }
        else if (armor > 0)
        {
            if (armor / maxArmor * 100 > 70) healthBadge.GetComponentInChildren<MeshFilter>().sharedMesh = meshesHeart[4];
            else if (armor / maxArmor * 100 > 50) healthBadge.GetComponentInChildren<MeshFilter>().sharedMesh = meshesHeart[5];
            else if (armor / maxArmor * 100 > 30) healthBadge.GetComponentInChildren<MeshFilter>().sharedMesh = meshesHeart[6];
            else if (armor / maxArmor * 100 <= 30 && health / maxHealth * 100 > 1) healthBadge.GetComponentInChildren<MeshFilter>().sharedMesh = meshesHeart[7];
        }
        else
        {
            if (health / maxHealth * 100 > 70) healthBadge.GetComponentInChildren<MeshFilter>().sharedMesh = meshesHeart[0];
            else if (health / maxHealth * 100 > 50) healthBadge.GetComponentInChildren<MeshFilter>().sharedMesh = meshesHeart[1];
            else if (health / maxHealth * 100 > 30) healthBadge.GetComponentInChildren<MeshFilter>().sharedMesh = meshesHeart[2];
            else if (health / maxHealth * 100 <= 30 && health / maxHealth * 100 > 0.01f) healthBadge.GetComponentInChildren<MeshFilter>().sharedMesh = meshesHeart[3];
            else healthBadge.GetComponentInChildren<MeshFilter>().sharedMesh = null;
        }
    }

    public void HealBoss(float amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, maxHealth);
        Debug.Log("Босс вылечен: "+health);
    }

}