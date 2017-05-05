using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBullet : MonoBehaviour
{

	private float speed = 1.67f;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
        float distancesqr = (Player.s_Player.FastTransform.Position - this.transform.position).sqrMagnitude;
        if (distancesqr < 169.0f)
        {
            CameraShake.AddShake(new Shake(0.2f * ((169.0f - distancesqr) / 169.0f), 0.01f));
        }
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
