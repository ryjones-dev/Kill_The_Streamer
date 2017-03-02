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

[RequireComponent(typeof(BooEnemyManager))]
[RequireComponent(typeof(SeekEnemyManager))]
[RequireComponent(typeof(GhostEnemyManager))]
public class EnemyManager : MonoBehaviour
{
    // A parent gameobject to put the enemies in for editor convenience
    private GameObject m_enemyParent;

    private Queue<EnemyNetworkInfo> m_enemyQueue; // A queue of enemies viewers spawn
    private Mutex m_enemyQueueMut; // A mutex to lock the queue for access by the network thread

    private int m_enemyTotal = 0; // A count of the total number of active enemies in the scene

    // Singleton instance
    private static EnemyManager s_instance;

    // Eneemy manager components
    private BooEnemyManager m_booEnemyManager;
    private SeekEnemyManager m_seekEnemyManager;
    private GhostEnemyManager m_ghostEnemyManager;

    private void Awake()
    {
        // Sets up the singleton
        if(s_instance == null)
        {
            s_instance = this;

            m_booEnemyManager = GetComponent<BooEnemyManager>();
            m_seekEnemyManager = GetComponent<SeekEnemyManager>();
            m_ghostEnemyManager = GetComponent<GhostEnemyManager>();

            m_enemyQueue = new Queue<EnemyNetworkInfo>();
            m_enemyQueueMut = new Mutex();

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
        m_booEnemyManager.Init(m_enemyParent.transform);
        m_seekEnemyManager.Init(m_enemyParent.transform);
        m_ghostEnemyManager.Init(m_enemyParent.transform);
    }

    /// <summary>
    /// Creates (actually activates) an enemy of a given type with a given index in the array. The index can be retreived the enemy's EnemyData script.
    /// </summary>
    public static GameObject CreateEnemy(EnemyType p_enemyType, string p_twitchUsername, Direction p_spawnDirection)
    {
        // Prevents creating a new enemy if there are already the maximum number of active enemies in the scene
        if (s_instance.m_enemyTotal >= Constants.MAX_ENEMIES) return null;

        switch(p_enemyType)
        {
            case EnemyType.BooEnemy:
                GameObject boo = s_instance.m_booEnemyManager.ActivateNextEnemy(p_twitchUsername, p_spawnDirection);
                if(boo != null) { s_instance.m_enemyTotal++; }
                return boo;

            case EnemyType.SeekEnemy:
                GameObject seek = s_instance.m_seekEnemyManager.ActivateNextEnemy(p_twitchUsername, p_spawnDirection);
                if (seek != null) { s_instance.m_enemyTotal++; }
                return seek;

            case EnemyType.GhostEnemy:
                GameObject ghost = s_instance.m_ghostEnemyManager.ActivateNextEnemy(p_twitchUsername, p_spawnDirection);
                if (ghost != null) { s_instance.m_enemyTotal++; }
                return ghost;

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
                bool booSuccess = s_instance.m_booEnemyManager.DeactivateEnemy(p_enemyIndex);
                if (booSuccess) { s_instance.m_enemyTotal--; }
                return booSuccess;

            case EnemyType.SeekEnemy:
                bool seekSuccess = s_instance.m_seekEnemyManager.DeactivateEnemy(p_enemyIndex);
                if (seekSuccess) { s_instance.m_enemyTotal--; }
                return seekSuccess;

            case EnemyType.GhostEnemy:
                bool ghostSuccess = s_instance.m_ghostEnemyManager.DeactivateEnemy(p_enemyIndex);
                if (ghostSuccess) { s_instance.m_enemyTotal--; }
                return ghostSuccess;

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
                return s_instance.m_booEnemyManager.GetActiveEnemy(p_index);

            case EnemyType.SeekEnemy:
                return s_instance.m_seekEnemyManager.GetActiveEnemy(p_index);

            case EnemyType.GhostEnemy:
                return s_instance.m_ghostEnemyManager.GetActiveEnemy(p_index);

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
                return s_instance.m_booEnemyManager.GetAllEnemies(out p_firstInactiveIndex);

            case EnemyType.SeekEnemy:
                return s_instance.m_seekEnemyManager.GetAllEnemies(out p_firstInactiveIndex);

            case EnemyType.GhostEnemy:
                return s_instance.m_ghostEnemyManager.GetAllEnemies(out p_firstInactiveIndex);

            default:
                p_firstInactiveIndex = -1;
                return null;
        }
    }

    public static void AddEnemyToQueue(EnemyNetworkInfo enemyInfo)
    {
        s_instance.m_enemyQueueMut.WaitOne();
        s_instance.m_enemyQueue.Enqueue(enemyInfo);
        s_instance.m_enemyQueueMut.ReleaseMutex();
    }

    private void Update()
    {
        while(s_instance.m_enemyQueue.Count > 0)
        {
            EnemyNetworkInfo enemyInfo = s_instance.m_enemyQueue.Dequeue();
            CreateEnemy(enemyInfo.type, enemyInfo.name, enemyInfo.direction);
        }
    }

    private void OnApplicationQuit()
    {
        m_enemyQueueMut.Close();
    }
}
