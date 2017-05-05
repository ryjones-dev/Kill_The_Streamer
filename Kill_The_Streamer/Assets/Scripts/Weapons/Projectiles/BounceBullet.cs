using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceBullet : MonoBehaviour
{
    bool isDead = false;

    void OnCollisionEnter(Collision collision)
    {
		if (!isDead)
		{
			if (collision.collider.gameObject.CompareTag("Enemy"))
			{
				AIBase ai = collision.collider.GetComponent<AIBase>();
				ai.TakeDamage();
				isDead = true;
				Destroy(this.gameObject);
			}

			if (collision.collider.gameObject.CompareTag("Shield"))
			{
				AiShieldSeek shieldAI = collision.collider.GetComponentInParent<AiShieldSeek>();
				shieldAI.ShieldTakeDamage();
				isDead = true;
				Destroy(this.gameObject);
			}

		}
    }
}
