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
        GameObject newSplit, newMeteorite;
        Vector3 mainObj = Vector3.zero;
        if (leftHand !=null && rightHand !=null) mainObj = (leftHand.transform.position + rightHand.transform.position)/2f;
        else if(leftHand !=null) mainObj = leftHand.transform.position;
        else if(rightHand != null) mainObj = rightHand.transform.position;
        mainObj.y = 0;
        if (leftHand != null && rightHand != null)
        {
            newSplit = Instantiate(split, mainObj, mainObjRot.rotation);
            for(int i = -1; i < 2; i++)
            {
                newMeteorite = Instantiate(meteorite, mainObj, mainObjRot.rotation);
                newMeteorite.transform.Rotate(Vector3.up * i * 30);
                newMeteorite.transform.localScale *= 1.3f;
                Destroy(newMeteorite, 5f);
            }
            
        }
        else
        {
            newMeteorite = Instantiate(meteorite, mainObj, mainObjRot.rotation);
            newMeteorite.transform.localScale *= 1.1f;
            Destroy(newMeteorite, 5f);
            newSplit = Instantiate(splitOneHand, mainObj, mainObjRot.rotation);
        }
        newSplit.transform.localScale *= 1.2f;
        Destroy(newSplit, 20f);
    }

    public void SpawnMeteorite()
    {
        if(leftHand != null) meteor = Instantiate(physixMeteor, leftHand.transform.position, mainObjRot.rotation, leftHand.transform);
        else if(rightHand != null) meteor = Instantiate(physixMeteor, rightHand.transform.position, mainObjRot.rotation, rightHand.transform);
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
        if(leftHand!=null) rb.AddForce((player.transform.position - leftHand.transform.position)* 1.4f + vec + Vector3.up * 1.5f, ForceMode.Impulse);
        else if(rightHand !=null) rb.AddForce((player.transform.position - rightHand.transform.position) * 1.4f + vec + Vector3.up * 1.5f, ForceMode.Impulse);
        Destroy(meteor, 5f);
    }

}
