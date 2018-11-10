using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChase : MonoBehaviour {

    public float SmoothingFactor;
    public Vector3 DistanceOffset;
    public Buggy5 TargetBuggy;
    Pushbar targetPushbar;
    public float TargetDistanceOffset;
    Camera cam;
    public float currentX;
    public float currentY;

	// Use this for initialization
	void Start () {
        cam = Camera.main;
        targetPushbar = TargetBuggy.pushbar;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void LateUpdate()
    {
        cam.transform.position = Vector3.Lerp(cam.transform.position, targetPushbar.transform.position - targetPushbar.transform.forward * TargetDistanceOffset + DistanceOffset, Time.deltaTime * SmoothingFactor);
        cam.transform.LookAt(targetPushbar.transform.position + targetPushbar.transform.forward * TargetDistanceOffset);
    }
}
