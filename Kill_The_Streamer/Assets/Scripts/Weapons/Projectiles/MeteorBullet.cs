using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorBullet : MonoBehaviour
{
    private void Update()
    {
        if(transform.position.y <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        transform.localScale *= 0.97f;
        if (transform.localScale.magnitude < 1.0f)
        {
            transform.localScale = transform.localScale.normalized;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            AIBase ai = other.GetComponent<AIBase>();
            ai.TakeDamage();
        }
    }
}
