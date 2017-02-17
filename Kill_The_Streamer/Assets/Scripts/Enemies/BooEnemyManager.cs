﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BooEnemyManager : MonoBehaviour
{
    public GameObject m_booPrefab;

    private GameObject[] m_booGameObjects;
    private EnemyData[] m_booEnemyData;
    private AiSeekFlee[] m_booSeekFleeComponents;
    private int m_firstInactiveIndex = 0;

    private static BooEnemyManager s_instance;

    private void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Called at the beginning of the game by the EnemyManager
    public static void Init(Transform p_parent)
    {
        // Initializes the gameobject and component arrays
        s_instance.m_booGameObjects = new GameObject[Constants.MAX_ENEMIES];
        s_instance.m_booEnemyData = new EnemyData[Constants.MAX_ENEMIES];
        s_instance.m_booSeekFleeComponents = new AiSeekFlee[Constants.MAX_ENEMIES];

        for (int i = 0; i < Constants.MAX_ENEMIES; i++)
        {
            // Instantiates each enemy
            GameObject boo = Instantiate<GameObject>(s_instance.m_booPrefab, Vector3.zero, Quaternion.identity, p_parent);
            EnemyData booData = boo.GetComponent<EnemyData>();
            AiSeekFlee booAISeekFlee = boo.GetComponent<AiSeekFlee>();

            // Sets the enemy's name and turns it off
            boo.name = s_instance.m_booPrefab.name + " " + i;
            boo.SetActive(false);

            // Saves the gameobject and components in the arrays
            s_instance.m_booGameObjects[i] = boo;
            s_instance.m_booEnemyData[i] = booData;
            s_instance.m_booSeekFleeComponents[i] = booAISeekFlee;
        }
    }

    // Called by the enemy manager when activating an enemy. Returns true if successful or false otherwise.
    public static GameObject ActivateNextEnemy(string p_twitchUsername, Vector3 p_position)
    {
        // Prevents adding an enemy if there is no more room in the array
        if (s_instance.m_firstInactiveIndex == Constants.MAX_ENEMIES) return null;

        // Gets first inactive enemy gameobject
        GameObject boo = s_instance.m_booGameObjects[s_instance.m_firstInactiveIndex];

        // Assigns the enemy's array index in the enemy data script
        s_instance.m_booEnemyData[s_instance.m_firstInactiveIndex].m_Index = s_instance.m_firstInactiveIndex;

        // Sets the gameobject's name to the twich username
        boo.name = p_twitchUsername;

        // Sets the position of the boo
        boo.transform.position = p_position;

        // Enables the gameobject
        boo.SetActive(true);

        // Increments the first inactive index
        s_instance.m_firstInactiveIndex++;

        // Returns the enemy gameobject
        return boo;
    }

    // Called by the enemy manager when deactivating an enemy. Returns true
    public static bool DeactivateEnemy(int p_enemyIndex)
    {
        // Fails if the enemy index is invalid or if there are no active enemies 
        if (p_enemyIndex < 0 || p_enemyIndex >= s_instance.m_firstInactiveIndex || s_instance.m_firstInactiveIndex == 0) return false;

        // Temporarily saves the data from the enemy we are deactivating
        GameObject temp = s_instance.m_booGameObjects[p_enemyIndex];
        EnemyData tempEnemyData = s_instance.m_booEnemyData[p_enemyIndex];
        AiSeekFlee tempAISeekFlee = s_instance.m_booSeekFleeComponents[p_enemyIndex];

        // Deactivates the enemy
        temp.SetActive(false);

        // Moves the enemy at the end of the active section to the deactivated position
        s_instance.m_booGameObjects[p_enemyIndex] = s_instance.m_booGameObjects[s_instance.m_firstInactiveIndex - 1];
        s_instance.m_booEnemyData[p_enemyIndex] = s_instance.m_booEnemyData[s_instance.m_firstInactiveIndex - 1];
        s_instance.m_booSeekFleeComponents[p_enemyIndex] = s_instance.m_booSeekFleeComponents[s_instance.m_firstInactiveIndex - 1];

        // Moves the deactivated enemy to the start of the inactive section
        s_instance.m_booGameObjects[s_instance.m_firstInactiveIndex - 1] = temp;
        s_instance.m_booEnemyData[s_instance.m_firstInactiveIndex - 1] = tempEnemyData;
        s_instance.m_booSeekFleeComponents[s_instance.m_firstInactiveIndex - 1] = tempAISeekFlee;

        // Makes sure the indices in the enemy data scripts are correct
        s_instance.m_booEnemyData[p_enemyIndex].m_Index = p_enemyIndex;
        s_instance.m_booEnemyData[s_instance.m_firstInactiveIndex - 1].m_Index = s_instance.m_firstInactiveIndex - 1;

        // Decrements the first inactive index
        s_instance.m_firstInactiveIndex--;

        return true;
    }
}
