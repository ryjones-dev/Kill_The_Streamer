using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    BooEnemy,
}

public class EnemyManager : MonoBehaviour
{
    // A parent gameobject to put the enemies in for editor convenience
    private GameObject m_enemyParent;

    // Singleton instance
    private static EnemyManager s_instance;

    private void Awake()
    {
        // Sets up the singleton
        if(s_instance == null)
        {
            s_instance = this;

            // Creates the enemy parent gameobject
            m_enemyParent = new GameObject("Enemies");

            // Initializes all enemy types
            InitEnemyTypes();

        }
        else
        {
            // Destroys the object if there is already a singleton instance
            Destroy(gameObject);
        }
    }

    private void InitEnemyTypes()
    {
        BooEnemyManager.Init(m_enemyParent.transform);
    }

    private void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            for (int i = 0; i < 1024; i++)
            {
                CreateEnemy(EnemyType.BooEnemy, "ExampleKappa", Vector3.zero);
            }
        }

        if(Input.GetButtonDown("Fire2"))
        {
            for (int i = 0; i < 1024; i++)
            {
                DestroyEnemy(EnemyType.BooEnemy, 0);
            }
        }
    }

    public static GameObject CreateEnemy(EnemyType p_enemyType, string p_twitchUsername, Vector3 p_position)
    {
        switch(p_enemyType)
        {
            case EnemyType.BooEnemy:
                return BooEnemyManager.ActivateNextEnemy(p_twitchUsername, p_position);

            default:
                Debug.Log("Invalid enemy type to spawn: " + p_enemyType);
                return null;
        }
    }

    public static bool DestroyEnemy(EnemyType p_enemyType, int p_enemyIndex)
    {
        switch(p_enemyType)
        {
            case EnemyType.BooEnemy:
                BooEnemyManager.DeactivateEnemy(p_enemyIndex);
                return true;

            default:
                Debug.Log("Invalid enemy type to destroy: " + p_enemyType);
                return false;
        }
    }
}
