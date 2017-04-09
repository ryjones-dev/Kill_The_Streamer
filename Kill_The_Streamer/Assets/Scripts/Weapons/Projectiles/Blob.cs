using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob : MonoBehaviour {
	public const float life = 2.0f;
    public float lifeTime;
    public SpriteRenderer blobRenderer;
	// Use this for initialization
	void Start () {
		lifeTime = life;
        this.blobRenderer = this.GetComponentInChildren<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		lifeTime -= Time.deltaTime;
        this.blobRenderer.color = new Color(1, 1, 1, lifeTime / 2.0f);
		if(lifeTime <= 0){
			Destroy (this.gameObject);
		}
	}

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
    }
}
