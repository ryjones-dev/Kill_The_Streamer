using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiTrail : AIBase{

    //all AI needs using UnityEngine.AI;
    private GameObject player;//the target to seek (player)
    private NavMeshAgent nav;//the navmeshAgent for the current AI. All AIs need a navMeshAgent to work.
    public float speedModifier = 0.05f;

    private float trailTimer;
    private float resetTrailTime;

    //anarchy and regular values
    public bool anarchyMode = false;
    //default info
    public float defaultSpeed;
    private float defaultResetTrailTime;
    //anarchy info
    private const float c_ANARCHY_SPEED_MULT = 2.0f;

    // Use this for initialization
    public override void Start () {
        //finding object with the tag "Player"
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player");
        nav = GetComponent<NavMeshAgent>();//getting the navMesh component of the AI
        //Instantiate(Resources.Load("Trail"), transform.position, transform.rotation);


        resetTrailTime = 0.14f;
        trailTimer = resetTrailTime;

        //set initialPosition
        NavMeshHit hit;
        //nav.FindClosestEdge(out hit);

        defaultSpeed = speedModifier;
        defaultResetTrailTime = resetTrailTime;
        
    }
	
	// Update is called once per frame
	protected override void Update () {
        
        base.Update();
        //keeps the salt flowing
        DropTrail();
    }

    public override void AILoop()
    {
        //more of a frantic dash around than a flee;
        Flee();
    }

    public override void DealDamage()
    {
		Player.s_Player.TakeDamage(10, name, false);
    }

    public override void UpdateSpeed()
    {
        if (m_anarchyMode)
        {
            nav.speed = defaultSpeed * EnemyManager.SpeedMultiplier * c_ANARCHY_SPEED_MULT;
        }
        else
        {
            nav.speed = defaultSpeed * EnemyManager.SpeedMultiplier;
        }
    }

    public void DropTrail()
    {
        trailTimer -= Time.deltaTime;
        if (trailTimer <= 0)
        {
            Instantiate(Resources.Load("Trail"), transform.position, transform.rotation);
            trailTimer = resetTrailTime;
        }
    }
    //prefered way of working
    //Start: find closest edge then run away from that
    //if within 1 unit of another edge update position to run away from that edge
    //repeat
    public void Flee()
    {
        NavMeshHit currentHit;
        nav.FindClosestEdge(out currentHit);

        //if it gets too close to the player, change direction
        if (Vector3.Distance(gameObject.transform.position, player.transform.position) <= 1f)
        {
            //change direction
            Vector3 lookAtPosition = gameObject.transform.position - player.transform.position;
            gameObject.transform.LookAt(lookAtPosition);
        }
        
            //if distance to wall is small change direction
        if(currentHit.distance <= 0.2)
        {
            //change direction to offset hit off of wall in somewhat random direction
            Vector3 lookAtPosition = gameObject.transform.position + -gameObject.transform.right + currentHit.normal;
            gameObject.transform.LookAt(lookAtPosition);

            //initialHitPosition = currentHit.position;
        }

        Vector3 moveTo = gameObject.transform.forward.normalized * speedModifier;

        gameObject.transform.position = gameObject.transform.position + moveTo;
    }
}
