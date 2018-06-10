using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour {

    public int NextHill;
    Pusher3 previousPusher;
    Buggy5 buggy;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Buggy"))
        {
            // Next pusher starts running
            buggy = other.transform.parent.GetComponentInChildren<Buggy5>();
            buggy.InTransition = true;
            Debug.Log("Buggy entered transition zone to Hill " + NextHill);
            // UI indicate transition time
        }
    }
    private void OnTriggerExit(Collider other)
    {
        
        if(other.CompareTag("Pusher"))
        {
            Pusher3 pusher = other.GetComponent<Pusher3>();
            if (pusher.hill != NextHill && pusher.IsPushing)
            {
                previousPusher = other.GetComponent<Pusher3>();
                Debug.Log("Transition DQ");
            }
        }
    }
}
