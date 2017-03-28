using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Possible AI (Current is #1): 
//1: Stand still before fleeing and standing still again
//2: Dodge back and forth a bit (not eradically) upon reach a certain distance from the player
//3: Shooter should stay within a certain distance from the player
public class AiShooter : AIBase
{

    // Use this for initialization
    //all AI needs using UnityEngine.AI;
    private GameObject player;//the target to seek (player)
    private NavMeshAgent nav;//the navmeshAgent for the current AI. All AIs need a navMeshAgent to work.
    private bool canAttack; //whether or not the AI can attack 
    private float attackTimer;
    private float attackResetTimer;
    private bool inLineOfSight;
    private bool isStopped;
    private float stopCD;
    private bool isFleeing;
    private float fleeCD;
    private float distFromPlayer;
    private NavMeshHit onlyExistsToRaycast;
    public float minimumRange;
    int randMovement;


    //default info
    public float defaultSpeed;
    public float defaultRotationSpeed;
    public float defaultAcceleration;
    public float defaultShootTimer;
    //anarchy info
    private const float c_ANARCHY_SPEED_MULT = 2;
    private const float c_ANARCHY_ROTATION_MULT = 2;
    private const float c_ANARCHY_ACCELERATION_MULT = 2;
    private float anarchyShootTimer;

    protected override void Start()
    {
        base.Start();

        //finding object with the tag "Player"
        player = GameObject.FindGameObjectWithTag("Player");
        nav = GetComponent<NavMeshAgent>();//getting the navMesh component of the AI
        canAttack = true;
        attackResetTimer = 1f;
        attackTimer = attackResetTimer;
        inLineOfSight = false;
        isStopped = false;
        stopCD = 1.5f;
        isFleeing = false;
        fleeCD = 1.5f;
        minimumRange = 7; // range at which the AI will stop approaching

        defaultSpeed = nav.speed;
        defaultRotationSpeed = nav.angularSpeed;
        defaultAcceleration = nav.acceleration;
        defaultShootTimer = attackResetTimer;
        
        anarchyShootTimer = attackResetTimer / 2;

        //stopping distance is closer as to not freeze at the edge outside of stopping distance and freeze on trying to Seek()
        nav.stoppingDistance = minimumRange - 1;
        InvokeRepeating("ChangeRandom", 0, 1.5f);
    }

    void ChangeRandom()
    {
        randMovement = UnityEngine.Random.Range(-1, 2);
        Debug.Log(randMovement);
    }

    public override void AILoop()
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
        else
        {
            Move(); //if player is within the minimumDistance Flee() for a certain amount of time, else Seek()
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        CheckShoot(); //tick attack's cooldown, shoot if at 0
    }
    public override void UpdateSpeed()
    {
        if (m_anarchyMode)
        {
            nav.speed = defaultSpeed * EnemyManager.SpeedMultiplier * c_ANARCHY_SPEED_MULT;
            nav.angularSpeed = defaultRotationSpeed * EnemyManager.SpeedMultiplier * c_ANARCHY_ROTATION_MULT;
            nav.acceleration = defaultAcceleration * EnemyManager.SpeedMultiplier * c_ANARCHY_ACCELERATION_MULT;
        }
        else
        {
            nav.speed = defaultSpeed * EnemyManager.SpeedMultiplier;
            nav.angularSpeed = defaultRotationSpeed * EnemyManager.SpeedMultiplier;
            nav.acceleration = defaultAcceleration * EnemyManager.SpeedMultiplier;
        }
    }

    public override void DealDamage()
    {
        throw new NotImplementedException();
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
                attackTimer = attackResetTimer;
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
            Flee(randMovement);
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
                Flee(randMovement);
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

    /// <summary>
    /// Used for fleeing from or stafing sideways out from the player.
    /// Based on NavMesh.
    /// 3 results, strafe left, strafe right, or move back and flee from the player
    /// </summary>
    public void Flee(int movementChoice)
    {
        //Vector3 lookAtPosition = gameObject.transform.position + gameObject.transform.right;
        //gameObject.transform.LookAt(lookAtPosition);

        if (movementChoice == 0)
        {
            //Vector3 runTo = 2 * (transform.position - player.transform.position);
            Vector3 runTo = -transform.forward * 0.02f;
            gameObject.transform.position = gameObject.transform.position + runTo;
            //nav.SetDestination(runTo);
        }

        Vector3 moveTo = movementChoice * gameObject.transform.right.normalized * 0.02f;
        gameObject.transform.position = gameObject.transform.position + moveTo;
    }
}
