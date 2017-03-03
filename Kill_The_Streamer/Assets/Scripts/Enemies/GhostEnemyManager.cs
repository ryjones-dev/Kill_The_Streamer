using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostEnemyManager : MonoBehaviour
{
    public GameObject m_ghostPrefab;

    private GameObject[] m_ghostGameObjects;
    private EnemyData[] m_ghostEnemyData;
    private AiGhost[] m_ghostGhostComponents;
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
        m_ghostGameObjects = new GameObject[Constants.MAX_ENEMIES];
        m_ghostEnemyData = new EnemyData[Constants.MAX_ENEMIES];
        m_ghostGhostComponents = new AiGhost[Constants.MAX_ENEMIES];

        for (int i = 0; i < Constants.MAX_ENEMIES; i++)
        {
            // Instantiates each enemy
            GameObject ghost = Instantiate<GameObject>(m_ghostPrefab, Vector3.zero, Quaternion.identity, p_parent);
            EnemyData ghostData = ghost.GetComponent<EnemyData>();
            AiGhost ghostComponent = ghost.GetComponent<AiGhost>();

            // Sets the enemy's name and turns it off
            ghost.name = m_ghostPrefab.name + " " + i;
            ghost.SetActive(false);

            // Saves the gameobject and components in the arrays
            m_ghostGameObjects[i] = ghost;
            m_ghostEnemyData[i] = ghostData;
            m_ghostGhostComponents[i] = ghostComponent;
        }
    }

    // Called by the enemy manager when activating an enemy. Returns true if successful or false otherwise.
    public GameObject ActivateNextEnemy(string p_twitchUsername, Direction p_spawnDirection)
    {
        // Prevents adding an enemy if there is no more room in the array
        if (m_firstInactiveIndex == Constants.MAX_ENEMIES) return null;

        // Gets first inactive enemy gameobject
        GameObject ghost = m_ghostGameObjects[m_firstInactiveIndex];

        // Assigns the enemy's array index in the enemy data script
        m_ghostEnemyData[m_firstInactiveIndex].m_Index = m_firstInactiveIndex;

        // Sets the enemy's name to the twich username
        ghost.name = p_twitchUsername;
        ghost.GetComponentInChildren<TextMesh>().text = p_twitchUsername;

        // Converts the spawn direction to a spawnpoint index
        int spawnIndex = (int)p_spawnDirection;
        if (spawnIndex >= m_spawnLocations.Length) { spawnIndex = Random.Range(0, m_spawnLocations.Length); }

        // Sets the position of the enemy
        Vector3 spawnVariance = spawnIndex % 2 == 0 ? new Vector3(Random.Range(-2.0f, 2.0f), 0, 0) : new Vector3(0, 0, Random.Range(-2.0f, 2.0f));
        ghost.transform.position = m_spawnLocations[spawnIndex].transform.position + spawnVariance;

        // Enables the gameobject
        ghost.SetActive(true);

        // Increments the first inactive index
        m_firstInactiveIndex++;

        // Returns the enemy gameobject
        return ghost;
    }

    // Called by the enemy manager when deactivating an enemy. Returns true
    public bool DeactivateEnemy(int p_enemyIndex)
    {
        // Fails if the enemy index is invalid or if there are no active enemies 
        if (p_enemyIndex < 0 || p_enemyIndex >= m_firstInactiveIndex || m_firstInactiveIndex == 0) return false;

        // Temporarily saves the data from the enemy we are deactivating
        GameObject temp = m_ghostGameObjects[p_enemyIndex];
        EnemyData tempEnemyData = m_ghostEnemyData[p_enemyIndex];
        AiGhost tempAISeekFlee = m_ghostGhostComponents[p_enemyIndex];

        // Deactivates the enemy
        temp.SetActive(false);

        // Moves the enemy at the end of the active section to the deactivated position
        m_ghostGameObjects[p_enemyIndex] = m_ghostGameObjects[m_firstInactiveIndex - 1];
        m_ghostEnemyData[p_enemyIndex] = m_ghostEnemyData[m_firstInactiveIndex - 1];
        m_ghostGhostComponents[p_enemyIndex] = m_ghostGhostComponents[m_firstInactiveIndex - 1];

        // Moves the deactivated enemy to the start of the inactive section
        m_ghostGameObjects[m_firstInactiveIndex - 1] = temp;
        m_ghostEnemyData[m_firstInactiveIndex - 1] = tempEnemyData;
        m_ghostGhostComponents[m_firstInactiveIndex - 1] = tempAISeekFlee;

        // Makes sure the indices in the enemy data scripts are correct
        m_ghostEnemyData[p_enemyIndex].m_Index = p_enemyIndex;
        m_ghostEnemyData[m_firstInactiveIndex - 1].m_Index = m_firstInactiveIndex - 1;

        // Decrements the first inactive index
        m_firstInactiveIndex--;

        return true;
    }

    public GameObject GetActiveEnemy(int p_index)
    {
        if (p_index < 0 || p_index >= m_firstInactiveIndex)
        {
            Debug.Log("Invalid index " + p_index + " in GhostEnemy array");
            return null;
        }

        return m_ghostGameObjects[p_index];
    }

    public GameObject[] GetAllEnemies(out int p_firstInactiveIndex)
    {
        p_firstInactiveIndex = m_firstInactiveIndex;
        return m_ghostGameObjects;
    }
}
