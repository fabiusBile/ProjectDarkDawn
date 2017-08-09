using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testInventory : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetKeyUp(KeyCode.A))
        {
            Inventory.Add(0);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            Inventory.Add(1);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            Inventory.Add(2);
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            Inventory.Add(3);
        }
        if (Input.GetKeyUp(KeyCode.G))
        {
            Inventory.Add(4);
        }
    }
}
