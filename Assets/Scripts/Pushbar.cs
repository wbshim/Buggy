using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushbar : MonoBehaviour {

    Buggy5 buggy;

    private void Start()
    {
        buggy = GetComponentInParent<Buggy5>();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 1);
    }

}
