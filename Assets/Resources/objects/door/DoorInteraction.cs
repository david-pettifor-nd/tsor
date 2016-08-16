using UnityEngine;
using System.Collections.Generic;

public class DoorInteraction : MonoBehaviour {

    private bool is_seen;
    private bool is_reachable;
    private bool is_openable;

	// Use this for initialization
	void Start () {
        is_seen = false;
        is_reachable = false;
        is_openable = false;
	}

	// Update is called once per frame
	void Update () {
        Renderer rend = this.GetComponentInChildren<Renderer>();

        if (rend.isVisible)
        {
            Vector3 diff = this.gameObject.transform.position - Camera.main.transform.position;
            //Debug.Log(diff);
            if (Mathf.Abs(diff.x) + Mathf.Abs(diff.z) < 3)
            {
                //Debug.Log("Press E to pickup key");
                is_reachable = true;
                //Debug.Log("Door is now reachable");
                GameObject mapgen = GameObject.Find("GameValues");
                Values val = mapgen.GetComponent("Values") as Values;
                List<Item> list = val.inventory;
                Debug.Log(list);
                for (int i = 0; i < list.Count; i++)
                {
                   Debug.Log(list[i]);
                    if (list[i].name == "Key")
                    {
                        is_openable = true;
                        is_reachable = true;
                        break;
                    }
                }
            }
            else
            {
                is_reachable = false;
            }
        }
        else
        {
            is_reachable = false;
        }


        if (is_reachable && Input.GetKeyDown(KeyCode.E) && is_openable)
        {
            //Debug.Log("Interacting with door...");
            // do we have the key?
            GameObject mapgen = GameObject.Find("GameValues");
            Values val = mapgen.GetComponent("Values") as Values;
            Debug.Log(val);
            List<Item> list = val.inventory;
            for (int i = 0; i < list.Count; i++)
            {
                if(list[i].name == "Key")
                {
                    // we have it!
                    // so remove it...
                    //Debug.Log("Opening door...");
                    GameObject gamevalues = GameObject.Find("GameValues");
                    Values vals = gamevalues.GetComponent("Values") as Values;
                    vals.width = vals.width + 1;
                    vals.height = vals.height + 1;
                    //GameObject level_key = list.itemlist[i].obj;
                    //Destroy(level_key);
                    list.RemoveAt(i);
                    //list.RemoveAt(i);

                    // do we have the artifacts?
                    if(HaveArtifacts()){
                      Debug.Log("Done!");
                      Application.Quit();
                      return;
                    }



                    vals.level = vals.level + 1;
                    Application.LoadLevel("MapGen");
                }
            }
        }
    }

    bool HaveArtifacts(){
      Debug.Log("Checking if you've won...");

      GameObject mapgen = GameObject.Find("GameValues");
      Values val = mapgen.GetComponent("Values") as Values;
      List<Item> list = val.inventory;

      // requirements
      bool scarab = false;
      Debug.Log(list.Count);
      Debug.Log(list);
      for (int i = 0; i < list.Count; i++){
        Debug.Log(list[i].name);
        if(list[i].name == "Scarab"){
          scarab = true;
          Debug.Log("You found the scarab!");
        }
      }

      Debug.Log("Returning...");
      Debug.Log(scarab);
      return scarab;
    }

    void OnGUI()
    {
        if (is_reachable)
        {
            if (is_openable)
            {
                GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "Press E to open door");
            }
            else
            {
                GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "This door requires a key to open...");
            }
        }
    }
}
