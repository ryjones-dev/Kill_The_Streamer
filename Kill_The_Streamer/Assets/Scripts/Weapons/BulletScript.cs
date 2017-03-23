using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Enemy"))
        {
            AIBase ai = collision.collider.GetComponent<AIBase>();
            ai.TakeDamage();
        }

        if(collision.collider.gameObject.CompareTag("Shield"))
        {
            AiShieldSeek shieldAI = collision.collider.GetComponentInParent<AiShieldSeek>();
            shieldAI.ShieldTakeDamage();
        }

        Destroy(this.gameObject);
    }
}
