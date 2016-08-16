using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Values : MonoBehaviour {

    public int width = 3;
    public int height = 3;
    public List<Item> inventory;
    public int level = 1;

    public bool scarab_generated = false;
    public bool staff_generated = false;
    public bool crown_generated = false;

    public int scarab_chance = 200; //(%; out of 100)

	// Use this for initialization
	void Start () {
        Cursor.visible = false;
	}

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);

        if(FindObjectsOfType(GetType()).Length > 1){
          Destroy(gameObject);
        }
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape) == true)
        {
            Application.Quit();
        }
    }
}
