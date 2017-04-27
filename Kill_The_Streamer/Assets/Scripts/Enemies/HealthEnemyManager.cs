using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthEnemyManager : EnemyManagerTemplate {

    private AiHealth[] m_healthComponents;

    // Called at the beginning of the game by the EnemyManager
    public override void Init(Transform p_parent)
    {
        base.Init(p_parent);

        // Initializes component array
        m_healthComponents = new AiHealth[Constants.MAX_ENEMIES];

        for (int i = 0; i < Constants.MAX_ENEMIES; i++)
        {
            m_healthComponents[i] = m_enemyGameObjects[i].GetComponent<AiHealth>();
        }
    }

    // Called by the enemy manager when deactivating an enemy
    public override bool DeactivateEnemy(int p_enemyIndex)
    {
        // Fails if the enemy index is invalid or if there are no active enemies 
        if (!base.DeactivateEnemy(p_enemyIndex))
        {
            return false;
        }

        AiHealth tempSeek = m_healthComponents[p_enemyIndex];
        m_healthComponents[p_enemyIndex] = m_healthComponents[m_firstInactiveIndex - 1];
        m_healthComponents[m_firstInactiveIndex - 1] = tempSeek;
        m_healthComponents[p_enemyIndex].Index = p_enemyIndex;
        m_healthComponents[m_firstInactiveIndex - 1].Index = m_firstInactiveIndex - 1;

        // Decrements the first inactive index
        m_firstInactiveIndex--;

        return true;
    }

    public AiHealth GetActiveEnemyAI(int p_index)
    {
        if (p_index < 0 || p_index >= m_firstInactiveIndex)
        {
            Debug.Log("Invalid index " + p_index + " in HealthEnemy array");
            return null;
        }

        return m_healthComponents[p_index];
    }

    public AiHealth[] GetAllEnemyAI(out int p_firstInactiveIndex)
    {
        p_firstInactiveIndex = m_firstInactiveIndex;
        return m_healthComponents;
    }
}
