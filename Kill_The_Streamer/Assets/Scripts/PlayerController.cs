using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    //variables
    float speed = 1.0f;
    Rigidbody2D player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //check for input
        if (Input.GetKey(KeyCode.W))
        {
            player.AddForce(Vector2.up * speed);
        }

        if (Input.GetKey(KeyCode.A))
        {
            player.AddForce(Vector2.left * speed);
        }

        if (Input.GetKey(KeyCode.S))
        {
            player.AddForce(Vector2.down * speed);
        }

        if (Input.GetKey(KeyCode.D))
        {
            player.AddForce(Vector2.right * speed);
        }
    }
}
