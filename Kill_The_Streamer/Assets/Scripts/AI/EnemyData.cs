using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    public int m_damageDealt; // Amount of damage the enemy can deal
    public int m_health; // The number of hits the enemy can take

    private int m_index; // The index of the enemy in the array
    public int m_Index { get { return m_index; } set { m_index = value; } }

    public void Clear()
    {
        m_damageDealt = 0;
        m_health = 0;
        m_index = 0;
    }
}
