using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoyBullet : MonoBehaviour
{
    private Rigidbody rbody;

    private float m_timer = 0;
    private float travelTime = 0.25f;

    private void Start()
    {
        rbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(rbody.velocity.sqrMagnitude > 0)
        {
            m_timer += Time.deltaTime;
            if (m_timer >= travelTime)
            {
                m_timer = 0;
                rbody.velocity = Vector3.zero;
            }
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
