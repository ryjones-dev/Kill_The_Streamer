using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiSeekFlee : AIBase{

    // Use this for initialization
    // Use this for initialization
    //all AI needs using UnityEngine.AI;
    private GameObject player;//the target to seek (player)
    private NavMeshAgent nav;//the navmeshAgent for the current AI. All AIs need a navMeshAgent to work.

    public int multBy = 5;//the number to multiply by what the Ai should run from

    private bool inPlayerSight;//tells if in the player's sight


    public int timeResetSeek;//tells how long after the player stops looking at the enemy until it should go back to seeking (should be small. Just as a buffer time)

    public float rangeView = 10.0f;//the range of view that the player can see to

    //anarchy and regular values
    public bool anarchyMode = false;
    //default info
    public float defaultSpeed;
    public float defaultRotationSpeed;
    public float defaultAcceleration;
    //anarchy info

    public int booDamage=3000;

    private const float c_ANARCHY_SPEED_MULT = 2;
    private const float c_ANARCHY_ROTATION_MULT = 2;
    private const float c_ANARCHY_ACCELERATION_MULT = 2;

    public override void Start () {
        base.Start();

        //finding object with the tag "Player"
        player = Player.s_Player.gameObject;
        //playerTargetting = player.GetComponent<PlayerLookingAtAI>();
        inPlayerSight = false;
        nav = GetComponent<NavMeshAgent>();//getting the navMesh component of the AI

        defaultSpeed = nav.speed;
        defaultRotationSpeed = nav.angularSpeed;
        defaultAcceleration = nav.acceleration;

    }
    public bool InPlayerSight
    {
        get { return inPlayerSight; }
        set { inPlayerSight = value; }
    }

    protected override void Update()
    {
        base.Update();
        CheckPlayerLooking();
        if (inPlayerSight)
        {
            Flee();
        }
    }

    public override void AILoop()
    {        
        if(!InPlayerSight)
        {
            Seek();
        }
    }

    public override void UpdateSpeed()
    {
        if (m_anarchyMode) {
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

    /// <summary>
    /// Used for seeking out and going to the player.
    /// Based on NavMesh.
    /// Changing speed and acceleration can be found in inspector.
    /// </summary>
    public void Seek()
    {
        nav.SetDestination(Player.s_Player.FastTransform.Position);//telling the AI to seek out and go to the player's location
    }


    public void Flee()
    {
        Vector3 runTo = multBy * (m_transform.Position - Player.s_Player.FastTransform.Position);
        nav.SetDestination(runTo);
    }

    public void CheckPlayerLooking()
    {
        Vector3 playerToEnemy = m_transform.Position - Player.s_Player.FastTransform.Position ;
        bool positiveLeft = (Vector3.Dot(Player.s_Player.LeftVisionAngle, playerToEnemy) >= 0);
        bool positiveRight = (Vector3.Dot(Player.s_Player.RightVisionAngle, playerToEnemy) >= 0);

        if (positiveLeft && positiveRight)
        {
            //get distance
            float distance = (Player.s_Player.FastTransform.Position - m_transform.Position).sqrMagnitude ;
            if (distance <= rangeView * rangeView)
            {
                //runaway
                inPlayerSight = true;
            }
            else
            {
                inPlayerSight = false;
            }
        }
        else
        {
            inPlayerSight = false;
        }
    }

    public override void DealDamage()
    {
		Player.s_Player.TakeDamage(booDamage, name, true);
    }

	void OnTriggerEnter(Collider col){
		if(col.CompareTag("PlayerHitbox")){
			DealDamage();
		}
	}
	void OnTriggerStay(Collider col){
		if(col.CompareTag("PlayerHitbox")){
			DealDamage();
		}
	}
}
