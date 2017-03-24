using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour {

    private float timeAlive = 1f;

    // Use this for initialization
    void Start () {
        //set tag to trail
        gameObject.tag = "Trail";
        //destroy after a certain amount of time
        Destroy(gameObject, timeAlive);
        //another option would be to destroy after a certain number of other trail has been spawned
    }
}
