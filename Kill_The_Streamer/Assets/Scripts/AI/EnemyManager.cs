using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    public int m_damageDealt;
    public int m_health;
}

public class EnemyManager : MonoBehaviour
{
    private Dictionary<string, GameObject> m_enemyPrefabs; // A map of the different enemy types. Ex "Kappa" could be mapped to a Kappa enemy prefab.

    private const int MAX_ENEMIES = 1024; // Maximum number of enemies in the enemy array.

    private GameObject[] m_enemies; // Enemy array is sorted so that all active enemies are in front and inactive enemies are in the back.
    private short m_firstInactiveIndex = 0; // Stores the index separating the active and inactive objects. (If 0, there are no active enemies)

    // Singleton instance
    private static EnemyManager s_instance;

    private void Awake()
    {
        // Sets up the singleton
        if(s_instance != null)
        {
            s_instance = this;

            // Load in the enemy prefabs into an array
            GameObject[] enemyTypes = Resources.LoadAll<GameObject>("Enemies");
            for (int i = 0; i < enemyTypes.Length; i++)
            {
                // Map each gameobject's name to the gameobject. The name should be the name of the command to spawn it.
                m_enemyPrefabs.Add(enemyTypes[i].name, enemyTypes[i]);
            }

            // Preallocate the enemy array
            m_enemies = new GameObject[MAX_ENEMIES];

            for (int i = 0; i < m_enemies.Length; i++)
            {
                m_enemies[i] = new GameObject();
            }
        }
        else
        {
            // Destroys the object if there is already a singleton instance
            Destroy(gameObject);
        }
    }

    public void SpawnEnemy(string p_enemyType, string p_twitchUsername, string p_command)
    {
        /*
           Order of enemy spawning:
           1. Copy enemy data to first inactive enemy
           2. Active Unity components
           3. Update inactive index 
       */

        // Gets the new object and sets it's name as the twitch user's name that spawned it
        GameObject newEnemy = m_enemies[m_firstInactiveIndex];
        newEnemy.name = p_twitchUsername;

        // Gets the enemy's data component
        EnemyData newEnemyData = newEnemy.GetComponent<EnemyData>();

        // Switches through each command and assigns the appropriate data based on the map of enemy prefabs.
        switch (p_command)
        {
            case "ExampleKappa":
                GameObject kappaPrefab = m_enemyPrefabs["ExampleKappa"];
                EnemyData kappaData = kappaPrefab.GetComponent<EnemyData>();
                newEnemyData = kappaData;
                break;
            default:
                break;
        }
    }

    private GameObject CopyEnemy(GameObject p_newEnemy, string p_command)
    {
        return null;
    }

    public void DestroyEnemy(GameObject p_enemy)
    {

    }
}
