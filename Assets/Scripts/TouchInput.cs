using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour {
    

    void Update () {
        //foreach(Touch touch in Input.touches)
        //      {
        //          if(touch.phase == TouchPhase.Began)
        //          {
        //              Debug.Log("Touched");
        //          }
        //      }
        if(Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            if(touch.phase == TouchPhase.Began)
            {
                Debug.Log("Tapped");
            }
        }
	}
}
