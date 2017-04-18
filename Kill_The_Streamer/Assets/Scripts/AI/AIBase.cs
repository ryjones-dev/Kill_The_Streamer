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

    protected FastTransform m_target;
    public FastTransform Target { get { return m_target; } set { m_target = value; } }

    public bool m_anarchyMode;

    protected FastTransform m_transform;
    protected Rigidbody m_rigidbody;

    protected float m_aiLoopTimer;
    protected float m_aiLoopMax = 0.1f;

    // Use this for initialization
    public virtual void Start () {
        m_transform = this.GetComponent<FastTransform>();
        m_rigidbody = this.GetComponent<Rigidbody>();
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

    public Rigidbody RigidBody
    {
        get { return m_rigidbody; }
    }

    protected virtual void Update()
    {
        if(!m_target)
        {
            m_target = Player.s_Player.GetComponent<FastTransform>();
        }

        m_aiLoopTimer -= Time.deltaTime;
        if(m_aiLoopTimer < 0.0f)
        {
            m_aiLoopTimer += m_aiLoopMax;
            AILoop();
            UpdateSpeed();
        }
    }

	public virtual void Initialize()
	{
        GameObject decoy = GameObject.FindGameObjectWithTag("Decoy");
        if(decoy)
        {
            m_target = decoy.GetComponent<FastTransform>();
        }
        else
        {
            m_target = Player.s_Player.GetComponent<FastTransform>();
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
