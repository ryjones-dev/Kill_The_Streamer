using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BooEnemyManager : MonoBehaviour
{
    public GameObject m_booPrefab;

    private GameObject[] m_booGameObjects;
    private EnemyData[] m_booEnemyData;
    private AiSeekFlee[] m_booSeekFleeComponents;
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
        m_booGameObjects = new GameObject[Constants.MAX_ENEMIES];
        m_booEnemyData = new EnemyData[Constants.MAX_ENEMIES];
        m_booSeekFleeComponents = new AiSeekFlee[Constants.MAX_ENEMIES];

        for (int i = 0; i < Constants.MAX_ENEMIES; i++)
        {
            // Instantiates each enemy
            GameObject boo = Instantiate<GameObject>(m_booPrefab, Vector3.zero, Quaternion.identity, p_parent);
            EnemyData booData = boo.GetComponent<EnemyData>();
            AiSeekFlee booAISeekFlee = boo.GetComponent<AiSeekFlee>();

            // Sets the enemy's name and turns it off
            boo.name = m_booPrefab.name + " " + i;
            boo.SetActive(false);

            // Saves the gameobject and components in the arrays
            m_booGameObjects[i] = boo;
            m_booEnemyData[i] = booData;
            m_booSeekFleeComponents[i] = booAISeekFlee;
        }
    }

    // Called by the enemy manager when activating an enemy. Returns true if successful or false otherwise.
    public GameObject ActivateNextEnemy(string p_twitchUsername, Direction p_spawnDirection)
    {
        // Prevents adding an enemy if there is no more room in the array
        if (m_firstInactiveIndex == Constants.MAX_ENEMIES) return null;

        // Gets first inactive enemy gameobject
        GameObject boo = m_booGameObjects[m_firstInactiveIndex];

        // Assigns the enemy's array index in the enemy data script
        m_booEnemyData[m_firstInactiveIndex].m_Index = m_firstInactiveIndex;

        // Sets the enemy's name to the twich username
        boo.name = p_twitchUsername;
        boo.GetComponentInChildren<Text>().text = p_twitchUsername;

        // Converts the spawn direction to a spawnpoint index
        int spawnIndex = (int)p_spawnDirection;
        if(spawnIndex >= m_spawnLocations.Length) { spawnIndex = Random.Range(0, m_spawnLocations.Length); }

        // Sets the position of the enemy
        boo.transform.position = m_spawnLocations[spawnIndex].transform.position;

        // Enables the gameobject
        boo.SetActive(true);

        // Increments the first inactive index
        m_firstInactiveIndex++;

        // Returns the enemy gameobject
        return boo;
    }

    // Called by the enemy manager when deactivating an enemy. Returns true
    public bool DeactivateEnemy(int p_enemyIndex)
    {
        // Fails if the enemy index is invalid or if there are no active enemies 
        if (p_enemyIndex < 0 || p_enemyIndex >= m_firstInactiveIndex || m_firstInactiveIndex == 0) return false;

        // Temporarily saves the data from the enemy we are deactivating
        GameObject temp = m_booGameObjects[p_enemyIndex];
        EnemyData tempEnemyData = m_booEnemyData[p_enemyIndex];
        AiSeekFlee tempAISeekFlee = m_booSeekFleeComponents[p_enemyIndex];

        // Deactivates the enemy
        temp.SetActive(false);

        // Moves the enemy at the end of the active section to the deactivated position
        m_booGameObjects[p_enemyIndex] = m_booGameObjects[m_firstInactiveIndex - 1];
        m_booEnemyData[p_enemyIndex] = m_booEnemyData[m_firstInactiveIndex - 1];
        m_booSeekFleeComponents[p_enemyIndex] = m_booSeekFleeComponents[m_firstInactiveIndex - 1];

        // Moves the deactivated enemy to the start of the inactive section
        m_booGameObjects[m_firstInactiveIndex - 1] = temp;
        m_booEnemyData[m_firstInactiveIndex - 1] = tempEnemyData;
        m_booSeekFleeComponents[m_firstInactiveIndex - 1] = tempAISeekFlee;

        // Makes sure the indices in the enemy data scripts are correct
        m_booEnemyData[p_enemyIndex].m_Index = p_enemyIndex;
        m_booEnemyData[m_firstInactiveIndex - 1].m_Index = m_firstInactiveIndex - 1;

        // Decrements the first inactive index
        m_firstInactiveIndex--;

        return true;
    }

    public GameObject GetActiveEnemy(int p_index)
    {
        if(p_index < 0 || p_index >= m_firstInactiveIndex)
        {
            Debug.Log("Invalid index " + p_index + " in BooEnemy array");
            return null;
        }

        return m_booGameObjects[p_index];
    }

    public GameObject[] GetAllEnemies(out int p_firstInactiveIndex)
    {
        p_firstInactiveIndex = m_firstInactiveIndex;
        return m_booGameObjects;
    }
}
