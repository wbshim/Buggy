using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour {

    Camera cam;

    public Buggy5 buggy;
    public float distance = 20.0f;
    public float zoomSpd = 2.0f;

    public float xSpeed = 240.0f;
    public float ySpeed = 123.0f;

    public int yMinLimit = -723;
    public int yMaxLimit = 877;

    public float yawOffset;
    public float pitchOffset;
    public float xOffset;
    public float yOffset;

    private float x = 0.0f;
    private float y = 0.0f;

    Rigidbody rb;
    Transform target;
    bool GotInitDeltaAngle = false;
    float initDeltaAngle;
    public float targetClampYaw = 45f;
    float cameraAngle;

    IEnumerator changeYaw;
    IEnumerator changeDistance;

    public void Start()
    {
        cam = Camera.main;
        target = buggy.transform;
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        // Make the rigid body not change rotation
        if (rb)
            rb.freezeRotation = true;
        //StartCoroutine("AutoRotate");
    }

    public void LateUpdate()
    {
        if (target)
        {
            y = ClampAngle(y, yMinLimit, yMaxLimit);

            Quaternion rotation = Quaternion.Euler(y + pitchOffset, x + yawOffset, 0.0f);
            Vector3 position = rotation * new Vector3(xOffset, yOffset, -20) + target.position;

            transform.rotation = rotation;
            transform.position = position;
            if(!GotInitDeltaAngle)
            {
                initDeltaAngle = cameraAngle - buggy.GetAngle();
                GotInitDeltaAngle = true;
            }
        }
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

    public void ChangeYaw(float _targetYaw, float duration)
    {
        if(changeYaw != null)
        {
            StopCoroutine(changeYaw);
        }
        changeYaw = _ChangeYaw(_targetYaw, duration);
        StartCoroutine(changeYaw);
    }
    IEnumerator _ChangeYaw(float _targetYaw, float duration)
    {
        float startTime = Time.time;
        while (Mathf.Abs(yawOffset - _targetYaw) > 0.1f)
        {
            float t = (Time.time - startTime) / duration;
            yawOffset = Mathf.SmoothStep(yawOffset, _targetYaw, t);
            yield return null;
        }
    }

    public void ChangeDistance(float d, float duration)
    {
        if (changeDistance != null)
            StopCoroutine(changeDistance);
        changeDistance = _ChangeDistance(d, duration);
        StartCoroutine(changeDistance);
    }
    IEnumerator _ChangeDistance(float d, float duration)
    {
        float startTime = Time.time;
        while(Mathf.Abs(distance - d) > 0.1f)
        {
            float t = (Time.time - startTime) / duration;
            distance = Mathf.SmoothStep(distance, d, t);
            cam.orthographicSize = distance;
            yield return null;
        }
    }
    IEnumerator AutoRotate()
    {
        while(true)
        {
            float buggyAngle = buggy.GetAngle();
            cameraAngle = transform.localEulerAngles.y;
            float deltaAngle;
            if(cameraAngle - buggyAngle > 180)
            {
                deltaAngle = cameraAngle - 360 - buggyAngle;
            }
            else if (cameraAngle - buggyAngle < -180)
                deltaAngle = cameraAngle - buggyAngle + 360;
            else
            {
                deltaAngle = cameraAngle - buggyAngle;
            }
            
            //Debug.Log("cameraAngle - buggyAngle = " + deltaAngle);
            Debug.Log("Buggy Angle = " + buggyAngle + ", cameraAngle = " + cameraAngle + ", DeltaAngle = " + deltaAngle);
            if(deltaAngle < initDeltaAngle - targetClampYaw)
            {
                // Rotate camera CW
                x += xSpeed * 0.02f;
            }
            else if (deltaAngle > initDeltaAngle + targetClampYaw)
            {
                // Rotate camera CCW
                x -= xSpeed * 0.02f;
            }
            yield return null;
        }
    }
}
