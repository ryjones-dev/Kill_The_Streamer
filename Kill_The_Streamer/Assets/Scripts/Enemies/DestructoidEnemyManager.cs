using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructoidEnemyManager : EnemyManagerTemplate
{
    private AiDestructoid[] m_destructoidAIComponents;

    // Called at the beginning of the game by the EnemyManager
    public override void Init(Transform p_parent)
    {
        base.Init(p_parent);

        // Initializes component array
        m_destructoidAIComponents = new AiDestructoid[Settings.maxEnemies];

        for (int i = 0; i < Settings.maxEnemies; i++)
        {
            m_destructoidAIComponents[i] = m_enemyGameObjects[i].GetComponent<AiDestructoid>();
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

        AiDestructoid tempDestructoid = m_destructoidAIComponents[p_enemyIndex];
        m_destructoidAIComponents[p_enemyIndex] = m_destructoidAIComponents[m_firstInactiveIndex - 1];
        m_destructoidAIComponents[m_firstInactiveIndex - 1] = tempDestructoid;
        m_destructoidAIComponents[p_enemyIndex].Index = p_enemyIndex;
        m_destructoidAIComponents[m_firstInactiveIndex - 1].Index = m_firstInactiveIndex - 1;

        // Decrements the first inactive index
        m_firstInactiveIndex--;

        return true;
    }

    public AiDestructoid GetActiveEnemyAI(int p_index)
    {
        if (p_index < 0 || p_index >= m_firstInactiveIndex)
        {
            Debug.Log("Invalid index " + p_index + " in DestructoidEnemy array");
            return null;
        }

        return m_destructoidAIComponents[p_index];
    }

    public AiDestructoid[] GetAllEnemyAI(out int p_firstInactiveIndex)
    {
        p_firstInactiveIndex = m_firstInactiveIndex;
        return m_destructoidAIComponents;
    }
}
