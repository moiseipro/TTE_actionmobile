using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour {

    public GameObject[] shopBox;

	// Use this for initialization
	void Start () {
		for(int i = 0; i< 4; i++)
        {
            GameObject objBox = Instantiate(shopBox[Random.Range(0, shopBox.Length)], transform.position, Quaternion.identity, gameObject.transform.parent);
            switch (i)
            {
                case 0:
                    objBox.transform.localPosition = new Vector3(0, 0.8f, -1.4f);
                    break;
                case 1:
                    objBox.transform.localPosition = new Vector3(0, 0.8f, 1.4f);
                    break;
                case 2:
                    objBox.transform.localPosition = new Vector3(-1.4f, 0.8f, 0);
                    break;
                case 3:
                    objBox.transform.localPosition = new Vector3(1.4f, 0.8f, 0);
                    break;
            }
            objBox.transform.rotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
