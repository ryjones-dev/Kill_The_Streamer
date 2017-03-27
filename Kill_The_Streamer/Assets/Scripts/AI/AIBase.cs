using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBase : MonoBehaviour {

    protected int index;

    [SerializeField]
    protected int health;

    [SerializeField]
    protected int damage;

    [SerializeField]
    protected EnemyType aiType;

    // Use this for initialization
    void Start () {
		
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
    public virtual void DealDamage()
    {

    }
}
