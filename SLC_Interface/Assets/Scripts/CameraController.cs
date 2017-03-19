using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private Rigidbody rb;

    public float speedMovement;
    public float speedRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        float last_axis = 0.0f;

        if (Input.GetKey(KeyCode.Y)) last_axis = 1.0f;
        if (Input.GetKey(KeyCode.X)) last_axis = -1.0f;
        Vector3 movement = new Vector3(moveHorizontal, last_axis, moveVertical);
        rb.velocity = movement * speedMovement;

        if (Input.GetKey(KeyCode.Q)) transform.Rotate(-Vector3.up * speedRotation * Time.deltaTime);
        if (Input.GetKey(KeyCode.E)) transform.Rotate(Vector3.up * speedRotation * Time.deltaTime);

    }
}
