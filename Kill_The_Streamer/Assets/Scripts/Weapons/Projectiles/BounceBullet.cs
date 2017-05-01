using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceBullet : MonoBehaviour
{
    bool isDead = false;

    void OnTriggerEnter(Collider collision)
    {
		if (!isDead)
		{
			if (collision.gameObject.CompareTag("Enemy"))
			{
				AIBase ai = collision.GetComponent<AIBase>();
				ai.TakeDamage();
				isDead = true;
				Destroy(this.gameObject);
			}

			if (collision.gameObject.CompareTag("Shield"))
			{
				AiShieldSeek shieldAI = collision.GetComponentInParent<AiShieldSeek>();
				shieldAI.ShieldTakeDamage();
				isDead = true;
				Destroy(this.gameObject);
			}

		}
    }
}
