using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsarBullet : MonoBehaviour {

    private float timeAlive = 1.5f; // Amount of time until the projectile is destroyed (range)

    //going to pass in the speed and direction for the bullet
    private float speed=0;
    private Vector3 direction=new Vector3(1,1,1);//PLACEHOLDER

    public const float DELAY_TIMER = 1500f;
    private float currentTimer = 0f;



    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, timeAlive);
    }

    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }
    public Vector3 Direction
    {
        get { return direction; }
        set { direction = value; }
    }

    // Update is called once per frame
    void Update()
    {
        currentTimer++;
        if(currentTimer < DELAY_TIMER)
        {
            this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }
        else
        {
            this.gameObject.GetComponent<Rigidbody>().velocity = direction*speed;
        }
       
        //transform.Translate(Vector3.forward * Time.deltaTime * speed); //moves projectile in target direction
    }

    //should be destroyed if hits wall
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            AIBase ai = collision.GetComponent<AIBase>();
            ai.TakeDamage();
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Shield"))
        {
            AiShieldSeek shieldAI = collision.GetComponentInParent<AiShieldSeek>();
            shieldAI.ShieldTakeDamage();
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Terrain"))
        {
            //Debug.Log("Collided with obstacle");
            Destroy(gameObject);
        }
    }
}
