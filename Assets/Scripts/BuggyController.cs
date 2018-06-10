using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BuggyController : MonoBehaviour {

    Rigidbody rb;

    public int MaxHumanSpeed = 7; // m/s
    public float MaxAcceleration = 3f; // m/s2
    public float TopTapSpeed = 0.15f;
    float relativeTapSpeed;

    float speed;
    Vector3 velocity;
    float acceleration;

    float lastTouchTime;
    float startTime;

    Vector3 turnAccel;
    public float TurnSmoothing = 5f;

    // Wheels
    float steering;
    float torque;
    float vehicleSpeed;
    public float MaxSteeringAngle;
    public float MaxTorque;
    public WheelCollider TurningWheel;
    public WheelCollider LeftWheel;
    public WheelCollider RightWheel;
    
    float yaw; // Buggy current yaw

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        turnAccel = Input.acceleration;
        speed = 0;
        lastTouchTime = 0f;
        startTime = Time.time;
    }
    // Update is called once per frame
    void Update ()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            if (touch.phase == TouchPhase.Began)
            {
                // Update acceleration based on tap speed
                float tapSpeed = Time.time - lastTouchTime;
                lastTouchTime = Time.time;
                acceleration = Mathf.Min(MaxAcceleration * TopTapSpeed / tapSpeed, MaxAcceleration);
                relativeTapSpeed = Mathf.Min(TopTapSpeed/tapSpeed,1);
            }
        }
        else
        {
            relativeTapSpeed = Mathf.Max(0,Mathf.Lerp(relativeTapSpeed, -0.5f, Time.deltaTime));
        }
        steering = MaxSteeringAngle * turnAccel.x;
        torque = MaxTorque * relativeTapSpeed;
        vehicleSpeed = transform.InverseTransformDirection(rb.velocity).z;
        turnAccel = Vector3.Lerp(turnAccel, Input.acceleration, TurnSmoothing * Time.deltaTime);

        // Update velocity
        speed = Mathf.Min(speed + acceleration * Time.deltaTime, MaxHumanSpeed);
        velocity = transform.forward * speed;

        //// Keep buggy on track
        //if (transform.localEulerAngles.y > 180)
        //    yaw = transform.localEulerAngles.y - 360;
        //else
        //    yaw = transform.localEulerAngles.y;
        //if (yaw > 0.25f)
        //{
        //    TurningWheel.steerAngle = -5f;
        //}
        //else if(yaw < -0.25f)
        //{
        //    TurningWheel.steerAngle = 5f;
        //}
        //else if(yaw < 0.25f && yaw > -0.25f)
        //{
        //    TurningWheel.steerAngle = 0;
        //}
        TurningWheel.steerAngle = steering; // If user input steering
        // Ensure pusher stays below max speed
        if (vehicleSpeed < MaxHumanSpeed)
            LeftWheel.motorTorque = RightWheel.motorTorque = torque;
        else
            LeftWheel.motorTorque = RightWheel.motorTorque = 0;
        //Debug.Log("vehicle angle = " + yaw.ToString() + " deg, wheel angle = " + TurningWheel.steerAngle.ToString());
        Debug.Log("Yaw: " + yaw.ToString() + ", Steering Angle: " + TurningWheel.steerAngle.ToString());
    }
    private void FixedUpdate()
    {
        //rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        //transform.rotation = Quaternion.Euler(new Vector3(0, 90 * turnAccel.x, 0));

        //TurningWheel.steerAngle = steering; // If user input steering

        // Keep buggy straight on hills


        //Debug.Log("Wheel angle = " + TurningWheel.steerAngle.ToString());
        //Debug.Log("Buggy speed = " + transform.InverseTransformDirection(rb.velocity).z * 2.236 + " mph");
        //Debug.Log("Speed = " + rb.velocity.ToString());
        //Debug.Log("Torque = " + LeftWheel.motorTorque.ToString());
        //rb.MoveRotation(Quaternion.Euler(new Vector3(0, 90 * turnAccel.x, 0)));
    }
    //private bool EnableGyro()
    //{
    //    if (SystemInfo.supportsGyroscope)
    //    {
    //        gyro = Input.gyro;
    //        gyro.enabled = true;
    //        return true;
    //    }
    //    return false;
    //}
}
