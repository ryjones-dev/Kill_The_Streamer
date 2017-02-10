using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private Dictionary<string, GameObject> m_enemyPrefabMap; // A map of the different enemy types. Ex "Kappa" could be mapped to a Kappa enemy prefab.

    private const int MAX_ENEMIES = 1024; // Maximum number of enemies in the enemy array.

    private GameObject enemyParent;// A parent gameobject to put the enemies in for editor convenience
    private GameObject[] m_enemies; // Enemy array is sorted so that all active enemies are in front and inactive enemies are in the back.
    private short m_firstInactiveIndex = 0; // Stores the index separating the active and inactive objects. (If 0, there are no active enemies)

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
            enemyParent = new GameObject("Enemies");
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
            SpawnEnemy("Twitch Username", "ExampleKappa");
            Debug.Log(enemyParent.transform.childCount);
        }
        else if(Input.GetButtonDown("Fire2"))
        {
            Debug.Log(DestroyEnemy(0) + ", " + enemyParent.transform.childCount);
        }
    }

    public GameObject SpawnEnemy(string p_twitchUsername, string p_command)
    {
        // Gets the appropriate prefab based on the command (prefab name should be the same as the command)
        if (!m_enemyPrefabMap.ContainsKey(p_command)) return null;
        GameObject enemyPrefab = m_enemyPrefabMap[p_command];

        // Gets the enemy slot to activate
        GameObject newEnemy = m_enemies[m_firstInactiveIndex];

        // Copies the prefab data to the enemy slot
        newEnemy = Instantiate<GameObject>(enemyPrefab);

        // Sets it's name as the twitch user's name that spawned it
        newEnemy.name = p_twitchUsername;

        // Sets it's parent for editor convenience
        newEnemy.transform.SetParent(enemyParent.transform);

        // Sets the enemy's index so it can be found in the array later
        newEnemy.GetComponent<EnemyData>().m_Index = m_firstInactiveIndex;

        // Increments the inactive index
        m_firstInactiveIndex++;

        return newEnemy;
    }

    public bool DestroyEnemy(short enemyIndex)
    {
        /* Process of destroying an enemy:
         * 1. Delete enemy data 
         * 2. Copy inactive index - 1 to enemy position
         * 3. Decrement inactive index
         */

        if (enemyIndex < 0 || enemyIndex >= m_firstInactiveIndex) return false;


        GameObject temp = m_enemies[m_firstInactiveIndex - 1];
        m_enemies[enemyIndex] = m_enemies[m_firstInactiveIndex - 1];
        m_enemies[m_firstInactiveIndex - 1] = temp;
        m_firstInactiveIndex--;
        return true;
    }
}
