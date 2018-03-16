using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellScript : MonoBehaviour {

	public bool cell_seen;
	public int x;
	public int y;
	public float[] boundary_x;
	public float[] boundary_y;
	public float cell_size = 5;

	// Use this for initialization
	void Start () {
		cell_seen = false;
		boundary_x = new float[2];
		boundary_x [0] = (x * cell_size) - (cell_size / 2f);
		boundary_x [1] = (x * cell_size) + (cell_size / 2f);
		boundary_y = new float[2];
		boundary_y [0] = (y * cell_size) - (cell_size / 2f);
		boundary_y [1] = (y * cell_size) + (cell_size / 2f);

	}
	
	// Update is called once per frame
	void Update () {
		if (!cell_seen) {
			GameObject player = GameObject.FindGameObjectWithTag ("Player").gameObject;
			Vector3 player_location = player.transform.position;

			if (player_location.x >= boundary_x [0] && player_location.x <= boundary_x [1] && player_location.z >= boundary_y [0] && player_location.z <= boundary_y [1]) {
				cell_seen = true;
			} 
		}
	}


	void OnGUI()
	{
//		if (x == 0 && y == 0) {
//			string t = "V: ";
//			foreach (var c in GameObject.FindGameObjectsWithTag("level_cell")) {
//				CellScript cell_script = c.GetComponent<CellScript> ();
//				t = t + cell_script.cell_size.ToString ();
//			}
//			GUI.Label(new Rect(0, 0, Screen.width, Screen.height), t);
//		}
	}
}
