using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuggyController2 : MonoBehaviour {

    Rigidbody rb;
    Vector3 lastPos;
    Vector3 vel;

    public float AngularVelocity;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = 20;
        lastPos = transform.position;
	}
    private void Update()
    {
        vel = (transform.position - lastPos) / Time.deltaTime;
        lastPos = transform.position;
        //Debug.Log("Velocity = " + vel.magnitude);
        Debug.Log("Ball Velocity = " + rb.velocity);
    }
    public void Accelerate(Vector3 dir, float f)
    {
        rb.AddForce(dir.normalized * f);
    }
    public void Turn(Vector3 dir, float f)
    {
        //rb.AddForce(dir.normalized * f);
    }
    public void Turn2(int i)
    {
        //Quaternion deltaRotation = Quaternion.Euler(new Vector3(0, i * AngularVelocity, 0) * Time.fixedDeltaTime);
        //Quaternion deltaRotation2 = Quaternion.AngleAxis(AngularVelocity * Time.fixedDeltaTime, Vector3.up);
        //rb.MoveRotation(deltaRotation2 * rb.rotation);
        rb.AddTorque(Vector3.up * 100 * i);
        //transform.Rotate(Vector3.up, 2f * i, Space.World);
    }


}
