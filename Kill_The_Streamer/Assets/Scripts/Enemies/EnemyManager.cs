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

    public static Queue<EnemyNetworkInfo> s_enemyQueue;
    public static Mutex s_enemyQueueMut;


    private void Awake()
    {
        // Sets up the singleton
        if(s_instance == null)
        {
            s_instance = this;

            s_enemyQueue = new Queue<EnemyNetworkInfo>();
            s_enemyQueueMut = new Mutex();

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

    public static GameObject CreateEnemy(EnemyType p_enemyType, string p_twitchUsername, Vector3 p_position)
    {
        switch(p_enemyType)
        {
            case EnemyType.BooEnemy:
                return BooEnemyManager.ActivateNextEnemy(p_twitchUsername, p_position);

            case EnemyType.SeekEnemy:
                return SeekEnemyManager.ActivateNextEnemy(p_twitchUsername, p_position);

            case EnemyType.GhostEnemy:
                return GhostEnemyManager.ActivateNextEnemy(p_twitchUsername, p_position);

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

            case EnemyType.SeekEnemy:
                return SeekEnemyManager.DeactivateEnemy(p_enemyIndex);

            case EnemyType.GhostEnemy:
                return GhostEnemyManager.DeactivateEnemy(p_enemyIndex);

            default:
                Debug.Log("Invalid enemy type to destroy: " + p_enemyType);
                return false;
        }
    }

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

    public static GameObject[] GetAllActiveEnemies(EnemyType p_enemyType, out int p_firstInactiveIndex)
    {
        switch(p_enemyType)
        {
            case EnemyType.BooEnemy:
                return BooEnemyManager.GetAllEnemies(out p_firstInactiveIndex);
            default:
                p_firstInactiveIndex = -1;
                return null;
        }
    }

    private void Update()
    {
        GameObject enemy = EnemyManager.GetEnemy(EnemyType.BooEnemy, 0);

        if(s_enemyQueue.Count > 0)
        {
            s_enemyQueueMut.WaitOne();

            while (s_enemyQueue.Count > 0)
            {
                EnemyNetworkInfo info = s_enemyQueue.Dequeue();
                CreateEnemy(info.type, info.name, info.position);
            }

            s_enemyQueueMut.ReleaseMutex();
        }
    }
}
