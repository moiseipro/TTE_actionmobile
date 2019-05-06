using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementhalEvents : MonoBehaviour {

    public GameObject split, splitOneHand, meteorite, physixMeteor;

    GameObject meteor;

    public Transform mainObjRot, leftHand, rightHand, player;
    Move_Controller mc;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        mc = GameObject.FindWithTag("Player").GetComponent<Move_Controller>();
    }

    public void CastFault()
    {
        Vector3 mainObj = (leftHand.transform.position + rightHand.transform.position)/2f;
        mainObj.y = 0;
        GameObject newSplit = Instantiate(split, mainObj , mainObjRot.rotation);
        newSplit.transform.localScale *= 1.2f;
        GameObject newMeteorite = Instantiate(meteorite, mainObj, mainObjRot.rotation);
        newMeteorite.transform.localScale *= 1.2f;
        Destroy(newMeteorite, 5f);
        Destroy(newSplit, 20f);
    }

    public void SpawnMeteorite()
    {
        meteor = Instantiate(physixMeteor, (leftHand.transform.position+rightHand.transform.position)/2f, mainObjRot.rotation, leftHand.transform);
        meteor.transform.localScale *= 0.011f;
        meteor.transform.localPosition -= Vector3.right/90;
    }

    public void ThrowMeteor()
    {
        Rigidbody rb = meteor.GetComponent<Rigidbody>();
        Vector3 vec = mc.GetVectorMove();
        vec.y = 0;
        rb.isKinematic = false;
        meteor.transform.parent = null;
        meteor.transform.localScale *= 1.2f;
        rb.AddForce((player.transform.position - leftHand.transform.position)* 1.4f + vec + Vector3.up * 1.5f, ForceMode.Impulse);
        Destroy(meteor, 5f);
    }

}
