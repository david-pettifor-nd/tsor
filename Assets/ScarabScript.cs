using UnityEngine;
using System.Collections;

public class ScarabScript : MonoBehaviour {

    private bool is_seen;
    private bool can_pickup;

    // Use this for initialization
    void Start()
    {
        is_seen = false;
        Debug.Log("Loaded SCARAB");
    }

    // Update is called once per frame
    void Update()
    {

        if (is_seen)
        {
            Vector3 diff = this.gameObject.transform.position - Camera.main.transform.position;
            if (Mathf.Abs(diff.x) + Mathf.Abs(diff.z) < 1)
            {
                //Debug.Log("Press E to pickup key");
                can_pickup = true;

            }
            else
            {
                can_pickup = false;
            }
        }
        else
        {
            can_pickup = false;
        }

        if (can_pickup && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Picking up scarab.");


            GameObject gamevalues = GameObject.Find("GameValues");
            Values vals = gamevalues.GetComponent("Values") as Values;
            Debug.Log(vals.inventory);
            Item scarab = new Item("Scarab");
            scarab.name = "Scarab";
            vals.inventory.Add(scarab);

            this.gameObject.transform.parent.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, -10, this.gameObject.transform.position.y);

            // hide the game object
            //Destroy(this.gameObject.transform.parent.gameObject);
            //Destroy(this.gameObject);
        }
    }

    void OnGUI()
    {
        if (can_pickup)
        {
            GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "Press E to pickup Scarab");
        }
    }

    public void OnBecameVisible()
    {
        Debug.Log("Scarab is visible");
        is_seen = true;

        //GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 200f, 200f), "Press E to pickup");
    }
    public void OnBecameInvisible()
    {
        is_seen = false;
    }
}
