using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraUpdate : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//this.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {

		if (this.enabled) {
			
			if (Input.GetKey(KeyCode.W)) 
			{
				Vector3 pos = this.GetComponent<Camera>().transform.position;
				pos.z += 0.1f;
				this.GetComponent<Camera> ().transform.position = pos;
			}
			if (Input.GetKey(KeyCode.S)) 
			{
				Vector3 pos = this.GetComponent<Camera>().transform.position;
				pos.z -= 0.1f;
				this.GetComponent<Camera> ().transform.position = pos;
			}

			if (Input.GetKey(KeyCode.A)) 
			{
				Vector3 pos = this.GetComponent<Camera>().transform.position;
				pos.x -= 0.1f;
				this.GetComponent<Camera> ().transform.position = pos;
			}

			if (Input.GetKey(KeyCode.D)) 
			{
				Vector3 pos = this.GetComponent<Camera>().transform.position;
				pos.x += 0.1f;
				this.GetComponent<Camera> ().transform.position = pos;
			}

			if (Input.GetAxis ("Mouse ScrollWheel") != 0) {
				Vector3 pos = this.GetComponent<Camera>().transform.position;

				if (pos.y < 5 && Input.GetAxis ("Mouse ScrollWheel") < 0) {
					// do nothing...stop zooming in
				} else {
					pos.y += (Input.GetAxis ("Mouse ScrollWheel") * 2);
					this.GetComponent<Camera> ().transform.position = pos;
				}
			}

		}
	}

}
