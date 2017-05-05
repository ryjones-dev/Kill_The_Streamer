using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerthaBullet : MonoBehaviour {
   // private float timeAlive = 1.5f; // Amount of time until the projectile is destroyed (range)
    private float speed = 15f;

    public float maxSize = 30.0f;
    public float scaleIncrease = .08f;

    private Vector3 bulletScale;

    private int skipFrame = 0;

    private Vector3 scalingVector;

    // Use this for initialization
    void Start () {
		bulletScale= this.transform.localScale;
        scalingVector = new Vector3(scaleIncrease, 0, scaleIncrease);
    }
	
	// Update is called once per frame
	void Update () {

        if(this.bulletScale.x >= maxSize || this.bulletScale.z >= maxSize)
        {
            return;
        }
        else
        {

            bulletScale = this.transform.localScale;

            bulletScale += scalingVector;
            
            this.transform.localScale = bulletScale;
        }

    }


    //should be destroyed if hits wall or enemies
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            AIBase ai = collision.GetComponent<AIBase>();
            ai.TakeDamage();
            Destroy(gameObject);
            CameraShake.AddShake(new global::Shake(bulletScale.sqrMagnitude / 2048, 0.05f));
        }

        if (collision.gameObject.CompareTag("Shield"))
        {
            AiShieldSeek shieldAI = collision.GetComponentInParent<AiShieldSeek>();

            shieldAI.shieldDestroy();
            //shieldAI.ShieldTakeDamage();
            Destroy(gameObject);
            CameraShake.AddShake(new global::Shake(bulletScale.sqrMagnitude / 2048, 0.05f));
        }

        
        if (collision.gameObject.CompareTag("Terrain"))
        {
            Destroy(gameObject);
            CameraShake.AddShake(new global::Shake(bulletScale.sqrMagnitude / 2048, 0.05f));
        }
        
    }
}
