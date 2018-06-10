using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    Camera cam;
    public Buggy5 buggy;
    Transform target;
    public float SmoothSpeed  = 0.125f;
    public Vector3 Offset;
    public float pitch;
    public float yaw;
    public float currentZoom = 10f;

    private void Start()
    {
        cam = Camera.main;
        target = buggy.transform;
    }

    private void LateUpdate()
    {
        currentZoom = 20 * Mathf.Max(0.5f, buggy.Speed / 20);
        transform.position = target.position - Offset * currentZoom;
        transform.LookAt(target.position + Vector3.up * pitch + Vector3.right * yaw);
        //transform.rotation = Quaternion.Euler(Vector3.Lerp(transform.localEulerAngles, Target.eulerAngles, SmoothSpeed * Time.fixedDeltaTime));
    }

}
