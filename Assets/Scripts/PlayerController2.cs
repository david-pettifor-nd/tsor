using UnityEngine;
using System.Collections;

public class PlayerController2 : MonoBehaviour {

    private Rigidbody rb;
    public float speed = 1f;
    Vector3 movement;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
		Camera MapCam = GameObject.Find ("MapCam").GetComponent<Camera>();
		if (MapCam.enabled) {

		} else {
			float moveHorizontal = Input.GetAxisRaw("Horizontal");
			float moveVertical = Input.GetAxisRaw("Vertical");

			movement.Set(moveHorizontal, 0f, moveVertical);

			movement = movement.normalized * speed * Time.deltaTime;

			//rb.AddRelativeForce(moveHorizontal, 0, moveVertical);
			rb.MovePosition(transform.position + movement);
		}
    }
}
