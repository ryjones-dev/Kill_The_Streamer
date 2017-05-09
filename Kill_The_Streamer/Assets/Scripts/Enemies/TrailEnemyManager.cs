using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailEnemyManager : EnemyManagerTemplate
{
    private AiTrail[] m_trailAIComponents;

    // Called at the beginning of the game by the EnemyManager
    public override void Init(Transform p_parent)
    {
        base.Init(p_parent);

        // Initializes component array
        m_trailAIComponents = new AiTrail[Settings.maxEnemies];

        for (int i = 0; i < Settings.maxEnemies; i++)
        {
            m_trailAIComponents[i] = m_enemyGameObjects[i].GetComponent<AiTrail>();
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

        AiTrail tempTrail = m_trailAIComponents[p_enemyIndex];
        m_trailAIComponents[p_enemyIndex] = m_trailAIComponents[m_firstInactiveIndex - 1];
        m_trailAIComponents[m_firstInactiveIndex - 1] = tempTrail;
        m_trailAIComponents[p_enemyIndex].Index = p_enemyIndex;
        m_trailAIComponents[m_firstInactiveIndex - 1].Index = m_firstInactiveIndex - 1;

        // Decrements the first inactive index
        m_firstInactiveIndex--;

        return true;
    }

    public AiTrail GetActiveEnemyAI(int p_index)
    {
        if (p_index < 0 || p_index >= m_firstInactiveIndex)
        {
            Debug.Log("Invalid index " + p_index + " in TrailEnemy array");
            return null;
        }

        return m_trailAIComponents[p_index];
    }

    public AiTrail[] GetAllEnemyAI(out int p_firstInactiveIndex)
    {
        p_firstInactiveIndex = m_firstInactiveIndex;
        return m_trailAIComponents;
    }
}
