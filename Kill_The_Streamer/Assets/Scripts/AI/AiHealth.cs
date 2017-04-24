using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiHealth : AIBase {

    // Use this for initialization
    //all AI needs using UnityEngine.AI;
    private NavMeshAgent nav;//the navmeshAgent for the current AI. All AIs need a navMeshAgent to work.
    public float speed = 5.0f;

    public GameObject healthPrefab;
    void Start () {
        nav = GetComponent<NavMeshAgent>();//getting the navMesh component of the AI
    }

    // Update is called once per frame
    /*
	public override void Update () {
        base.Start();
	}
    */

    public override void AILoop()
    {

    }


    public override void DealDamage()
    {
        Player.s_Player.TakeDamage(1000, name, true);
    }


    public override void UpdateSpeed()
    {
        nav.speed = speed;
    }

    public override void TakeDamage()
    {
        health--;
        if (health <= 0)
        {
            Instantiate(healthPrefab, this.transform.position,Quaternion.identity);
            EnemyManager.DestroyEnemy(aiType, index);
            

        }
    }



}
