using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverController : MonoBehaviour {

    public Animator activatebleObject;
    private Animator thisAnim;

	// Use this for initialization
	void Start () {
        thisAnim = GetComponent<Animator>();
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            thisAnim.SetTrigger("Activate");
            activatebleObject.SetTrigger("Activate");
        }
    }

    public void TakeDamage()
    {
        gameObject.tag = "Map";
        thisAnim.SetTrigger("Activate");
        activatebleObject.SetTrigger("Activate");
    }
}
