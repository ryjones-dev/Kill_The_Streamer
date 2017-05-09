using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekEnemyManager : EnemyManagerTemplate
{
    private AiSeeking[] m_seekComponents;

    // Called at the beginning of the game by the EnemyManager
    public override void Init(Transform p_parent)
    {
        base.Init(p_parent);

        // Initializes component array
        m_seekComponents = new AiSeeking[Settings.maxEnemies];

        for (int i = 0; i < Settings.maxEnemies; i++)
        {
            m_seekComponents[i] = m_enemyGameObjects[i].GetComponent<AiSeeking>();
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

        AiSeeking tempSeek = m_seekComponents[p_enemyIndex];
        m_seekComponents[p_enemyIndex] = m_seekComponents[m_firstInactiveIndex - 1];
        m_seekComponents[m_firstInactiveIndex - 1] = tempSeek;
        m_seekComponents[p_enemyIndex].Index = p_enemyIndex;
        m_seekComponents[m_firstInactiveIndex - 1].Index = m_firstInactiveIndex - 1;

        // Decrements the first inactive index
        m_firstInactiveIndex--;

        return true;
    }

    public AiSeeking GetActiveEnemyAI(int p_index)
    {
        if (p_index < 0 || p_index >= m_firstInactiveIndex)
        {
            Debug.Log("Invalid index " + p_index + " in SeekEnemy array");
            return null;
        }

        return m_seekComponents[p_index];
    }

    public AiSeeking[] GetAllEnemyAI(out int p_firstInactiveIndex)
    {
        p_firstInactiveIndex = m_firstInactiveIndex;
        return m_seekComponents;
    }
}
