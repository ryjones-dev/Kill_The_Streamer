using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    public int m_damageDealt; // Amount of damage the enemy can deal
    public int m_health; // The number of hits the enemy can take

    private short m_index; // The index of the enemy in the array
    public short m_Index { get { return m_index; } set { m_index = value; } }
}
