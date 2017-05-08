using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public enum EnemyType
{
    BooEnemy,
    SeekEnemy,
    GhostEnemy,
    ShieldEnemy,
    DestructoidEnemy,
    ShooterEnemy,
    TrailEnemy,
    HealthEnemy
}

[RequireComponent(typeof(BooEnemyManager))]
[RequireComponent(typeof(SeekEnemyManager))]
[RequireComponent(typeof(GhostEnemyManager))]
[RequireComponent(typeof(ShieldEnemyManager))]
[RequireComponent(typeof(DestructoidEnemyManager))]
[RequireComponent(typeof(ShooterEnemyManager))]
[RequireComponent(typeof(TrailEnemyManager))]
[RequireComponent(typeof(HealthEnemyManager))]

public class EnemyManager : MonoBehaviour
{
    // A parent gameobject to put the enemies in for editor convenience
    private GameObject m_enemyParent;

    private Queue<EnemyNetworkInfo> m_enemyQueue; // A queue of enemies viewers spawn
    private Mutex m_enemyQueueMut; // A mutex to lock the queue for access by the network thread

    private int m_enemyTotal = 0; // A count of the total number of active enemies in the scene

    private int m_enemyDeathCount = 0; // A count of the total number of enemies that have died

    // Singleton instance
    private static EnemyManager s_instance;

    // Enemy manager components
    private BooEnemyManager m_booEnemyManager;
    private SeekEnemyManager m_seekEnemyManager;
    private GhostEnemyManager m_ghostEnemyManager;
    private ShieldEnemyManager m_shieldEnemyManager;
    private DestructoidEnemyManager m_destructoidEnemyManager;
    private ShooterEnemyManager m_shooterEnemyManager;
    private TrailEnemyManager m_trailEnemyManager;
    private HealthEnemyManager m_healthEnemyManager;

    [SerializeField]
    private float m_speedMultiplier;
    private float m_speedTimer;

    public static float SpeedMultiplier { get { return s_instance.m_speedMultiplier; } }

    private void Awake()
    {
        // Sets up the singleton
        if (s_instance == null)
        {
            s_instance = this;
            m_speedMultiplier = 1.0f;
            m_speedTimer = 10.0f;

            m_booEnemyManager = GetComponent<BooEnemyManager>();
            m_seekEnemyManager = GetComponent<SeekEnemyManager>();
            m_ghostEnemyManager = GetComponent<GhostEnemyManager>();
            m_shieldEnemyManager = GetComponent<ShieldEnemyManager>();
            m_destructoidEnemyManager = GetComponent<DestructoidEnemyManager>();
            m_shooterEnemyManager = GetComponent<ShooterEnemyManager>();
            m_trailEnemyManager = GetComponent<TrailEnemyManager>();
            m_healthEnemyManager = GetComponent<HealthEnemyManager>();
            

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
        m_shieldEnemyManager.Init(m_enemyParent.transform);
        m_destructoidEnemyManager.Init(m_enemyParent.transform);
        m_shooterEnemyManager.Init(m_enemyParent.transform);
        m_trailEnemyManager.Init(m_enemyParent.transform);
        m_healthEnemyManager.Init(m_enemyParent.transform);
    }

    /// <summary>
    /// Creates (actually activates) an enemy of a given type with a given index in the array. The index can be retreived the enemy's EnemyData script.
    /// This function bypasses the enemy queue, so only call it for debugging purposes. Otherwise, use AddEnemyToQueue().
    /// </summary>
    public static GameObject CreateEnemy(EnemyType p_enemyType, string p_twitchUsername, Direction p_spawnDirection)
    {
        // Prevents creating a new enemy if there are already the maximum number of active enemies in the scene
        if (s_instance.m_enemyTotal >= Constants.MAX_ENEMIES) return null;

        switch (p_enemyType)
        {
            case EnemyType.BooEnemy:
                GameObject boo = s_instance.m_booEnemyManager.ActivateNextEnemy(p_twitchUsername, p_spawnDirection);
                if (boo != null) { s_instance.m_enemyTotal++; }
                return boo;

            case EnemyType.SeekEnemy:
                GameObject seek = s_instance.m_seekEnemyManager.ActivateNextEnemy(p_twitchUsername, p_spawnDirection);
                if (seek != null) { s_instance.m_enemyTotal++; }
                return seek;

            case EnemyType.GhostEnemy:
                GameObject ghost = s_instance.m_ghostEnemyManager.ActivateNextEnemy(p_twitchUsername, p_spawnDirection);
                if (ghost != null) { s_instance.m_enemyTotal++; }
                return ghost;

            case EnemyType.ShieldEnemy:
                GameObject shield = s_instance.m_shieldEnemyManager.ActivateNextEnemy(p_twitchUsername, p_spawnDirection);
                if (shield != null) { s_instance.m_enemyTotal++; }
                return shield;

            case EnemyType.DestructoidEnemy:
                GameObject destructoid = s_instance.m_destructoidEnemyManager.ActivateNextEnemy(p_twitchUsername, p_spawnDirection);
                if (destructoid != null) { s_instance.m_enemyTotal++; }
                return destructoid;

            case EnemyType.ShooterEnemy:
                GameObject shooter = s_instance.m_shooterEnemyManager.ActivateNextEnemy(p_twitchUsername, p_spawnDirection);
                if (shooter != null) { s_instance.m_enemyTotal++; }
                return shooter;

            case EnemyType.TrailEnemy:
                GameObject trail = s_instance.m_trailEnemyManager.ActivateNextEnemy(p_twitchUsername, p_spawnDirection);
                if (trail != null) { s_instance.m_enemyTotal++; }
                return trail;

            case EnemyType.HealthEnemy:
                GameObject health = s_instance.m_healthEnemyManager.ActivateNextEnemy(p_twitchUsername, p_spawnDirection);
                if(health != null) { s_instance.m_enemyTotal++; }
                return health;

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
        GameObject enemy = null;

        bool success = false;
        switch (p_enemyType)
        {
            case EnemyType.BooEnemy:
                enemy = s_instance.m_booEnemyManager.GetActiveEnemyGameObject(p_enemyIndex);
                success = s_instance.m_booEnemyManager.DeactivateEnemy(p_enemyIndex);
                if (success)
                {
                    s_instance.m_enemyTotal--;
                    s_instance.m_enemyDeathCount++;

				Scoreboard.s_points += (int)(100 * SpeedMultiplier);
                }
                break;

            case EnemyType.SeekEnemy:
                enemy = s_instance.m_seekEnemyManager.GetActiveEnemyGameObject(p_enemyIndex);
                success = s_instance.m_seekEnemyManager.DeactivateEnemy(p_enemyIndex);
                if (success)
                {
                    s_instance.m_enemyTotal--;
                    s_instance.m_enemyDeathCount++;

				Scoreboard.s_points += (int)(100 * SpeedMultiplier);
                }
                break;

            case EnemyType.GhostEnemy:
                enemy = s_instance.m_ghostEnemyManager.GetActiveEnemyGameObject(p_enemyIndex);
                success = s_instance.m_ghostEnemyManager.DeactivateEnemy(p_enemyIndex);
                if (success)
                {
                    s_instance.m_enemyTotal--;
                    s_instance.m_enemyDeathCount++;

				Scoreboard.s_points += (int)(100 * SpeedMultiplier);
                }
                break;

            case EnemyType.ShieldEnemy:
                enemy = s_instance.m_shieldEnemyManager.GetActiveEnemyGameObject(p_enemyIndex);
                success = s_instance.m_shieldEnemyManager.DeactivateEnemy(p_enemyIndex);
                if (success)
                {
                    s_instance.m_enemyTotal--;
                    s_instance.m_enemyDeathCount++;

				Scoreboard.s_points += (int)(100 * SpeedMultiplier);
                }
                break;

            case EnemyType.DestructoidEnemy:
                enemy = s_instance.m_destructoidEnemyManager.GetActiveEnemyGameObject(p_enemyIndex);
                success = s_instance.m_destructoidEnemyManager.DeactivateEnemy(p_enemyIndex);
                if (success)
                {
                    s_instance.m_enemyTotal--;
                    s_instance.m_enemyDeathCount++;

				Scoreboard.s_points += (int)(100 * SpeedMultiplier);
                }
                break;

            case EnemyType.ShooterEnemy:
                enemy = s_instance.m_shooterEnemyManager.GetActiveEnemyGameObject(p_enemyIndex);
                success = s_instance.m_shooterEnemyManager.DeactivateEnemy(p_enemyIndex);
                if (success)
                {
                    s_instance.m_enemyTotal--;
                    s_instance.m_enemyDeathCount++;
				Scoreboard.s_points += (int)(100 * SpeedMultiplier);
                }
                break;

            case EnemyType.TrailEnemy:
                enemy = s_instance.m_trailEnemyManager.GetActiveEnemyGameObject(p_enemyIndex);
                success = s_instance.m_trailEnemyManager.DeactivateEnemy(p_enemyIndex);
                if (success)
                {
                    s_instance.m_enemyTotal--;
                    s_instance.m_enemyDeathCount++;
				Scoreboard.s_points += (int)(100 * SpeedMultiplier);
                }
                break;
            case EnemyType.HealthEnemy:
                enemy = s_instance.m_healthEnemyManager.GetActiveEnemyGameObject(p_enemyIndex);
                success = s_instance.m_healthEnemyManager.DeactivateEnemy(p_enemyIndex);
                if (success)
                {
                    s_instance.m_enemyTotal--;
                    s_instance.m_enemyDeathCount++;
                }
                break;

            default:
                Debug.Log("Invalid enemy type to destroy: " + p_enemyType);
                return false;
        }

        if (s_instance.m_enemyDeathCount % 10 == 0)
        {
            Tool_WeaponSpawner.s_instance.SpawnWeapon(enemy.transform.position);
        }

        return success;
    }

    /// <summary>
    /// Returns an enemy of a given type with a given index in the array. The index can be retreived from the enemy's EnemyData script.
    /// </summary>
    public static GameObject GetEnemyGameObject(EnemyType p_enemyType, int p_index)
    {
        switch (p_enemyType)
        {
            case EnemyType.BooEnemy:
                return s_instance.m_booEnemyManager.GetActiveEnemyGameObject(p_index);

            case EnemyType.SeekEnemy:
                return s_instance.m_seekEnemyManager.GetActiveEnemyGameObject(p_index);

            case EnemyType.GhostEnemy:
                return s_instance.m_ghostEnemyManager.GetActiveEnemyGameObject(p_index);

            case EnemyType.ShieldEnemy:
                return s_instance.m_shieldEnemyManager.GetActiveEnemyGameObject(p_index);

            case EnemyType.DestructoidEnemy:
                return s_instance.m_destructoidEnemyManager.GetActiveEnemyGameObject(p_index);

            case EnemyType.ShooterEnemy:
                return s_instance.m_shooterEnemyManager.GetActiveEnemyGameObject(p_index);

            case EnemyType.TrailEnemy:
                return s_instance.m_trailEnemyManager.GetActiveEnemyGameObject(p_index);

            case EnemyType.HealthEnemy:
                return s_instance.m_healthEnemyManager.GetActiveEnemyGameObject(p_index);

            default:
                return null;
        }
    }

    /// <summary>
    /// Returns an array of all enemies of a given type. Also provides the first inactive index through an out parameter.
    /// To loop through all active enemies of that type in the scene, use the first inactive index variable as the upper limit of the loop.
    /// All enemies before that index are guaranteed to be active, and all enemies at and after that index are guaranteed to be inactive.
    /// </summary>
    public static GameObject[] GetAllEnemyGameObjects(EnemyType p_enemyType, out int p_firstInactiveIndex)
    {
        switch (p_enemyType)
        {
            case EnemyType.BooEnemy:
                return s_instance.m_booEnemyManager.GetAllEnemyGameObjects(out p_firstInactiveIndex);

            case EnemyType.SeekEnemy:
                return s_instance.m_seekEnemyManager.GetAllEnemyGameObjects(out p_firstInactiveIndex);

            case EnemyType.GhostEnemy:
                return s_instance.m_ghostEnemyManager.GetAllEnemyGameObjects(out p_firstInactiveIndex);

            case EnemyType.ShieldEnemy:
                return s_instance.m_shieldEnemyManager.GetAllEnemyGameObjects(out p_firstInactiveIndex);

            case EnemyType.DestructoidEnemy:
                return s_instance.m_destructoidEnemyManager.GetAllEnemyGameObjects(out p_firstInactiveIndex);

            case EnemyType.ShooterEnemy:
                return s_instance.m_shooterEnemyManager.GetAllEnemyGameObjects(out p_firstInactiveIndex);

            case EnemyType.TrailEnemy:
                return s_instance.m_trailEnemyManager.GetAllEnemyGameObjects(out p_firstInactiveIndex);

            case EnemyType.HealthEnemy:
                return s_instance.m_healthEnemyManager.GetAllEnemyGameObjects(out p_firstInactiveIndex);

            default:
                p_firstInactiveIndex = -1;
                return null;
        }
    }

    /// <summary>
    /// Returns an enemy's AI scipt of a given type with a given index in the array.
    /// The type returned will be an AIBase, so it will need to be casted to the expected type.
    /// </summary>
    public static AIBase GetEnemyAI(EnemyType p_enemyType, int p_index)
    {
        switch (p_enemyType)
        {
            case EnemyType.BooEnemy:
                return s_instance.m_booEnemyManager.GetActiveEnemyAI(p_index);

            case EnemyType.SeekEnemy:
                return s_instance.m_seekEnemyManager.GetActiveEnemyAI(p_index);

            case EnemyType.GhostEnemy:
                return s_instance.m_ghostEnemyManager.GetActiveEnemyAI(p_index);

            case EnemyType.ShieldEnemy:
                return s_instance.m_shieldEnemyManager.GetActiveEnemySeekAI(p_index);

            case EnemyType.DestructoidEnemy:
                return s_instance.m_destructoidEnemyManager.GetActiveEnemyAI(p_index);

            case EnemyType.ShooterEnemy:
                return s_instance.m_shooterEnemyManager.GetActiveEnemyAI(p_index);

            case EnemyType.TrailEnemy:
                return s_instance.m_trailEnemyManager.GetActiveEnemyAI(p_index);

            case EnemyType.HealthEnemy:
                return s_instance.m_healthEnemyManager.GetActiveEnemyAI(p_index);

            default:
                return null;
        }
    }

    /// <summary>
    /// Returns an array of all enemy AI scripts of a given type.
    /// The type returned will be an AIBase, so it will need to be casted to the expected type.
    /// Provides the first inactive index through an out parameter.
    /// To loop through all active enemies of that type in the scene, use the first inactive index variable as the upper limit of the loop.
    /// All enemies before that index are guaranteed to be active, and all enemies at and after that index are guaranteed to be inactive.
    /// </summary>
    public static AIBase[] GetAllEnemyAI(EnemyType p_enemyType, out int p_firstInactiveIndex)
    {
        switch (p_enemyType)
        {
            case EnemyType.BooEnemy:
                return s_instance.m_booEnemyManager.GetAllEnemyAI(out p_firstInactiveIndex);

            case EnemyType.SeekEnemy:
                return s_instance.m_seekEnemyManager.GetAllEnemyAI(out p_firstInactiveIndex);

            case EnemyType.GhostEnemy:
                return s_instance.m_ghostEnemyManager.GetAllEnemyAI(out p_firstInactiveIndex);

            case EnemyType.ShieldEnemy:
                return s_instance.m_shieldEnemyManager.GetAllEnemyAI(out p_firstInactiveIndex);

            case EnemyType.DestructoidEnemy:
                return s_instance.m_destructoidEnemyManager.GetAllEnemyAI(out p_firstInactiveIndex);

            case EnemyType.ShooterEnemy:
                return s_instance.m_shooterEnemyManager.GetAllEnemyAI(out p_firstInactiveIndex);

            case EnemyType.TrailEnemy:
                return s_instance.m_trailEnemyManager.GetAllEnemyAI(out p_firstInactiveIndex);


            case EnemyType.HealthEnemy:
                return s_instance.m_healthEnemyManager.GetAllEnemyAI(out p_firstInactiveIndex);

            default:
                p_firstInactiveIndex = -1;
                return null;
        }
    }

    /// <summary>
    /// Adds an enemy to the queue to be spawned.
    /// </summary>
    public static void AddEnemyToQueue(EnemyNetworkInfo enemyInfo)
    {
        s_instance.m_enemyQueueMut.WaitOne();
        s_instance.m_enemyQueue.Enqueue(enemyInfo);
        s_instance.m_enemyQueueMut.ReleaseMutex();
    }

    private void Update()
    {
        //-----DEBUG ONLY
		if (Input.GetKey(KeyCode.O) && Input.GetKey(KeyCode.RightShift))
        {
            EnemyNetworkInfo info = new EnemyNetworkInfo();

            info.name = "lunalovecraft";
            info.type = EnemyType.BooEnemy;
            info.direction = Direction.Random;

            AddEnemyToQueue(info);
        }

		if (Input.GetKey(KeyCode.I) && Input.GetKey(KeyCode.RightShift))
        {
            EnemyNetworkInfo info = new EnemyNetworkInfo();

            info.name = "lunalovecraft";
            info.type = EnemyType.HealthEnemy;
            info.direction = Direction.Random;

            AddEnemyToQueue(info);
        }

		if (Input.GetKey(KeyCode.P) && Input.GetKey(KeyCode.RightShift))
        {
            EnemyNetworkInfo info = new EnemyNetworkInfo();

            info.name = "lunalovecraft";
            info.type = EnemyType.GhostEnemy;
            info.direction = Direction.Random;

            AddEnemyToQueue(info);
        }

		if (Input.GetKey(KeyCode.LeftBracket) && Input.GetKey(KeyCode.RightShift))
        {
            EnemyNetworkInfo info = new EnemyNetworkInfo();

            info.name = "lunalovecraft";
            info.type = EnemyType.SeekEnemy;
            info.direction = Direction.Random;

            AddEnemyToQueue(info);
        }

		if (Input.GetKey(KeyCode.RightBracket) && Input.GetKey(KeyCode.RightShift))
        {
            EnemyNetworkInfo info = new EnemyNetworkInfo();

            info.name = "lunalovecraft";
            info.type = EnemyType.ShieldEnemy;
            info.direction = Direction.Random;

            AddEnemyToQueue(info);
        }

		if (Input.GetKey(KeyCode.L) && Input.GetKey(KeyCode.RightShift))
        {
            EnemyNetworkInfo info = new EnemyNetworkInfo();

            info.name = "lunalovecraft";
            info.type = EnemyType.TrailEnemy;
            info.direction = Direction.Random;

            AddEnemyToQueue(info);
        }
        //-----DEBUG ONLY

        m_speedTimer -= Time.deltaTime;
        if (m_speedTimer < 0.0f)
        {
            m_speedTimer += 10.0f;
            m_speedMultiplier *= 1.0116f;
            Debug.Log(m_speedMultiplier);
        }

        while (s_instance.m_enemyQueue.Count > 0)
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
