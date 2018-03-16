using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Values : MonoBehaviour {

    public int width = 5;
    public int height = 5;
    public List<Item> inventory;
    public int level = 1;

    public bool scarab_generated = false;
    public bool staff_generated = false;
    public bool crown_generated = false;

    public int scarab_chance = 1000; //(%; out of 100)

	// Use this for initialization
	void Start () {
		Camera MapCam = GameObject.Find ("MapCam").GetComponent<Camera>();
		MapCam.enabled = false;
        Cursor.visible = false;



		// turn off all of the cell lights
		foreach (var lightObj in GameObject.FindGameObjectsWithTag("CellLight")) {
			lightObj.GetComponent<Light>().enabled = false;
		}
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
		Camera MapCam = GameObject.Find ("MapCam").GetComponent<Camera>();
		GameObject FPC = GameObject.Find ("FirstPersonCharacter");
        if (Input.GetKeyDown(KeyCode.Escape) == true)
        {
            Application.Quit();
        }

		if (Input.GetKeyDown(KeyCode.M) == true)
		{
			// adjust the map camera
			Vector3 update_cam = MapCam.transform.position;
			update_cam.z = FPC.transform.position.z;
			update_cam.x = FPC.transform.position.x;
			//update_cam.y = 20;  // leave the zoom where it was before.
			MapCam.transform.position = update_cam;


			FPC.GetComponent<Camera>().enabled = !FPC.GetComponent<Camera>().enabled;
			MapCam.enabled = !MapCam.enabled;

			if (MapCam.enabled) {
				// run through each cell created and hide the ceilings
				foreach (var ceilingObj in GameObject.FindGameObjectsWithTag("Ceiling")) {
					ceilingObj.GetComponent<Renderer>().enabled = false;
				}

				// turn on all of the cell lights for those we've visited

				foreach (var c in GameObject.FindGameObjectsWithTag("level_cell")) {
					CellScript cell_script = c.GetComponent<CellScript> ();
					if (cell_script.cell_seen) {
						// get the light object
						GameObject lightObj = c.transform.Find("CellLight").gameObject;
						lightObj.GetComponent<Light>().enabled = true;
					}
				}
			} 
			if (FPC.GetComponent<Camera>().enabled)
			{
				foreach (var ceilingObj in GameObject.FindGameObjectsWithTag("Ceiling")) {
					ceilingObj.GetComponent<Renderer>().enabled = true;
				}

				// turn off all of the cell lights
				foreach (var lightObj in GameObject.FindGameObjectsWithTag("CellLight")) {
					lightObj.GetComponent<Light>().enabled = false;
				}
			}
		}
	}

	void OnGUI(){
		
	}
}