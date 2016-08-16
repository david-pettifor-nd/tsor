using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Item : MonoBehaviour
{
    //public GameObject obj;
    public int amount;
    public string name;

    public Item(string new_name)
    {
        name = new_name;
        amount = 1;
    }
}

public class InventoryList : MonoBehaviour {

    public List<Item> itemlist;

    // Use this for initialization
    void Start()
    {
    }

	// Update is called once per frame
	void Update () {

	}
}
