using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemyManager : EnemyManagerTemplate
{
    private AiShooter[] m_shooterAIComponents;

    // Called at the beginning of the game by the EnemyManager
    public override void Init(Transform p_parent)
    {
        base.Init(p_parent);

        // Initializes component array
        m_shooterAIComponents = new AiShooter[Settings.maxEnemies];

        for (int i = 0; i < Settings.maxEnemies; i++)
        {
            m_shooterAIComponents[i] = m_enemyGameObjects[i].GetComponent<AiShooter>();
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

        AiShooter tempShooter = m_shooterAIComponents[p_enemyIndex];
        m_shooterAIComponents[p_enemyIndex] = m_shooterAIComponents[m_firstInactiveIndex - 1];
        m_shooterAIComponents[m_firstInactiveIndex - 1] = tempShooter;
        m_shooterAIComponents[p_enemyIndex].Index = p_enemyIndex;
        m_shooterAIComponents[m_firstInactiveIndex - 1].Index = m_firstInactiveIndex - 1;

        // Decrements the first inactive index
        m_firstInactiveIndex--;

        return true;
    }

    public AiShooter GetActiveEnemyAI(int p_index)
    {
        if (p_index < 0 || p_index >= m_firstInactiveIndex)
        {
            Debug.Log("Invalid index " + p_index + " in ShooterEnemy array");
            return null;
        }

        return m_shooterAIComponents[p_index];
    }

    public AiShooter[] GetAllEnemyAI(out int p_firstInactiveIndex)
    {
        p_firstInactiveIndex = m_firstInactiveIndex;
        return m_shooterAIComponents;
    }
}
