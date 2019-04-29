using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraderController : MonoBehaviour {

    public CageController cage;
    public int idCharOpen;
    SaveSystem ss;

    PlayerManager playerManager;

    // Use this for initialization
    void Start () {
        ss = GameObject.FindWithTag("Manager").GetComponent<SaveSystem>();
        playerManager = GameObject.FindWithTag("Manager").GetComponent<PlayerManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (playerManager.keyValue >= 40){
                if (!ss.SaveFileOpenCh(idCharOpen)) {
                    cage.OpenCage();
                    playerManager.keyValue -= 40;
                };
            };
            
        }
    }
}
