using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            AIBase ai = collision.GetComponent<AIBase>();
            ai.TakeDamage();
        }

        if(collision.gameObject.CompareTag("Shield"))
        {
            AiShieldSeek shieldAI = collision.GetComponentInParent<AiShieldSeek>();
            shieldAI.ShieldTakeDamage();
        }

        Destroy(this.gameObject);
    }
}
