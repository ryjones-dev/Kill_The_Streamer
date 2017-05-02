using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleBullet : MonoBehaviour {
    // private float timeAlive = 1.5f; // Amount of time until the projectile is destroyed (range)


    public float maxSize = 3.0f;
    public float minSize = 0.3f;
    public float scaleIncrease = .1f;

    private bool swapSize = false;

    private Vector3 bulletScale;

    private Vector3 scalingVector;

    public float lifeTime = 6;
    private float timer = 0;

    // Use this for initialization
    void Start()
    {
        bulletScale = this.transform.localScale;
        scalingVector = new Vector3(scaleIncrease, 0, scaleIncrease);
    }

    // Update is called once per frame
    void Update()
    {

        if (this.bulletScale.x >= maxSize || this.bulletScale.z >= maxSize)
        {
            swapSize = true;
        }
        else if(this.bulletScale.x <= minSize || this.bulletScale.z <= minSize)
        {
            swapSize = false;
        }

        if(swapSize==false)
        {
            bulletScale = this.transform.localScale;

            bulletScale += scalingVector;

            this.transform.localScale = bulletScale;
        }
        else if(swapSize==true)
        {
            bulletScale = this.transform.localScale;

            bulletScale -= scalingVector;

            this.transform.localScale = bulletScale;
        }
  
        timer += 1.0f * Time.deltaTime;
        if(timer >= lifeTime)
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
