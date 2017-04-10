using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonGoldenBullets : MonoBehaviour {

    public float lifeTime = 0.1f;
    private float timer = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        timer += 1.0f * Time.deltaTime;
        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }


    //should be destroyed if hits wall or enemies
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
            Destroy(gameObject);
        }

    }
}
