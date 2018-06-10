using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pusher2 : MonoBehaviour {

    CharacterController controller;
    Vector3 look;
    Vector3 move;
    float speed;
    public float MaxSpeed;
    public float AccelerationAmount;
    public float DeccelerationAmount;
    public float MinTapSpeed = 0.1f;
    public float MaxTapSpeed = 0.5f;
    float tapStartTime;
    float tapSpeed;
    public float shoveTime;
    public float shoveWaitTime;

    private bool atPushbar;
    public bool AtPushbar
    {
        get
        {
            return atPushbar;
        }
        set
        {
            atPushbar = value;
            if(atPushbar)
            {
                Debug.Log("Pusher grabbed pushbar");
                Buggy.GetComponent<Rigidbody>().isKinematic = true;
                MaxSpeed /= 1.25f;
                AccelerationAmount /= 1.5f;
                transform.rotation = Quaternion.LookRotation(Buggy.transform.forward);
                Buggy.transform.SetParent(transform);
                Buggy.EnableSteering(false);
            }
            else
            {
                Debug.Log("Pusher released pushbar");
                MaxSpeed *= 1.25f;
                AccelerationAmount *= 1.5f;
                Buggy.transform.SetParent(null);
                Buggy.GetComponent<Rigidbody>().isKinematic = false;
                Buggy.EnableSteering(true);
            }
        }
    }

    public int Hill;
    public Buggy3 Buggy;
    Pushbar pushbar;
    public float PushingForce;
    public float PushingDuration;

    bool active = false;
    bool transitioning = false;

    // Use this for initialization
    void Start ()
    {
        shoveTime = Time.time;
        tapStartTime = Time.time;
        controller = GetComponent<CharacterController>();
        pushbar = Buggy.GetComponentInChildren<Pushbar>();
    }
	
	// Update is called once per frame
	void Update () {
        if(active)
        {
            if (Input.GetKeyDown("space"))
            {
                Accelerate();
            }
            else if (Time.time - tapStartTime > 0.5f)
            {
                Deccelerate();
            }


            if (atPushbar)
            {
                if (Input.GetKey("a"))
                    transform.Rotate(-Vector3.up, 10 * Time.deltaTime);
                if (Input.GetKey("d"))
                    transform.Rotate(Vector3.up, 10 * Time.deltaTime);
            }
            else
            {
                if (!transitioning)
                {
                    look = new Vector3(pushbar.transform.position.x, transform.position.y, pushbar.transform.position.z) - transform.position;
                    transform.rotation = Quaternion.LookRotation(look);
                }
            }
        }
        else if (Time.time - tapStartTime > 0.5f)
        {
            Deccelerate();
        }
        if (Input.GetKeyDown("c"))
        {
            if (Buggy != null && atPushbar)
            {
                Buggy.transform.SetParent(null);
                //Buggy.GetComponent<Rigidbody>().isKinematic = false;
                AtPushbar = false;
                shoveTime = Time.time;
                Buggy.Push(PushingForce, PushingDuration);
            }
        }

        if (Input.GetKeyDown("v"))
        {
            if (Buggy != null && atPushbar)
            {
                Buggy.transform.SetParent(null);
                //Buggy.GetComponent<Rigidbody>().isKinematic = false;
                AtPushbar = false;
                shoveTime = Time.time;
                Buggy.Push(PushingForce + 1500, PushingDuration);
            }
        }
        //move = pushbar.transform.position - transform.position;
        //move = move - Vector3.up * 9.8f;
        //move = move.normalized;
        controller.Move(transform.forward* speed * Time.deltaTime);
        controller.Move(-Vector3.up * 9.8f * Time.deltaTime); // Gravity
        
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
    void Deccelerate()
    {
        if (speed > 0.5f)
        {
            speed = Mathf.Lerp(speed, 0, DeccelerationAmount * Time.deltaTime);
        }
        else
        {
            speed = 0;
        }
    }
    //public void SetAtPushbar(bool b)
    //{
    //    atPushbar = b;
    //    if (b)
    //    {
    //        //Buggy.GetComponent<Rigidbody>().isKinematic = true;
    //        MaxSpeed /= 1.25f;
    //        AccelerationAmount /= 1.5f;
    //        transform.rotation = Quaternion.LookRotation(Buggy.transform.forward);
    //        Buggy.transform.SetParent(transform);
    //        Buggy.EnableSteering(false);
    //    }
    //    else
    //    {
    //        MaxSpeed *= 1.25f;
    //        AccelerationAmount *= 1.5f;
    //        Buggy.transform.SetParent(null);
    //        //Buggy.GetComponent<Rigidbody>().isKinematic = false;
    //        Buggy.EnableSteering(true);
    //    }
    //}
    public void SetPusherActive(bool b)
    {
        active = b;
    }
    public void SetPusherTransitioning(bool b)
    {
        transitioning = b;
    }
}
