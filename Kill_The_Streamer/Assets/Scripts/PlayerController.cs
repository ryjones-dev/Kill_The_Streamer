using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    //variables
    float speed = 1.0f;
    float maxSpeed;
    Rigidbody2D player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //check for input
        if (Input.GetKey("w"))
        {
            player.AddForce(Vector2.up * speed);
        }

        if (Input.GetKey("s"))
        {
            player.AddForce(Vector2.down * speed);
        }

        if (Input.GetKey("a"))
        {
            player.AddForce(Vector2.left * speed);
        }

        if (Input.GetKey("d"))
        {
            player.AddForce(Vector2.right * speed);
        }
    }
}
