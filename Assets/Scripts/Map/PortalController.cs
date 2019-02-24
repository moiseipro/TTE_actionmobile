using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour {

    PlayerManager playerManager;

    public int portalID;

    private void Start()
    {
        playerManager = GameObject.FindWithTag("Manager").GetComponent<PlayerManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && portalID == 1)
        {
            playerManager.NextLevel();
        }
    }
}
