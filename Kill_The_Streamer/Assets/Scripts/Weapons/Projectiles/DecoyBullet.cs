using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoyBullet : Targetable
{
    private Rigidbody rbody;

    private float m_timer = 0;
    private float travelTime = 0.25f;

    public int health = 100; // Number of times the decoy can damage enemies

    private void Start()
    {
        rbody = GetComponent<Rigidbody>();
    }

    protected override void Update()
    {
        base.Update();

        if (rbody.velocity.sqrMagnitude > 0)
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
            health--;

            if(health <= 0)
            {
                Destroy(gameObject);
            }
        }

        if (other.gameObject.CompareTag("Terrain"))
        {
            Destroy(gameObject);
        }
    }
}
