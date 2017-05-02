using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorBullet : MonoBehaviour
{
    public Vector3 lightDirection;

    private GameObject shadow;

    private void Start()
    {
        shadow = transform.FindChild("Shadow").gameObject;
    }

    private void Update()
    {
        if(transform.position.y <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        shadow.transform.position = Vector3.Lerp(shadow.transform.position, transform.position, Time.fixedDeltaTime);
        shadow.transform.localScale *= 0.99f;

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
