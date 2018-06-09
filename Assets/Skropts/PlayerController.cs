using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
		/*
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * speed);
        */

		Plane plane = new Plane(Vector3.up, transform.position);
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		float distance = 100;
		if (plane.Raycast (ray, out distance)) {
			Vector3 hitPoint = ray.GetPoint (distance);
			Vector3 projectionPoint = new Vector3 (hitPoint.x, 0.0f, hitPoint.z);
			Vector3 relativePosition = projectionPoint - transform.position;
			rb.AddForce (relativePosition);
		}
    }
}
