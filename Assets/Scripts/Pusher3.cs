using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pusher3 : MonoBehaviour {

    CharacterController controller;
    public int hill;

    // Tap speed
    public float minTapTime = 0.13f;
    float tapTime;
    float tapStartTime;
    bool isFirstTap = true;
    float tapSpeed; // Essentially percentage of minimum tap speed

    public float speedMultiplier = 1.5f;
    public float maxPusherSpeed;
    
    Vector3 velocity;
    Vector3 lastPos;

    Buggy5 buggy;
    bool isPushing = true;
    public bool IsPushing
    {
        get { return isPushing; }
        set
        {
            isPushing = value;
            if (isPushing)
                maxPusherSpeed /= speedMultiplier;
            else
                maxPusherSpeed *= speedMultiplier;
        }
    }

    // Use this for initialization
    void Start () {
        controller = GetComponent<CharacterController>();
        lastPos = transform.position;
	}
	
    public void SetBuggy(Buggy5 _buggy)
    {
        buggy = _buggy;
    }

    public void Run(float _speed, bool lookAtBuggy)
    {
        controller.Move(transform.forward * _speed * Time.deltaTime);
        controller.Move(-transform.up * 9.8f * Time.deltaTime);
        float relPosToBuggy = Vector3.Dot(buggy.transform.position - transform.position, transform.forward);
        if (relPosToBuggy > 0 && lookAtBuggy)
            LookAtBuggy();
    }
    void LookAtBuggy()
    {
        Vector3 buggyDirection = buggy.transform.position - transform.position;
        buggyDirection.y = 0;
        Quaternion lookDirection = Quaternion.LookRotation(buggyDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookDirection, 5 * Time.deltaTime);
    }
    public void TransitionOut()
    {
        // When player swipes left or right, makes current pusher exit in swiped direction
        StartCoroutine("_transitionOut");
    }
    IEnumerator _transitionOut()
    {
        Vector3 targetPosition = transform.position + transform.right;
        while(Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, 5 * Time.deltaTime);
            yield return null;
        }
    }
}
