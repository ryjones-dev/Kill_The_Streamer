using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeekEnemyManager : MonoBehaviour
{
    public GameObject m_seekPrefab;

    private GameObject[] m_seekEnemyGameObjects;
    private EnemyData[] m_seekEnemyData;
    private AiSeeking[] m_seekSeekComponents;
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
        m_seekEnemyGameObjects = new GameObject[Constants.MAX_ENEMIES];
        m_seekEnemyData = new EnemyData[Constants.MAX_ENEMIES];
        m_seekSeekComponents = new AiSeeking[Constants.MAX_ENEMIES];

        for (int i = 0; i < Constants.MAX_ENEMIES; i++)
        {
            // Instantiates each enemy
            GameObject seek = Instantiate<GameObject>(m_seekPrefab, Vector3.zero, Quaternion.identity, p_parent);
            EnemyData seekData = seek.GetComponent<EnemyData>();
            AiSeeking seekComponent = seek.GetComponent<AiSeeking>();

            // Sets the enemy's name and turns it off
            seek.name = m_seekPrefab.name + " " + i;
            seek.SetActive(false);

            // Saves the gameobject and components in the arrays
            m_seekEnemyGameObjects[i] = seek;
            m_seekEnemyData[i] = seekData;
            m_seekSeekComponents[i] = seekComponent;
        }
    }

    // Called by the enemy manager when activating an enemy. Returns true if successful or false otherwise.
    public GameObject ActivateNextEnemy(string p_twitchUsername, Direction p_spawnDirection)
    {
        // Prevents adding an enemy if there is no more room in the array
        if (m_firstInactiveIndex == Constants.MAX_ENEMIES) return null;

        // Gets first inactive enemy gameobject
        GameObject seek = m_seekEnemyGameObjects[m_firstInactiveIndex];

        // Assigns the enemy's array index in the enemy data script
        m_seekEnemyData[m_firstInactiveIndex].m_Index = m_firstInactiveIndex;

        // Sets the enemy's name to the twich username
        seek.name = p_twitchUsername;
        seek.GetComponentInChildren<TextMesh>().text = p_twitchUsername;

        // Converts the spawn direction to a spawnpoint index
        int spawnIndex = (int)p_spawnDirection;
        if (spawnIndex >= m_spawnLocations.Length) { spawnIndex = Random.Range(0, m_spawnLocations.Length); }

        // Sets the position of the enemy
        Vector3 spawnVariance = spawnIndex % 2 == 0 ? new Vector3(Random.Range(-2.0f, 2.0f), 0, 0) : new Vector3(0, 0, Random.Range(-2.0f, 2.0f));
        seek.transform.position = m_spawnLocations[spawnIndex].transform.position + spawnVariance;

        // Enables the gameobject
        seek.SetActive(true);

        // Increments the first inactive index
        m_firstInactiveIndex++;

        // Returns the enemy gameobject
        return seek;
    }

    // Called by the enemy manager when deactivating an enemy. Returns true
    public bool DeactivateEnemy(int p_enemyIndex)
    {
        // Fails if the enemy index is invalid or if there are no active enemies 
        if (p_enemyIndex < 0 || p_enemyIndex >= m_firstInactiveIndex || m_firstInactiveIndex == 0) return false;

        // Temporarily saves the data from the enemy we are deactivating
        GameObject temp = m_seekEnemyGameObjects[p_enemyIndex];
        EnemyData tempEnemyData = m_seekEnemyData[p_enemyIndex];
        AiSeeking tempAISeekFlee = m_seekSeekComponents[p_enemyIndex];

        // Deactivates the enemy
        temp.SetActive(false);

        // Moves the enemy at the end of the active section to the deactivated position
        m_seekEnemyGameObjects[p_enemyIndex] = m_seekEnemyGameObjects[m_firstInactiveIndex - 1];
        m_seekEnemyData[p_enemyIndex] = m_seekEnemyData[m_firstInactiveIndex - 1];
        m_seekSeekComponents[p_enemyIndex] = m_seekSeekComponents[m_firstInactiveIndex - 1];

        // Moves the deactivated enemy to the start of the inactive section
        m_seekEnemyGameObjects[m_firstInactiveIndex - 1] = temp;
        m_seekEnemyData[m_firstInactiveIndex - 1] = tempEnemyData;
        m_seekSeekComponents[m_firstInactiveIndex - 1] = tempAISeekFlee;

        // Makes sure the indices in the enemy data scripts are correct
        m_seekEnemyData[p_enemyIndex].m_Index = p_enemyIndex;
        m_seekEnemyData[m_firstInactiveIndex - 1].m_Index = m_firstInactiveIndex - 1;

        // Decrements the first inactive index
        m_firstInactiveIndex--;

        return true;
    }

    public GameObject GetActiveEnemy(int p_index)
    {
        if (p_index < 0 || p_index >= m_firstInactiveIndex)
        {
            Debug.Log("Invalid index " + p_index + " in SeekEnemy array");
            return null;
        }

        return m_seekEnemyGameObjects[p_index];
    }

    public GameObject[] GetAllEnemies(out int p_firstInactiveIndex)
    {
        p_firstInactiveIndex = m_firstInactiveIndex;
        return m_seekEnemyGameObjects;
    }
}
