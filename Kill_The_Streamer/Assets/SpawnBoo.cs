using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBoo : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            EnemyManager.CreateEnemy(EnemyType.BooEnemy, "Boo AI", transform.position);
        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            EnemyManager.CreateEnemy(EnemyType.SeekEnemy, "Seek AI", transform.position);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            EnemyManager.CreateEnemy(EnemyType.GhostEnemy, "Ghost AI", transform.position);
        }
    }
}
