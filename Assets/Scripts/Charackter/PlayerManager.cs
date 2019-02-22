using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    private int moneyValue, keyValue;

	// Use this for initialization
	void Start () {
		
	}
	
    public void AddMoney(int val)
    {
        moneyValue += val;
    }

    public void AddKey()
    {
        moneyValue++;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
