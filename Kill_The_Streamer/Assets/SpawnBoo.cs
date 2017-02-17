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
            EnemyManager.CreateEnemy(EnemyType.BooEnemy, "Twitch Username", transform.position);
        }
    }
}
