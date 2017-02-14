using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BooEnemies : MonoBehaviour
{
    private GameObject[] m_booGameObjects;
    private AiSeekFlee[] m_booSeekFleeComponents;
    private int m_firstInactiveIndex = 0;

    private void Awake()
    {
        m_booGameObjects = new GameObject[EnemyManager.MAX_ENEMIES];
        m_booSeekFleeComponents = new AiSeekFlee[EnemyManager.MAX_ENEMIES];

        for (int i = 0; i < 1024; i++)
        {

        }
    }
}

public class EnemyManager : MonoBehaviour
{
    private Dictionary<string, GameObject> m_enemyPrefabMap; // A map of the different enemy types. Ex "Kappa" could be mapped to a Kappa enemy prefab.

    public const int MAX_ENEMIES = 1024; // Maximum number of enemies in the enemy array.

    private GameObject m_enemyParent;// A parent gameobject to put the enemies in for editor convenience
    private GameObject[] m_enemies; // Enemy array is sorted so that all active enemies are in front and inactive enemies are in the back.
    private int m_firstInactiveIndex = 0; // Stores the index separating the active and inactive objects. (If 0, there are no active enemies)

    // Singleton instance
    private static EnemyManager s_instance;

    private void Awake()
    {
        // Sets up the singleton
        if(s_instance == null)
        {
            s_instance = this;

            // Initializes the prefab map
            m_enemyPrefabMap = new Dictionary<string, GameObject>();

            // Load in the enemy prefabs into an array
            GameObject[] enemyTypes = Resources.LoadAll<GameObject>("Enemies");
            for (int i = 0; i < enemyTypes.Length; i++)
            {
                // Map each gameobject's name to the gameobject. The name should be the name of the command to spawn it.
                m_enemyPrefabMap.Add(enemyTypes[i].name, enemyTypes[i]);
            }

            // Preallocate the enemy array
            m_enemies = new GameObject[MAX_ENEMIES];

            // Creates the enemy parent gameobject
            m_enemyParent = new GameObject("Enemies");

            
        }
        else
        {
            // Destroys the object if there is already a singleton instance
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            for (int i = 0; i < 1024; i++)
            {
                CreateEnemy("", "ExampleKappa", Vector3.zero, Quaternion.identity, true);
            }
        }

        if(Input.GetButtonDown("Fire2"))
        {
            for (int i = 0; i < 1024; i++)
            {
                DestroyEnemy(0);
            }
        }
    }

    public static GameObject CreateEnemy(string p_twitchUsername, string p_command, Vector3 position, Quaternion rotation, bool isActive)
    {
        // Gets the appropriate prefab based on the command (prefab name should be the same as the command)
        if (!s_instance.m_enemyPrefabMap.ContainsKey(p_command) || s_instance.m_firstInactiveIndex == 1024) return null;
        GameObject enemyPrefab = s_instance.m_enemyPrefabMap[p_command];

        // Instantiates the enemy prefab
        GameObject newEnemy = Instantiate<GameObject>(enemyPrefab, position, rotation, s_instance.m_enemyParent.transform);

        // Sets it's name as the twitch user's name that spawned it
        newEnemy.name = p_twitchUsername + " " + s_instance.m_firstInactiveIndex;

        // Sets the enemy's index so it can be found in the array later
        newEnemy.GetComponent<EnemyData>().m_Index = s_instance.m_firstInactiveIndex;

        // Stores the enemy in the enemy array
        s_instance.m_enemies[s_instance.m_firstInactiveIndex] = newEnemy;

        // Increments the inactive index
        s_instance.m_firstInactiveIndex++;

        // Sets the newly spawned enemy to be active or inactive
        newEnemy.SetActive(isActive);

        return newEnemy;
    }

    public static bool DestroyEnemy(int enemyIndex)
    {
        if ((enemyIndex < 0 || enemyIndex >= s_instance.m_firstInactiveIndex) && s_instance.m_firstInactiveIndex <= 0) return false;

        // Moves the enemy to delete to a temporary variable
        GameObject temp = s_instance.m_enemies[enemyIndex];
        s_instance.m_enemies[enemyIndex] = null;

        // Deletes the old enemy
        Destroy(temp);

        // Moves the last active enemy to deleted enemy slot
        s_instance.m_enemies[enemyIndex] = s_instance.m_enemies[s_instance.m_firstInactiveIndex - 1];
        s_instance.m_enemies[s_instance.m_firstInactiveIndex - 1] = null;

        // Decrements the first inactive index by 1
        s_instance.m_firstInactiveIndex--;

        return true;
    }
}
