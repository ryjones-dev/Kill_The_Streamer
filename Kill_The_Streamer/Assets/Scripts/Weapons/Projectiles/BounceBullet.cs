using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceBullet : MonoBehaviour
{
    bool isDead = false;
    public Rigidbody myBody;

    void Start()
    {
        myBody = this.GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider collision)
    {
        if (!isDead)
        {
            if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Shield"))
            {
                AIBase ai = collision.GetComponent<AIBase>();
                ai.TakeDamage();
                Vector3 vec = myBody.velocity;
                Vector3 norm = myBody.velocity.normalized;
                float dot = Vector3.Dot(vec, norm);
                myBody.velocity = (-2 * (dot) * norm + vec);
            }

            if (collision.gameObject.CompareTag("Terrain"))
            {
                isDead = true;
                Destroy(this.gameObject);
            }
        }
    }
}
