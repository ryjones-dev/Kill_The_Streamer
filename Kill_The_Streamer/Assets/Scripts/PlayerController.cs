using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    //variables
    public float speed = 0.2f;
    public bool dash = false;
    public float dashSpeed = 28.0f;
    float dashTime = 0.0f;


    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        //check for input
        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorPos.y = 0;
        Debug.DrawLine(transform.position, cursorPos, Color.red);
        //check for dash
        if(dash == true)
        {
            if(Time.time - dashTime >= 0.5f)
            {
                dash = false;
                speed = speed / dashSpeed;
            }
        }
        //movement
        if(Input.GetKey(KeyCode.W) && dash == false)
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                speed = speed * dashSpeed;
                dash = true;
                dashTime = Time.time;

            }
            transform.position = transform.position + new Vector3(speed, 0, 0);
        }

        if (Input.GetKey(KeyCode.A) && dash == false)
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                speed = speed * dashSpeed;
                dash = true;
                dashTime = Time.time;
            }
            transform.position = transform.position + new Vector3(0, 0, speed);
        }

        if (Input.GetKey(KeyCode.S) && dash == false)
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                speed = speed * dashSpeed;
                dash = true;
                dashTime = Time.time;
            }
            transform.position = transform.position + new Vector3(-speed, 0, 0);
        }

        if (Input.GetKey(KeyCode.D) && dash == false)
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                speed = speed * dashSpeed;
                dash = true;
                dashTime = Time.time;
            }
            transform.position = transform.position + new Vector3(0, 0, -speed);
        }

        //shooting
        if (Input.GetMouseButtonDown(0))
        {
            //when it will fire
        }
    }
}
