using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteorite : MonoBehaviour {

    float speed = 4;

    public bool physix;

    Rigidbody rb;

    private void Start()
    {
        if(physix == true)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    // Update is called once per frame
    void Update () {
        if (physix == false)
        {
            transform.Translate(transform.forward * Time.deltaTime * speed, Space.World);
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Destroy(gameObject, 0.1f);
        }
        if (other.tag == "Player")
        {
            other.SendMessage("TakeDamage", -1);
            other.GetComponent<BaffController>().CreateBaff(2, 1, 0);
        }
    }
}
