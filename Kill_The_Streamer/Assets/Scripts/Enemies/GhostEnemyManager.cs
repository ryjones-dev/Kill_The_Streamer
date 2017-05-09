using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEnemyManager : EnemyManagerTemplate
{
    private AiGhost[] m_ghostComponents;

    // Called at the beginning of the game by the EnemyManager
    public override void Init(Transform p_parent)
    {
        base.Init(p_parent);

        // Initializes component array
        m_ghostComponents = new AiGhost[Settings.maxEnemies];

        for (int i = 0; i < Settings.maxEnemies; i++)
        {
            m_ghostComponents[i] = m_enemyGameObjects[i].GetComponent<AiGhost>();
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

        AiGhost tempGhost = m_ghostComponents[p_enemyIndex];
        m_ghostComponents[p_enemyIndex] = m_ghostComponents[m_firstInactiveIndex - 1];
        m_ghostComponents[m_firstInactiveIndex - 1] = tempGhost;
        m_ghostComponents[p_enemyIndex].Index = p_enemyIndex;
        m_ghostComponents[m_firstInactiveIndex - 1].Index = m_firstInactiveIndex - 1;

        // Decrements the first inactive index
        m_firstInactiveIndex--;

        return true;
    }

    public AiGhost GetActiveEnemyAI(int p_index)
    {
        if (p_index < 0 || p_index >= m_firstInactiveIndex)
        {
            Debug.Log("Invalid index " + p_index + " in GhostEnemy array");
            return null;
        }

        return m_ghostComponents[p_index];
    }

    public AiGhost[] GetAllEnemyAI(out int p_firstInactiveIndex)
    {
        p_firstInactiveIndex = m_firstInactiveIndex;
        return m_ghostComponents;
    }
}
