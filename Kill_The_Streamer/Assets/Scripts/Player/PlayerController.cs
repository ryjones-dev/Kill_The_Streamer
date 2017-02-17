using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //variables
    
    public float defaultSpeed = 8.0f;
    private float speed;
    public bool dash = false;
    public float dashSpeed = 40.0f;
    public Vector3 velocity = new Vector3(0, 0, 0);
    public GameObject bulletPrefab;
    public float bulletSpeed = 10.0f;
    float dashTime = 0.0f;


    // Use this for initialization
    void Start()
    {
        speed = defaultSpeed;
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
                speed = defaultSpeed;
            }
        }
        //movement
        
        if (Input.GetKey(KeyCode.W) && dash == false)
        {

           tempVelocity += new Vector3(0, 0, 1);
        }

        if (Input.GetKey(KeyCode.A) && dash == false)
        {
            tempVelocity += new Vector3(-1, 0, 0);
        }

        if (Input.GetKey(KeyCode.S) && dash == false)
        {
            tempVelocity += new Vector3(0, 0, -1);
        }

        if (Input.GetKey(KeyCode.D) && dash == false)
        {
            tempVelocity += new Vector3(1, 0, 0);
        }

        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && dash == false)
        {
            speed = dashSpeed;
            dash = true;
            dashTime = Time.time;
        }

        if (tempVelocity != Vector3.zero)
        {
            tempVelocity.Normalize();
        }

        velocity += tempVelocity * speed;

        //cap the speed
        if(velocity.x >= defaultSpeed && dash == false)
        {
            velocity.x = defaultSpeed;
        }

        if (velocity.x <= -defaultSpeed && dash == false)
        {
            velocity.x = -defaultSpeed;
        }

        if (velocity.y >= defaultSpeed && dash == false)
        {
            velocity.y = defaultSpeed;
        }

        if (velocity.y <= -defaultSpeed && dash == false)
        {
            velocity.y = -defaultSpeed;
        }

        if (velocity.z >= defaultSpeed && dash == false)
        {
            velocity.z = defaultSpeed;
        }

        if (velocity.z <= -defaultSpeed && dash == false)
        {
            velocity.z = -defaultSpeed;
        }

        transform.position += velocity * Time.deltaTime;
        transform.forward = (cursorPos - transform.position).normalized;
        velocity = velocity * 0.8f;
        //shooting
        if (Input.GetMouseButtonDown(0))
        {
            //when it will fire
            GameObject bullet = (GameObject)Instantiate(bulletPrefab, new Vector3(this.transform.position.x, 0, this.transform.position.z), Quaternion.identity);
            bullet.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
        }
    }
}
