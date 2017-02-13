using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiShooter : MonoBehaviour {

    // Use this for initialization
    //all AI needs using UnityEngine.AI;
    private GameObject player;//the target to seek (player)
    private NavMeshAgent nav;//the navmeshAgent for the current AI. All AIs need a navMeshAgent to work.
    public int multBy = 1;//the number to multiply by what the Ai should run from
    private bool canAttack = true; //whether or not the AI can attack 
    private float attackCD = 1f;

    void Start()
    {
        //finding object with the tag "Player"
        player = GameObject.FindGameObjectWithTag("Player");
        nav = GetComponent<NavMeshAgent>();//getting the navMesh component of the AI

    }

    // Update is called once per frame
    void Update()
    {
        //Needs to be updated, short on time
        //Possible solutions: 
            //Stand still before fleeing and standing still again
            //Dodge back and forth a bit (not eradically) upon reach a certain distance from the player
        //Shooter should stay within a certain distance from the player
        float distFromPlayer = Vector3.Distance(player.transform.position, transform.position); //distance from the shooter to the player
        if(distFromPlayer > 3) //further than 3 units seek
        {
            Seek();
        }
        else if(distFromPlayer < 2) //closer than 2 units flee
        {
            Flee();
        }

        //Shoot for the player
        if (canAttack)
        {
            Vector3 enemyToPlayer = player.transform.position - transform.position;
            Quaternion rot = Quaternion.LookRotation(enemyToPlayer, Vector3.up);
            Instantiate(Resources.Load("Bullet"), transform.position, rot);
            canAttack = false;
        }


        //update attack cooldown
        if (!canAttack)
        {
            attackCD -= Time.deltaTime;
            if (attackCD <= 0)
            {
                canAttack = true;
                attackCD = 1;
            }
        }
    }


    /// <summary>
    /// Used for seeking out and going to the player.
    /// Based on NavMesh.
    /// Changing speed and acceleration can be found in inspector.
    /// </summary>
    public void Seek()
    {
        nav.SetDestination(player.transform.position);//telling the AI to seek out and go to the player's location
    }

    public void Flee()
    {
        Vector3 runTo = multBy * (transform.position - player.transform.position);
        nav.SetDestination(runTo);
    }
}
