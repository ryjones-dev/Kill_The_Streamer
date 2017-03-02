﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Possible AI (Current is #1): 
//1: Stand still before fleeing and standing still again
//2: Dodge back and forth a bit (not eradically) upon reach a certain distance from the player
//3: Shooter should stay within a certain distance from the player
public class AiShooter : MonoBehaviour
{

    // Use this for initialization
    //all AI needs using UnityEngine.AI;
    private GameObject player;//the target to seek (player)
    private NavMeshAgent nav;//the navmeshAgent for the current AI. All AIs need a navMeshAgent to work.
    private bool canAttack; //whether or not the AI can attack 
    private float attackTimer;
    private bool inLineOfSight;
    private bool isStopped;
    private float stopCD;
    private bool isFleeing;
    private float fleeCD;
    private float distFromPlayer;
    private NavMeshHit onlyExistsToRaycast;
    public float minimumRange;

    void Start()
    {
        //finding object with the tag "Player"
        player = GameObject.FindGameObjectWithTag("Player");
        nav = GetComponent<NavMeshAgent>();//getting the navMesh component of the AI
        canAttack = true;
        attackTimer = 1f;
        inLineOfSight = false;
        isStopped = false;
        stopCD = 1.5f;
        isFleeing = false;
        fleeCD = 1.5f;
        minimumRange = 5; // range at which the AI will stop approaching
        nav.stoppingDistance = minimumRange;
    }

    // Update is called once per frame
    void Update()
    {
        distFromPlayer = Vector3.Distance(player.transform.position, transform.position); //distance from the shooter to the player
        if (nav.Raycast(player.transform.position, out onlyExistsToRaycast)) //whether or not the AI could hit the player from current position
        {
            isStopped = false; //instantly begin seeking if player is not in sight
            isFleeing = false;
            inLineOfSight = false; //don't shoot if not in line of sight of player, that would look stupid
        }
        else inLineOfSight = true;

        if (isStopped)
        {
            Stop(); //called between every movement switch in Move()
        }
        else {
            Move(); //if player is within the minimumDistance Flee() for a certain amount of time, else Seek()
        }

        CheckShoot(); //tick attack's cooldown, shoot if at 0
    }

    public void CheckShoot()
    {
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
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0 && inLineOfSight)
            {
                canAttack = true;
                attackTimer = 1;
            }
        }
    }

    public void Move()
    {
        if (distFromPlayer <= minimumRange && !isFleeing) //if at 3 units pause unless currently fleeing from player
        {
            isStopped = true;
        }
        else if (isFleeing)
        {
            fleeCD -= Time.deltaTime;
            Flee();
            if (fleeCD <= 0)
            {
                isFleeing = false;
                fleeCD = 1.5f;
                isStopped = true;
            }
        }
        else
        {
            Seek();
        }
    }

    public void Stop()
    {
        stopCD -= Time.deltaTime;
        if (stopCD <= 0)
        {
            isStopped = false;
            stopCD = 1.5f;
            //Do a check to see if AI should seek or flee after stop
            //if within 3 range flee else Seek player
            if (distFromPlayer <= minimumRange)
            {
                isFleeing = true;
                Flee();
            }
            else Seek();
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
        Vector3 runTo = 3 *(transform.position - player.transform.position);
        nav.SetDestination(runTo);
    }
}