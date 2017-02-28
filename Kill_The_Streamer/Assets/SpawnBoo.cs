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
        if(Input.GetKey(KeyCode.P))
        {
            EnemyManager.CreateEnemy(EnemyType.BooEnemy, "Twitch Username", Direction.Up);
        }
    }
}
