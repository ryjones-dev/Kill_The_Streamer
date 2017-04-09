using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet_Alien : MonoBehaviour {

    bool isDead = false;
    Rigidbody myBody;

    void Start()
    {
        myBody = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider collision)
    {
        if (!isDead)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                AIBase ai = collision.GetComponent<AIBase>();
                ai.TakeDamage();
                float speed = myBody.velocity.magnitude;
                Vector2 point = Random.insideUnitCircle.normalized;
                myBody.velocity = speed * new Vector3(point.x, 0, point.y);
                transform.rotation = Quaternion.LookRotation(new Vector3(point.x, 0, point.y), Vector3.up);
            }
            if (collision.gameObject.CompareTag("Terrain"))
            {
                isDead = true;
                Destroy(this.gameObject);
            }
        }
    }
}
