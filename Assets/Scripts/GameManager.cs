using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    float gameStartTime;
    public float GameStartTime
    {
        get { return gameStartTime; }
    }
	// Use this for initialization
	void Start () {
        gameStartTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
