using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buggy : MonoBehaviour {

    public BuggyController2 controller;

    Vector3 lastPos;
    Vector3 dir;
    float speed;
    public float rotationSmoothing;
    public float force;
    public float turningForce;
    bool isTurning;
    float timeInterval = 0.5f;
    Vector3 buggyVelocity;

	// Use this for initialization
	void Start () {
        lastPos = transform.position;
        //StartCoroutine("UpdateSpeed");
	}
	
	// Update is called once per frame
	void Update () {
        if(lastPos == transform.position)
        {
            dir = transform.forward;
        }
        else
        {
            dir = transform.position - lastPos;
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), rotationSmoothing * Time.deltaTime);
        //speed = (transform.position - lastPos).magnitude / Time.deltaTime;
        //lastPos = transform.position;
        transform.position = controller.transform.position;

        if (Input.GetKeyDown("space"))
        {
            controller.Accelerate(dir, force);
        }
        if (Input.GetKey("d"))
        {
            //controller.Turn(transform.right, turningForce);
            controller.Turn2(1);
        }
        if (Input.GetKey("a"))
        {
            //controller.Turn(-transform.right, turningForce);
            controller.Turn2(-1);
        }

    }
    private void FixedUpdate()
    {
        buggyVelocity = (transform.position - lastPos) / Time.fixedDeltaTime;
        Debug.Log("Buggy velocity = " + Vector3.Dot(transform.forward, buggyVelocity).ToString());
        lastPos = transform.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), rotationSmoothing * Time.fixedDeltaTime);
        transform.position = controller.transform.position;
    }
    //IEnumerator UpdateSpeed()
    //{
    //    while(true)
    //    {
    //        speed = (transform.position - lastPos).magnitude / timeInterval;
    //        lastPos = transform.position;
    //        //Debug.Log(speed);
    //        yield return new WaitForSeconds(timeInterval);
    //    }
    //}
}
