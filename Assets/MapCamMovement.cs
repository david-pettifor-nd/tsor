using UnityEngine;
using System.Collections;

public class CameraMovements : MonoBehaviour {

	void Update(){
		Camera mapcam = GameObject.Find ("MapCam").GetComponent<Camera>();

		Vector3 pos = mapcam.transform.position;

		pos.x += 1;
		pos.z += 1;

		mapcam.transform.position = pos;
	}
}