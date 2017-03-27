using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemyManager : MonoBehaviour
{
    public GameObject m_shieldPrefab;

    private GameObject[] m_shieldEnemyGameObjects;
    private AiShieldSeek[] m_shieldSeekComponents;
    private int m_firstInactiveIndex = 0;

    private GameObject[] m_spawnLocations;

    private void Start()
    {
        GameObject spawnpoints = GameObject.FindGameObjectWithTag("Spawnpoints");

        m_spawnLocations = new GameObject[spawnpoints.transform.childCount];
        for (int i = 0; i < m_spawnLocations.Length; i++)
        {
            m_spawnLocations[i] = spawnpoints.transform.GetChild(i).gameObject;
        }
    }

    // Called at the beginning of the game by the EnemyManager
    public void Init(Transform p_parent)
    {
        // Initializes the gameobject and component arrays
        m_shieldEnemyGameObjects = new GameObject[Constants.MAX_ENEMIES];
        m_shieldSeekComponents = new AiShieldSeek[Constants.MAX_ENEMIES];

        for (int i = 0; i < Constants.MAX_ENEMIES; i++)
        {
            // Instantiates each enemy
            GameObject shield = Instantiate<GameObject>(m_shieldPrefab, Vector3.zero, Quaternion.identity, p_parent);
            AiShieldSeek shieldSeekComponent = shield.GetComponent<AiShieldSeek>();

            // Sets the enemy's name and turns it off
            shield.name = m_shieldPrefab.name + " " + i;
            shield.SetActive(false);

            // Saves the gameobject and components in the arrays
            m_shieldEnemyGameObjects[i] = shield;
            m_shieldSeekComponents[i] = shieldSeekComponent;
        }
    }

    // Called by the enemy manager when activating an enemy. Returns true if successful or false otherwise.
    public GameObject ActivateNextEnemy(string p_twitchUsername, Direction p_spawnDirection)
    {
        // Prevents adding an enemy if there is no more room in the array
        if (m_firstInactiveIndex == Constants.MAX_ENEMIES) return null;

        // Gets first inactive enemy gameobject
        GameObject shield = m_shieldEnemyGameObjects[m_firstInactiveIndex];

        // Assigns the enemy's array index
        m_shieldSeekComponents[m_firstInactiveIndex].Index = m_firstInactiveIndex;

        // Sets the enemy's name to the twich username
        shield.name = p_twitchUsername;
        shield.GetComponentInChildren<TextMesh>().text = p_twitchUsername;

        // Converts the spawn direction to a spawnpoint index
        int spawnIndex = (int)p_spawnDirection;
        if (spawnIndex >= m_spawnLocations.Length) { spawnIndex = Random.Range(0, m_spawnLocations.Length); }

        // Sets the position of the enemy
        Vector3 spawnVariance = spawnIndex % 2 == 0 ? new Vector3(Random.Range(-2.0f, 2.0f), 0, 0) : new Vector3(0, 0, Random.Range(-2.0f, 2.0f));
        shield.transform.position = m_spawnLocations[spawnIndex].transform.position + spawnVariance;

        // Enables the gameobject
        shield.SetActive(true);

        // Increments the first inactive index
        m_firstInactiveIndex++;

        // Returns the enemy gameobject
        return shield;
    }

    // Called by the enemy manager when deactivating an enemy. Returns true
    public bool DeactivateEnemy(int p_enemyIndex)
    {
        // Fails if the enemy index is invalid or if there are no active enemies 
        if (p_enemyIndex < 0 || p_enemyIndex >= m_firstInactiveIndex || m_firstInactiveIndex == 0) return false;

        // Temporarily saves the data from the enemy we are deactivating
        GameObject temp = m_shieldEnemyGameObjects[p_enemyIndex];
        AiShieldSeek tempShieldSeek = m_shieldSeekComponents[p_enemyIndex];

        // Deactivates the enemy
        temp.SetActive(false);

        // Moves the enemy at the end of the active section to the deactivated position
        m_shieldEnemyGameObjects[p_enemyIndex] = m_shieldEnemyGameObjects[m_firstInactiveIndex - 1];
        m_shieldSeekComponents[p_enemyIndex] = m_shieldSeekComponents[m_firstInactiveIndex - 1];

        // Moves the deactivated enemy to the start of the inactive section
        m_shieldEnemyGameObjects[m_firstInactiveIndex - 1] = temp;
        m_shieldSeekComponents[m_firstInactiveIndex - 1] = tempShieldSeek;

        // Makes sure the indices are correct
        m_shieldSeekComponents[p_enemyIndex].Index = p_enemyIndex;
        m_shieldSeekComponents[m_firstInactiveIndex - 1].Index = m_firstInactiveIndex - 1;

        // Decrements the first inactive index
        m_firstInactiveIndex--;

        return true;
    }

    public GameObject GetActiveEnemyGameObject(int p_index)
    {
        if (p_index < 0 || p_index >= m_firstInactiveIndex)
        {
            Debug.Log("Invalid index " + p_index + " in ShieldEnemy array");
            return null;
        }

        return m_shieldEnemyGameObjects[p_index];
    }

    public GameObject[] GetAllEnemyGameObjects(out int p_firstInactiveIndex)
    {
        p_firstInactiveIndex = m_firstInactiveIndex;
        return m_shieldEnemyGameObjects;
    }

    public AiShieldSeek GetActiveEnemySeekAI(int p_index)
    {
        if (p_index < 0 || p_index >= m_firstInactiveIndex)
        {
            Debug.Log("Invalid index " + p_index + " in ShieldEnemy array");
            return null;
        }

        return m_shieldSeekComponents[p_index];
    }

    public AiShieldSeek[] GetAllEnemySeekAI(out int p_firstInactiveIndex)
    {
        p_firstInactiveIndex = m_firstInactiveIndex;
        return m_shieldSeekComponents;
    }
}
