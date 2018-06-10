using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Pusher : MonoBehaviour {

    Rigidbody rb;
    public Buggy3 buggy;
    Pushbar pushbar;
    public float MaxSpeed;
    public float MinTapSpeed = 0.1f;
    public float MaxTapSpeed = 0.5f;
    public float AccelerationAmount;
    public float DeccelerationAmount;
    public float PushingForce;
    public float PushingDuration;
    float speed;
    float tapStartTime;
    float tapSpeed;
    bool startedRunning = false;
    bool atPushbar = false;
    Vector3 look;
    Vector3 move;
    Vector3 incline;


    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        pushbar = buggy.GetComponentInChildren<Pushbar>();
        speed = 0;
        tapStartTime = Time.time;
                RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 3))
        {
            incline = Quaternion.AngleAxis(90, transform.right) * hit.normal;
            Debug.DrawRay(transform.position, incline * 5, Color.red);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown("space"))
        {
            Accelerate();
        }
        if(Input.GetKeyDown("v"))
        {
            if(buggy != null && atPushbar)
            {
                buggy.Push(PushingForce, PushingDuration);
            }
        }
        else if (Time.time - tapStartTime > 0.5f)
        {
            Slow();
        }
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 3))
        {
            incline = Quaternion.AngleAxis(90, transform.right) * hit.normal;
            Debug.DrawRay(transform.position, incline * 5, Color.red);
        }

        // Set direction of movement
        look = new Vector3(pushbar.transform.position.x, transform.position.y, pushbar.transform.position.z) - transform.position;
        move = pushbar.transform.position - transform.position;
        move = move.normalized;

    }
    void Slow()
    {
        if(speed > 0.5f)
        {
            speed = Mathf.Lerp(speed, 0, DeccelerationAmount * Time.deltaTime);
        }
        else
        {
            speed = 0;
        }
    }
    void Accelerate()
    {
        tapSpeed = Time.time - tapStartTime;
        tapStartTime = Time.time;
        if (tapSpeed < MaxTapSpeed)
        {
            speed = Mathf.Lerp(speed, Mathf.Min(MaxSpeed, MaxSpeed * (MinTapSpeed / tapSpeed)), AccelerationAmount * Time.deltaTime);
        }
    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + transform.forward * speed * Time.fixedDeltaTime);
        rb.rotation = Quaternion.LookRotation(look);
    }
    public void SetAtPushbar(bool b)
    {
        atPushbar = b;
    }
}
