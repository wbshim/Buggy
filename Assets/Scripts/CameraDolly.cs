using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDolly : MonoBehaviour {
    
    CameraOrbit cam;
    public float targetYaw;
    public float distance;
    public float smoothing;

	// Use this for initialization
	void Start () {
        cam = Camera.main.GetComponent<CameraOrbit>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Buggy"))
        {
            cam.ChangeYaw(targetYaw, smoothing);
            cam.ChangeDistance(distance, smoothing);
        }
    }
    
}
