using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buggy4 : MonoBehaviour {

    public float Force;
    public float SteerAngle;

    public WheelCollider SteeringWheel;
    Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        SteeringWheel.motorTorque = 0.0001f;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("a"))
        {
            SteeringWheel.steerAngle = -SteerAngle;
        }
        if(Input.GetKeyUp("a"))
        {
            SteeringWheel.steerAngle = 0f;
        }
        if (Input.GetKeyDown("d"))
        {
            SteeringWheel.steerAngle = SteerAngle;
        }
        if (Input.GetKeyUp("d"))
        {
            SteeringWheel.steerAngle = 0f;
        }
        if (Input.GetKey("space"))
        {
            rb.AddForce(transform.forward * Force);
        }
	}
}
