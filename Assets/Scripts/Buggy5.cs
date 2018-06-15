using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buggy5 : MonoBehaviour {

    public Swipe swipeControls;
    float heatDuration;

    public float maxSpeed;
    float timeToShove;
    private float shoveStartTime;
    public float ShoveStartTime
    {
        get { return shoveStartTime; }
    }
    public float maxShoveStrength;
    float speed;
    public float Speed
    {
        get { return speed; }
    }
    float targetSpeed;
    float pusherSpeed;
    public float speedSmooth;
    public float rotationSpeed;
    Vector3 lastPos;
    Vector3 vel;
    public Vector3 Vel
    {
        get
        {
            return vel;
        }
    }
    Vector3 lookDirection = new Vector3();


    // Graphics
    public Transform GFX;
    float inclineAngle;
    bool skipMeshUpdate = false;
    private IEnumerator _tempSkipMeshUpdate;

    // Kinematics
    public float gravity = 20f;
    float acc;
    bool gravityEnabled = false;

    // Tapping parameters
    public float minTapTime;
    float tapTime;
    float tapStartTime;
    float tapSpeed; // Essentially percentage of minimum tap speed

    // Pushers
    public Pusher3[] pushers;
    Pusher3 currentPusher;
    public float maxPusherSpeed;
    bool beingPushed = true;
    private IEnumerator _setMaxPusherSpeed;

    // Buggy parameters
    [Range(0, 1)]
    public float rollingFriction;
    int currentHill;
    public int CurrentHill
    {
        get { return currentHill; }
        set { currentHill = value; }
    }
    bool inTransition;
    public bool InTransition
    {
        get { return inTransition; }
        set { inTransition = value; }
    }
    Pushbar pushbar;

    public LayerMask mask;
       

    public bool BeingPushed
    {
        get { return beingPushed; }
        set
        {
            if(value)
            {
                beingPushed = value;
                gravityEnabled = false;
                if (_tempSkipMeshUpdate != null)
                    StopCoroutine(_tempSkipMeshUpdate);
                _tempSkipMeshUpdate = tempSkipMeshUpdate(0.1f);
                speed = pusherSpeed;
            }
        }
    }
	// Use this for initialization
	void Start () {
        acc = 0f;
        lastPos = transform.position;
        currentHill = 1;
        StartCoroutine("LookDirection");
        currentPusher = pushers[0];
        
        foreach(Pusher3 p in pushers)
        {
            if (p != null)
            {
                p.SetBuggy(this);
                if (p.hill > 1)
                    p.IsPushing = false;
            }
        }
        heatDuration = 0;
        pushbar = GetComponentInChildren<Pushbar>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        TrackTime();
        vel = (transform.position - lastPos) / Time.deltaTime;
        lastPos = transform.position;
        RaycastHit hit;
        if (Physics.Raycast(transform.position + transform.up, -transform.up, out hit, 2, mask))
        {   
            if(!hit.transform.CompareTag("Buggy"))
            {
                transform.position = Vector3.Lerp(transform.position, hit.point, 15 * Time.deltaTime);
            }
        }
        
        if(Input.GetMouseButtonDown(0))
        {
            tapTime = Time.time - tapStartTime;
            tapSpeed = minTapTime / tapTime;
            tapStartTime = Time.time;
        }
        if (swipeControls.SwipeUp)
        {
            //Debug.Log("Swipe speed = " + swipeControls.SwipeSpeed + ". Shove strength = " + (maxShoveStrength * swipeControls.SwipeSpeed / 5000));
            if (beingPushed)
            {
                if (maxShoveStrength * swipeControls.SwipeSpeed / 5000 > speed + 2.5)
                    Shove(maxShoveStrength * swipeControls.SwipeSpeed / 5000);
                else
                {
                    Debug.Log("Current speed is " + speed + ". Shove strength = " + (maxShoveStrength * swipeControls.SwipeSpeed / 5000) + ". Shove not strong enough. Swipe faster.");
                }
            }
        }
        if(Input.GetKeyDown("c"))
        {
            Transition();
        }
        if (Input.GetKey("a"))
        {
            transform.Rotate(-transform.up * rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey("d"))
        {
            transform.Rotate(transform.up * rotationSpeed * Time.deltaTime);
        }
        if(Input.GetKeyDown("f"))
        {
            BeingPushed = true;
        }
        if (Input.GetKeyDown("space"))
            speed = 0;
        if (Time.time - tapStartTime > 0.5f)
        {
            if (tapSpeed > 0.05f)
                tapSpeed = Mathf.Lerp(tapSpeed, 0, 5f * Time.deltaTime);
            else
            {
                tapSpeed = 0;
            }
        }
        // Kinematics
        if(speed > 0)
        {
            if (Vector3.Angle(GFX.forward, vel) > 90)
            {
                acc = Mathf.Round((gravity * Mathf.Cos(inclineAngle) - Mathf.Abs(speed * rollingFriction)) * 1000f) / 1000f;
            }
        }
        else
        {
            acc = 0;
            speed = 0;
        }
        if (gravityEnabled)
        {
            speed += acc * Time.deltaTime;
            transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
        }
        else
        {
            if(beingPushed)
            {
                transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
                speed = Mathf.Floor(Mathf.Lerp(speed, maxSpeed * tapSpeed, 5f * Time.deltaTime) * 1000) / 1000;
            }
            else
            {
                gravityEnabled = true;
            }
        }

        pusherSpeed = Mathf.Floor(Mathf.Lerp(pusherSpeed, currentPusher.maxPusherSpeed * tapSpeed, 5f * Time.deltaTime) * 1000) / 1000;

        // Run the pusher
        if (currentPusher != null)
        {
            currentPusher.Run(pusherSpeed, true);
            CheckPushbarDistance();
        }
        if (inTransition)
            if (pushers[currentPusher.hill] != null)
            {
                pushers[currentPusher.hill].Run(pusherSpeed, false);
            }

        if (speed > 0)
        {
            BuggyMeshUpdate();
        }

        if (Mathf.Round(speed) <= 0)
        {
            if(!beingPushed && !gravityEnabled)
            {
                gravityEnabled = true;
            }
        }
    }

    void CheckPushbarDistance()
    {
        float d = Vector3.Distance(currentPusher.transform.position, pushbar.transform.position);
        if (d <= 1f)
        {
            // Check if shoved
            if (!BeingPushed)
            {
                // If pusher is close to pushbar for a while, set beingPushed to true
                if (Time.time - shoveStartTime > 0.25f)
                {
                    BeingPushed = true;
                    currentPusher.IsPushing = true;
                }
            }
        }
        else if (d > 1.5f)
        {
            // Speed up pusher
            if(beingPushed)
            {
                beingPushed = false;
                currentPusher.IsPushing = false;
            }
        }
    }
    public void Shove(float strength)
    {
        if(shoveStartTime > 0)
        {
            if(Time.time - shoveStartTime < 1f)
            {
                Debug.Log("Shoving too often!");
                return;
            }
        }
        beingPushed = false;
        currentPusher.IsPushing = false;
        // Skip graphics update for some time
        if (_tempSkipMeshUpdate != null)
            StopCoroutine(_tempSkipMeshUpdate);
        _tempSkipMeshUpdate = tempSkipMeshUpdate(0.25f);
        StartCoroutine(_tempSkipMeshUpdate);

        shoveStartTime = Time.time;
        speed = strength;
    }

    public void Transition()
    {
        // Transition to next pusher
        Pusher3 lastPusher = currentPusher;
        currentPusher = pushers[lastPusher.hill];
        inTransition = false;
        Debug.Log("Transitioning from Hill " + lastPusher.hill + " pusher to Hill " + currentPusher.hill + " pusher");
        lastPusher.TransitionOut();
    }
    void BuggyMeshUpdate()
    {
        inclineAngle = Vector3.Angle(GFX.forward, Vector3.up) * Mathf.Deg2Rad;
        if (vel.magnitude > 0.25f)
        {
            if (Quaternion.Angle(GFX.rotation, Quaternion.LookRotation(lookDirection)) > 0.5f)
            {
                if (!skipMeshUpdate)
                    GFX.rotation = Quaternion.Slerp(GFX.rotation, Quaternion.LookRotation(lookDirection), 10 * Time.deltaTime);
            }
            GFX.position = transform.position + GFX.transform.up / 2;
        }
    }
    IEnumerator setMaxPusherSpeed(float s, float d)
    {
        while(Vector3.Distance(transform.position, currentPusher.transform.position) < d)
        {
            yield return null;
        }
        maxPusherSpeed *= s;
    }
    IEnumerator LookDirection()
    {
        while(true)
        {
            if(vel.magnitude > 0.25f)
            {
                
                if (Vector3.Angle(GFX.forward, vel) > 90)
                {
                    lookDirection = lastPos - transform.position;
                }
                else
                {
                    lookDirection = transform.position - lastPos;
                }
            }
            yield return new WaitForSeconds(0.05f);
        }
    }
    IEnumerator tempSkipMeshUpdate(float t)
    {
        skipMeshUpdate = true;
        yield return new WaitForSeconds(t);
        skipMeshUpdate = false;
    }
    void TrackTime()
    {
        heatDuration += Time.deltaTime * 2;
        //Debug.Log("Time elapsed: " + heatDuration.ToString());
    }
    public float GetAngle()
    {
        return transform.eulerAngles.y;
    }

}
