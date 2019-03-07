using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour {

    PlayerManager playerManager;
    GameObject player;

    public int portalID;

    private void Start()
    {
        playerManager = GameObject.FindWithTag("Manager").GetComponent<PlayerManager>();
        player = GameObject.FindWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && portalID == 1)
        {
            player.GetComponent<Move_Controller>().joystickFire.enabled = false;
            player.GetComponent<Move_Controller>().joystickMove.enabled = false;
            playerManager.NextLevel();
        }
    }
}
