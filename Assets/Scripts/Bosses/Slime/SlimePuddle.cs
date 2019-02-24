using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimePuddle : MonoBehaviour {

    GameObject target;

    BaffController baffController;

    private void Start()
    {
        baffController = GameObject.FindWithTag("Player").GetComponent<BaffController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            baffController.StaticBaff(40, 2);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            baffController.StaticBaff(0, 2);
        }
    }
    private void OnDestroy()
    {
        baffController.StaticBaff(0, 2);
    }

}
