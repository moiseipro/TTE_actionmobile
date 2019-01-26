using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimePuddle : MonoBehaviour {

    GameObject target;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            target = other.gameObject;
            other.GetComponent<Move_Controller>().StaticSpeedDebaf(40f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Move_Controller>().StaticSpeedDebaf(0f);
        }
    }
    private void OnDestroy()
    {
        if(target!=null) target.GetComponent<Move_Controller>().StaticSpeedDebaf(0f);
    }

}
