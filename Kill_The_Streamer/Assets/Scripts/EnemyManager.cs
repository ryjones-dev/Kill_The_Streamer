using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    /*
        Order of enemy spawning:
        1. Copy enemy data to first inactive enemy
        2. Active Unity components
        3. Update inactive index 
    */

    private const int MAX_ENEMIES = 1024;

    private Enemy[] m_enemies; // Sorted array of enemies. Active enemies will be at the beginning of the array, and inactive enemies will be at the end.
    private short m_firstInactiveIndex = 0; // Stores the index separating the active and inactive objects. (If 0, there are no active enemies)

    // Singleton instance
    private static EnemyManager s_instance;

    private void Awake()
    {
        // Sets up the singleton
        if(s_instance != null)
        {
            s_instance = this;
            
            // Preallocate the enemy array
            m_enemies = new Enemy[MAX_ENEMIES];
        }
        else
        {
            // Destroys the object if there is already a singleton instance
            Destroy(gameObject);
        }
    }
}
