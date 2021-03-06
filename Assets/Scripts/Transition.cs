﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour {

    public int NextHill;
    Pusher3 previousPusher;
    Buggy5 buggy;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Buggy"))
        {
            // Next pusher starts running
            buggy = other.transform.parent.GetComponentInChildren<Buggy5>();
            Debug.Log("Buggy entered transition zone to Hill " + NextHill);
            if(NextHill != 3)
                buggy.InTransition = true;
            else if (NextHill == 3 && buggy.CurrentPusher.hill != 3)
                buggy.Transition();
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
