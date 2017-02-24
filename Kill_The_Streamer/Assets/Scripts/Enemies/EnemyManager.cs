using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public enum EnemyType
{
    BooEnemy,
    SeekEnemy,
    GhostEnemy
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
        SeekEnemyManager.Init(m_enemyParent.transform);
        GhostEnemyManager.Init(m_enemyParent.transform);
    }

    /// <summary>
    /// Creates (actually activates) an enemy of a given type with a given index in the array. The index can be retreived the enemy's EnemyData script.
    /// </summary>
    public static GameObject CreateEnemy(EnemyType p_enemyType, string p_twitchUsername, int p_spawnLocation)
    {
        switch(p_enemyType)
        {
            case EnemyType.BooEnemy:
                return BooEnemyManager.ActivateNextEnemy(p_twitchUsername, p_spawnLocation);

            case EnemyType.SeekEnemy:
                return SeekEnemyManager.ActivateNextEnemy(p_twitchUsername, p_spawnLocation);

            case EnemyType.GhostEnemy:
                return GhostEnemyManager.ActivateNextEnemy(p_twitchUsername, p_spawnLocation);

            default:
                Debug.Log("Invalid enemy type to spawn: " + p_enemyType);
                return null;
        }
    }

    /// <summary>
    /// Destroys (actually deactivates) an enemy of a given type with a given index in the array. The index can be retreived the enemy's EnemyData script.
    /// </summary>
    public static bool DestroyEnemy(EnemyType p_enemyType, int p_enemyIndex)
    {
        switch(p_enemyType)
        {
            case EnemyType.BooEnemy:
                BooEnemyManager.DeactivateEnemy(p_enemyIndex);
                return true;

            case EnemyType.SeekEnemy:
                return SeekEnemyManager.DeactivateEnemy(p_enemyIndex);

            case EnemyType.GhostEnemy:
                return GhostEnemyManager.DeactivateEnemy(p_enemyIndex);

            default:
                Debug.Log("Invalid enemy type to destroy: " + p_enemyType);
                return false;
        }
    }

    /// <summary>
    /// Returns an enemy of a given type with a given index in the array. The index can be retreived the enemy's EnemyData script.
    /// </summary>
    public static GameObject GetEnemy(EnemyType p_enemyType, int p_index)
    {
        switch(p_enemyType)
        {
            case EnemyType.BooEnemy:
                return BooEnemyManager.GetActiveEnemy(p_index);

            case EnemyType.SeekEnemy:
                return SeekEnemyManager.GetActiveEnemy(p_index);

            case EnemyType.GhostEnemy:
                return GhostEnemyManager.GetActiveEnemy(p_index);

            default:
                return null;
        }
    }

    /// <summary>
    /// Returns an array of all enemies of a given type. Also provides the first inactive index through an out parameter.
    /// To loop through all active enemies of that type in the scene, use the first inactive index variable as the upper limit of the loop.
    /// All enemies before that index are guaranteed to be active, and all enemies at and after that index are guaranteed to be inactive.
    /// </summary>
    public static GameObject[] GetAllActiveEnemies(EnemyType p_enemyType, out int p_firstInactiveIndex)
    {
        switch(p_enemyType)
        {
            case EnemyType.BooEnemy:
                return BooEnemyManager.GetAllEnemies(out p_firstInactiveIndex);

            case EnemyType.SeekEnemy:
                return SeekEnemyManager.GetAllEnemies(out p_firstInactiveIndex);

            case EnemyType.GhostEnemy:
                return GhostEnemyManager.GetAllEnemies(out p_firstInactiveIndex);

            default:
                p_firstInactiveIndex = -1;
                return null;
        }
    }
}
