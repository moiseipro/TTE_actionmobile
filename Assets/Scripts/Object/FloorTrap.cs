using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTrap : MonoBehaviour {

    public bool isDamage = true;

    public void ActivateDamage()
    {
        isDamage = true;
    }

    public void DeactivateDamage()
    {
        isDamage = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player" && isDamage == true)
        {
            other.gameObject.GetComponent<HeartSystem>().TakeDamage(-2);
            isDamage = false;
        }
    }

}
