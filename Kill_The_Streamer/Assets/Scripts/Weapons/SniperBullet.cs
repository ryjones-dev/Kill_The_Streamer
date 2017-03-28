using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperBullet : MonoBehaviour
{

    private float timeAlive = 1.5f; // Amount of time until the projectile is destroyed (range)
    private float speed = 15f;

    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, timeAlive);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //should be destroyed if hits wall
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            AIBase ai = collision.GetComponent<AIBase>();
            ai.TakeDamage();
        }

        if (collision.gameObject.CompareTag("Shield"))
        {
            AiShieldSeek shieldAI = collision.GetComponentInParent<AiShieldSeek>();
            shieldAI.ShieldTakeDamage();
        }

        if (collision.gameObject.CompareTag("Terrain"))
        {
            //Debug.Log("Collided with obstacle");
            Destroy(gameObject);
        }
    }
}
