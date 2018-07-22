using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : MonoBehaviour {

    private float startTouchTime;
    private float endTouchTime;
    private float swipeTime;
    private bool tap, swipeLeft, swipeRight, swipeUp, swipeDown;
    private bool isDragging = false;
    private Vector2 startTouch, swipeDelta;
    private float swipeSpeed;

    private void Update()
    {
        tap = swipeLeft = swipeRight = swipeUp = swipeDown = false;

        #region Standalone Inputs
        if(Input.GetMouseButtonDown(0))
        {
            startTouchTime = Time.time;
            tap = true;
            isDragging = true;
            startTouch = Input.mousePosition;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            endTouchTime = Time.time;
            isDragging = false;
            Reset();
        }
        DirectionCheck();
        #endregion
        #region Mobile Inputs
        if (Input.touches.Length > 0 )
        {
            if(Input.touches[0].phase == TouchPhase.Began)
            {
                startTouchTime = Time.time;
                tap = true;
                isDragging = true;
                startTouch = Input.touches[0].position;

            }
            else if(Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {

                endTouchTime = Time.time;
                isDragging = false;
                Reset();
            }
            DirectionCheck();
        }
        #endregion
        // Calculate the distance
        swipeDelta = Vector2.zero;
        if(isDragging)
        {
            if (Input.touches.Length > 0)
                swipeDelta = Input.touches[0].position - startTouch;
            else if (Input.GetMouseButton(0))
                swipeDelta = (Vector2)Input.mousePosition - startTouch;
        }

    }

    private void Reset()
    {
        startTouch = swipeDelta = Vector2.zero;
        isDragging = false;
    }

    private void DirectionCheck()
    {
        // Did we cross the deadzone?
        if (swipeDelta.magnitude > 100 && isDragging)
        {
            isDragging = false;
            swipeTime = Time.time - startTouchTime;
            Debug.Log("Swiped");
            // Which direction?
            float x = swipeDelta.x;
            float y = swipeDelta.y;
            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                // Left or right
                if (x < 0)
                    swipeLeft = true;
                else
                    swipeRight = true;
            }
            else
            {
                // Up or down
                if (y < 0)
                    swipeDown = true;
                else
                    swipeUp = true;
            }
            swipeSpeed = swipeDelta.magnitude / swipeTime;
            Reset();
        }
    }

    public int NumFingers { get { return Input.touches.Length; } }
    public Vector2 SwipeDelta { get { return swipeDelta; } }
    public bool SwipeLeft { get { return swipeLeft; } }
    public bool SwipeRight { get { return swipeRight; } }
    public bool SwipeUp { get { return swipeUp; } }
    public bool SwipeDown { get { return swipeDown; } }
    public float SwipeSpeed { get { return swipeSpeed; } }
}
