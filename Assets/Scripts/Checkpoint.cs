using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Buggy"))
        {
            Debug.Log("Cleared the chute in " + (Time.time - gameManager.GameStartTime).ToString() + " seconds");
        }
    }
}
