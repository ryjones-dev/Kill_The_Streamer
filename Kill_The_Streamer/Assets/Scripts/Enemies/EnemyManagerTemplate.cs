using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyManagerTemplate : MonoBehaviour {

    public GameObject m_enemyPrefab;

    protected GameObject[] m_enemyGameObjects;
    protected int m_firstInactiveIndex = 0;

    protected GameObject[] m_spawnLocations;

    protected virtual void Start()
    {
        GameObject spawnpoints = GameObject.FindGameObjectWithTag("Spawnpoints");

        m_spawnLocations = new GameObject[spawnpoints.transform.childCount];
        for (int i = 0; i < m_spawnLocations.Length; i++)
        {
            m_spawnLocations[i] = spawnpoints.transform.GetChild(i).gameObject;
        }
    }

    // Called at the beginning of the game by the EnemyManager
    public virtual void Init(Transform p_parent)
    {
        // Initializes the gameobject and component arrays
        m_enemyGameObjects = new GameObject[Constants.MAX_ENEMIES];

        for (int i = 0; i < Constants.MAX_ENEMIES; i++)
        {
            // Instantiates each enemy
            GameObject enemy = Instantiate<GameObject>(m_enemyPrefab, Vector3.zero, Quaternion.identity, p_parent);

            // Sets the enemy's name and turns it off
            enemy.name = m_enemyPrefab.name + " " + i;
            enemy.SetActive(false);

			enemy.GetComponent<AIBase> ().Start ();

            // Saves the gameobject and components in the arrays
            m_enemyGameObjects[i] = enemy;
        }
    }

    // Called by the enemy manager when activating an enemy. Returns true if successful or false otherwise.
    public virtual GameObject ActivateNextEnemy(string p_twitchUsername, Direction p_spawnDirection)
    {
        // Prevents adding an enemy if there is no more room in the array
        if (m_firstInactiveIndex == Constants.MAX_ENEMIES) return null;

        // Gets first inactive enemy gameobject
        GameObject enemy = m_enemyGameObjects[m_firstInactiveIndex];

        // Assigns the enemy's array index
		AIBase enemyBase = enemy.GetComponent<AIBase>();
        enemyBase.Index = m_firstInactiveIndex;

        // Sets the enemy's name to the twich username
        enemy.name = p_twitchUsername;
        enemy.GetComponentInChildren<TextMesh>().text = p_twitchUsername;

        // Converts the spawn direction to a spawnpoint index
        int spawnIndex = (int)p_spawnDirection;
        if (spawnIndex >= m_spawnLocations.Length) { spawnIndex = Random.Range(0, m_spawnLocations.Length); }

        // Sets the position of the enemy
        Vector3 spawnVariance = spawnIndex % 2 == 0 ? new Vector3(Random.Range(-2.0f, 2.0f), 0, 0) : new Vector3(0, 0, Random.Range(-2.0f, 2.0f));
        enemy.transform.position = m_spawnLocations[spawnIndex].transform.position + spawnVariance;


        // Enables the gameobject
        enemy.SetActive(true);
		enemyBase.Initialize();
		enemy.transform.position = new Vector3 (enemy.transform.position.x, 0, enemy.transform.position.z);

        // Increments the first inactive index
        m_firstInactiveIndex++;

        // Returns the enemy gameobject
        return enemy;
    }

    // Called by the enemy manager when deactivating an enemy. Returns true
    public virtual bool DeactivateEnemy(int p_enemyIndex)
    {
        // Fails if the enemy index is invalid or if there are no active enemies 
        if (p_enemyIndex < 0 || p_enemyIndex >= m_firstInactiveIndex || m_firstInactiveIndex == 0) return false;

        // Temporarily saves the data from the enemy we are deactivating
        GameObject temp = m_enemyGameObjects[p_enemyIndex];

        // Deactivates the enemy
        temp.SetActive(false);

        // Moves the enemy at the end of the active section to the deactivated position
        m_enemyGameObjects[p_enemyIndex] = m_enemyGameObjects[m_firstInactiveIndex - 1];

        // Moves the deactivated enemy to the start of the inactive section
        m_enemyGameObjects[m_firstInactiveIndex - 1] = temp;
        return true;
    }

    public virtual GameObject GetActiveEnemyGameObject(int p_index)
    {
        if (p_index < 0 || p_index >= m_firstInactiveIndex)
        {
            Debug.Log("Invalid index " + p_index + " in enemy gameobject array");
            return null;
        }

        return m_enemyGameObjects[p_index];
    }

    public virtual GameObject[] GetAllEnemyGameObjects(out int p_firstInactiveIndex)
    {
        p_firstInactiveIndex = m_firstInactiveIndex;
        return m_enemyGameObjects;
    }
}
