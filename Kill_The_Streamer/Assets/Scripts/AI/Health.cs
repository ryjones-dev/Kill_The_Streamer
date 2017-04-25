using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    public int healthIncrease = 100;
	public SpriteRenderer m_spriteRenderer;
	private Sprite sprite;

	//keeps track of when to delete it
	public float m_timer;

	//MAXIMUM TIME ALIVE
	public const float LIFETIME = 8.0f;

	// Use this for initialization
	void Start () {
		//Player.s_Player
		m_timer=LIFETIME;
		m_spriteRenderer = this.GetComponent<SpriteRenderer>();
		sprite = m_spriteRenderer.sprite;
	}
	
	// Update is called once per frame
	void Update () {
		m_timer -= Time.deltaTime;
		if(m_timer > 0.0f && m_timer <= 3.0f)
		{
			if(((int)(m_timer * 8)) % 2 == 0)
			{
				this.m_spriteRenderer.enabled = false;
			}
			else
			{
				this.m_spriteRenderer.enabled = true;
			}
		}
		else if(m_timer <= 0.0f)
		{
			Destroy(this.gameObject);
		}
		
	}

    void OnTriggerEnter(Collider coll)
    {
        if(coll.tag == "Player")
        {
			if (Player.s_Player.m_health + healthIncrease < 40000) {
				Player.s_Player.TakeHealing (healthIncrease);
				Destroy (this.gameObject);
			}
        }
    }
}
