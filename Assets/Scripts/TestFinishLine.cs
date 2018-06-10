using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFinishLine : MonoBehaviour {

    float startTime;
    float elapsedTime;

	// Use this for initialization
	void Start () {
        startTime = Time.time;
	}

    private void OnTriggerEnter(Collider other)
    {
        elapsedTime = Time.time - startTime;
        Debug.Log("Crossed in " + elapsedTime.ToString() + " seconds");
    }
}
