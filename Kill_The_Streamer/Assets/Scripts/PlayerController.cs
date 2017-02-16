using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //variables
    public float speed = 0.8f;
    public bool dash = false;
    public float dashSpeed = 25.0f;
    public Vector3 velocity = new Vector3(0, 0, 0);
    public Vector3 maxVelocity = new Vector3 (5,5,5);
    float dashTime = 0.0f;


    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //check for input
        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 tempVelocity = new Vector3(0,0,0);
        cursorPos.y = 0;
        Debug.DrawLine(transform.position, cursorPos, Color.red);
        //check for dash
        if (dash == true)
        {
            if (Time.time - dashTime >= 0.5f)
            {
                dash = false;
                speed = speed / dashSpeed;
            }
        }
        //movement
        
        if (Input.GetKey(KeyCode.W) && dash == false)
        {

           tempVelocity += new Vector3(1, 0, 0);
        }

        if (Input.GetKey(KeyCode.A) && dash == false)
        {
            tempVelocity += new Vector3(0, 0, 1);
        }

        if (Input.GetKey(KeyCode.S) && dash == false)
        {
            tempVelocity += new Vector3(-1, 0, 0);
        }

        if (Input.GetKey(KeyCode.D) && dash == false)
        {
            tempVelocity += new Vector3(0, 0, -1);
        }

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) && dash == false)
        {
            speed = speed * dashSpeed;
            dash = true;
            dashTime = Time.time;
        }

        if (tempVelocity != Vector3.zero)
        {
            tempVelocity.Normalize();
        }

        velocity += tempVelocity * speed;

        //cap the speed
        if(velocity.x >= maxVelocity.x && dash == false)
        {
            velocity.x = maxVelocity.x;
        }

        if (velocity.x <= -maxVelocity.x && dash == false)
        {
            velocity.x = -maxVelocity.x;
        }

        if (velocity.y >= maxVelocity.y && dash == false)
        {
            velocity.y = maxVelocity.y;
        }

        if (velocity.y <= -maxVelocity.y && dash == false)
        {
            velocity.y = -maxVelocity.y;
        }

        if (velocity.z >= maxVelocity.z && dash == false)
        {
            velocity.z = maxVelocity.z;
        }

        if (velocity.z <= -maxVelocity.z && dash == false)
        {
            velocity.z = -maxVelocity.z;
        }

        transform.position += velocity * Time.deltaTime;
        velocity = velocity * 0.8f;
        //shooting
        if (Input.GetMouseButtonDown(0))
        {
            //when it will fire
        }
    }
}
