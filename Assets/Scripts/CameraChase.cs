using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChase : MonoBehaviour {

    public float SmoothingFactor;
    public Vector3 DistanceOffset;
    public Buggy5 TargetBuggy;
    public float TargetDistanceOffset;
    Camera cam;
    public float currentX;
    public float currentY;

	// Use this for initialization
	void Start () {
        cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void LateUpdate()
    {
        cam.transform.position = Vector3.Lerp(cam.transform.position, TargetBuggy.transform.position - TargetBuggy.transform.forward * TargetDistanceOffset + DistanceOffset, Time.deltaTime * SmoothingFactor);
        cam.transform.LookAt(TargetBuggy.transform.position + TargetBuggy.transform.forward * TargetDistanceOffset);
    }
}
