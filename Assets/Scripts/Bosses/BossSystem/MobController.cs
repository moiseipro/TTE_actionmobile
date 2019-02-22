using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobController : MonoBehaviour {

    public float maxHealth;
    protected float health;

    public float movSpeed = 10f;
    public int damage;
    public Mesh[] mobMesh;
    protected Rigidbody rb;
    protected GameObject Player, Boss;

    public void SearchForCh()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        Player = GameObject.FindWithTag("Player");
        Boss = GameObject.FindWithTag("Boss");
    }

    public void AddDamage(float damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);
        if (health < 1)
        {
            Destroy(gameObject);
        }
    }

    public void AnimationChoose(int num)
    {
        if (num < mobMesh.Length && num >= 0)
        {
            GetComponent<MeshFilter>().sharedMesh = mobMesh[num];
        }
        else
        {
            Debug.Log("Такой анимации нет");
        }
    }

}
