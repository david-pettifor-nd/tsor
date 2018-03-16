using UnityEngine;
using System.Collections;

public class ItemInView : MonoBehaviour {

    private bool is_seen;
    private bool can_pickup;

	// Use this for initialization
	void Start () {
        is_seen = false;
        //Debug.Log("Loaded ITEM");
	}

	// Update is called once per frame
	void Update () {
        if (is_seen)
        {
            Vector3 diff = this.gameObject.transform.position - Camera.main.transform.position;
            if(Mathf.Abs(diff.x) + Mathf.Abs(diff.z) < 2)
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

        if(can_pickup && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Picking up key.");
            // hide the game object
            Destroy(this.gameObject);


            GameObject gamevalues = GameObject.Find("GameValues");
            Values vals = gamevalues.GetComponent("Values") as Values;
            Debug.Log(vals.inventory);
            vals.inventory.Add(new Item("Key"));

            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, -10, this.gameObject.transform.position.y);

            //Destroy(this.gameObject);
        }
    }

    void OnGUI()
    {
        if (can_pickup)
        {
            GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "Press E to pickup Key");
        }
    }

    void OnBecameVisible()
    {
        is_seen = true;

    }
    void OnBecameInvisible()
    {
        is_seen = false;
    }
}
