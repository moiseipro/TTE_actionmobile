using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageController : MonoBehaviour {

    Animator anim;
    bool opened = false;
    

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void OpenCage()
    {
        if (opened == false)
        {
            anim.SetTrigger("Opening");
            opened = true;
        }
    }
}
