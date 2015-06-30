using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour {

	public float speed = 3.0f;
	public float rotSpeed = 180.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKey (KeyCode.W)) 
			transform.position += transform.forward * speed * Time.deltaTime;
		else if (Input.GetKey (KeyCode.S)) 
			transform.position -= transform.forward * speed * Time.deltaTime;

		if (Input.GetKey (KeyCode.A)) 
			transform.RotateAround (transform.position, Vector3.up, -rotSpeed * Time.deltaTime);
		else if (Input.GetKey (KeyCode.D)) 
			transform.RotateAround (transform.position, Vector3.up, rotSpeed * Time.deltaTime);
	}
}
