using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemyManager : EnemyManagerTemplate
{
    private AiShieldSeek[] m_shieldSeekComponents;

    // Called at the beginning of the game by the EnemyManager
    public override void Init(Transform p_parent)
    {
        base.Init(p_parent);

        m_shieldSeekComponents = new AiShieldSeek[Constants.MAX_ENEMIES];

        for (int i = 0; i < Constants.MAX_ENEMIES; i++)
        {
            m_shieldSeekComponents[i] = m_enemyGameObjects[i].GetComponent<AiShieldSeek>();
        }
    }

    // Called by the enemy manager when deactivating an enemy. Returns true
    public override bool DeactivateEnemy(int p_enemyIndex)
    {
        // Fails if the enemy index is invalid or if there are no active enemies 
        if (!base.DeactivateEnemy(p_enemyIndex))
        {
            return false;
        }

        AiShieldSeek tempShieldSeek = m_shieldSeekComponents[p_enemyIndex];
        m_shieldSeekComponents[p_enemyIndex] = m_shieldSeekComponents[m_firstInactiveIndex - 1];
        m_shieldSeekComponents[m_firstInactiveIndex - 1] = tempShieldSeek;
        m_shieldSeekComponents[p_enemyIndex].Index = p_enemyIndex;
        m_shieldSeekComponents[m_firstInactiveIndex - 1].Index = m_firstInactiveIndex - 1;

        // Decrements the first inactive index
        m_firstInactiveIndex--;

        return true;
    }

    public AiShieldSeek GetActiveEnemySeekAI(int p_index)
    {
        if (p_index < 0 || p_index >= m_firstInactiveIndex)
        {
            Debug.Log("Invalid index " + p_index + " in ShieldEnemy array");
            return null;
        }

        return m_shieldSeekComponents[p_index];
    }

    public AiShieldSeek[] GetAllEnemySeekAI(out int p_firstInactiveIndex)
    {
        p_firstInactiveIndex = m_firstInactiveIndex;
        return m_shieldSeekComponents;
    }
}
