using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Buggy3 : MonoBehaviour {

    Rigidbody rb;

    public Pusher2[] Pushers;

    public float Force;
    public float Torque;
    public float UphillForce;
    float pushingDuration;
    float rollUphillStartTime;
    float rollUphillDuration = 2;

    Vector3 lastPosition;
    Vector3 movement;

    private bool rollingUphill = false;
    private bool steering = false;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        Vector3 lastLocalPosition = transform.localPosition;
        Pushers[0].SetPusherActive(true);
    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey("w"))
        {
            rb.AddForce(transform.forward * Force);
        }
        if (Input.GetKey("s"))
        {
            rb.AddForce(-transform.forward * Force);
        }
        if(steering)
        {
            if (Input.GetKey("d"))
            {
                rb.AddTorque(transform.up * Torque);
            }
            else if (Input.GetKey("a"))
            {
                rb.AddTorque(-transform.up * Torque);
            }
        }
        
        movement = transform.position - lastPosition;
        lastPosition = transform.position;
	}
    public void Push(float pushingForce, float _pushingDuration)
    {
        rollUphillStartTime = Time.time;
        rollingUphill = true;
        rb.AddForce(transform.forward * pushingForce);
        UphillForce = pushingForce * 0.05f;
        pushingDuration = _pushingDuration;
    }

    private void FixedUpdate()
    {
        // Apply constant force
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 3))
        {
            // On ground
            // If downhill  
            float inclineAngle = Mathf.Round(Vector3.Angle(transform.forward, Vector3.up));
            if (inclineAngle > 90)
            {
                rollingUphill = false;
                rb.AddForce(transform.forward * Force);
            }
            else
            {
                // Add decreasing force
                if (rollingUphill)
                {
                    if (Time.time - rollUphillStartTime < rollUphillDuration)
                    {
                        float f = UphillForce * (rollUphillDuration - (Time.time - rollUphillStartTime)) / rollUphillDuration;
                        rb.AddForce(transform.forward * UphillForce);
                    }
                    else
                        rollingUphill = false;
                }
            }
        }
    }
    public void EnableSteering(bool b)
    {
        steering = b;
    }
}
