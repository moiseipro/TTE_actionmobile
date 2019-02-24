using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : MonoBehaviour {

    public bool isDamage = true;
    public int damage;
    public int timeDamage;
    private ParticleSystem ps;
    private BaffController baffController;

    private void Start()
    {
        ps = GetComponentInChildren<ParticleSystem>();
        baffController = GameObject.FindWithTag("Player").GetComponent<BaffController>();
    }

    public void ActivateDamage()
    {
        isDamage = true;
        ps.Play();
    }

    public void DeactivateDamage()
    {
        isDamage = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && isDamage == true)
        {
            baffController.CreateBaff(timeDamage, damage, 0);
            isDamage = false;
        }
    }
}
