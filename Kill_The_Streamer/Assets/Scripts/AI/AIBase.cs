using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIBase : MonoBehaviour {

    protected int index;

    [SerializeField]
    protected int health;

    [SerializeField]
    protected int damage;

    [SerializeField]
    protected EnemyType aiType;

    public bool m_anarchyMode;

    protected FastTransform m_transform;

    protected float m_aiLoopTimer;
    protected float m_aiLoopMax = 0.1f;

    // Use this for initialization
    protected virtual void Start () {
        m_transform = this.GetComponent<FastTransform>();
        m_aiLoopTimer = m_aiLoopMax;
	}
	
    public int Index
    {
        get { return index; }
        set { index = value; }
    }

    public int Health
    {
        get { return health; }
        set { health = value; }
    }


    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    public EnemyType AIType
    {
        get { return aiType; }
    }

    public FastTransform FastTransform
    {
        get { return m_transform; }
    }

    protected virtual void Update()
    {
        m_aiLoopTimer -= Time.deltaTime;
        if(m_aiLoopTimer < 0.0f)
        {
            m_aiLoopTimer += m_aiLoopMax;
            AILoop();
            UpdateSpeed();
        }
    }

    /// <summary>
    /// This is called if the enemy takes damage. If the enemy reaches 0 health
    /// then set inactive
    /// </summary>
    public virtual void TakeDamage()
    {
        health--;
        if(health <= 0)
        {
            EnemyManager.DestroyEnemy(aiType, index);
        }
    }

    /// <summary>
    /// Function for enemy dealing damage to the player
    /// </summary>
    public abstract void DealDamage();

    public abstract void UpdateSpeed();

    public abstract void AILoop();
}
