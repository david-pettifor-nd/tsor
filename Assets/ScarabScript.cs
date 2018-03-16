using UnityEngine;
using System.Collections;

public class ScarabScript : MonoBehaviour {

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
			Debug.Log("Picking up scarab.");
			// hide the game object
			Destroy(this.gameObject);


			GameObject gamevalues = GameObject.Find("GameValues");
			Values vals = gamevalues.GetComponent("Values") as Values;
			Debug.Log(vals.inventory);
			vals.inventory.Add(new Item("Scarab"));

			this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, -10, this.gameObject.transform.position.y);

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

	void OnBecameVisible()
	{
		is_seen = true;
		GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "Scarab VISIBLE");
		//GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 200f, 200f), "Press E to pickup");
	}
	void OnBecameInvisible()
	{
		is_seen = false;
	}
}
