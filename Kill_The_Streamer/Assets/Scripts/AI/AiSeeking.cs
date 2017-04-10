using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiSeeking : AIBase
{

    // Use this for initialization
    //all AI needs using UnityEngine.AI;
    private NavMeshAgent nav;//the navmeshAgent for the current AI. All AIs need a navMeshAgent to work.

    public float distanceActive =12.0f;//will give the distance that it will be able to go to another shield

 
    private float maxDist;//the distance behind the leader the UI will go to
    private bool splitOff;//tells if the UI should split off away from the shielder and seek out the player instead

    private float closeShield;//gives the closest shield dist

    public float distanceAwayPlayer;//gives the distance away from player before the seeks will seek out the player and ignore shielder

    //anarchy and regular values
    public bool anarchyMode = false;
    //default info
    public float defaultSpeed;
    public float defaultRotationSpeed;
    public float defaultAcceleration;
    //anarchy info
    private const float c_ANARCHY_SPEED_MULT = 2;
    private const float c_ANARCHY_ROTATION_MULT = 2;
    private const float c_ANARCHY_ACCELERATION_MULT = 2;

    public AiShieldSeek leader;

    private int skipFrame;



    public override void Start () {
        base.Start();

        leader = null;
        skipFrame = 0;

        nav = GetComponent<NavMeshAgent>();//getting the navMesh component of the AI
        closeShield = float.MaxValue;
        //give seeker a random range to be away from the leader
        maxDist = UnityEngine.Random.Range(1.5f, 5.0f);
        splitOff = true;//seek out player

        defaultSpeed = nav.speed;
        defaultRotationSpeed = nav.angularSpeed;
        defaultAcceleration = nav.acceleration;
    }

    public override void AILoop()
    {
        if (leader && !leader.gameObject)
        {
            leader = null;
        }
        
        ClosestShield();
        Seek();
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
		Player.s_Player.TakeDamage(1000, name, true);
    }

    /// <summary>
    /// Used for seeking out and going to the player.
    /// Based on NavMesh.
    /// Changing speed and acceleration can be found in inspector.
    /// </summary>
    public void Seek()
    {
        if((m_target.position - m_transform.Position).sqrMagnitude < distanceAwayPlayer * distanceAwayPlayer)
        {
            leader = null;
        }

        if (leader)
        {
            nav.SetDestination(leader.FastTransform.Position - leader.FastTransform.Trans.forward);
        }
        else
        {
            nav.SetDestination(m_target.position);//telling the AI to seek out and go to the player's location
        }
      
    }

    /// <summary>
    /// This is for checking and finding the closest shield and if so then see it and follow until close enough to player.
    /// Will get the closest enabled shield. If not, continue seeking.
    /// </summary>
    public void ClosestShield()
    {
        if(skipFrame > 0)
        {
            skipFrame--;
            return;
        }
        skipFrame = 20;

        //get all shielders in level
        int activeLength;
        AiShieldSeek[] shieldsInLevel = (AiShieldSeek[])EnemyManager.GetAllEnemyAI(EnemyType.ShieldEnemy, out activeLength);

        //make sure they are enabled
        if (shieldsInLevel.Length == 0)
        {
            //splitOff = true;
            return;
        }

        //get some random large distance for now
        float distance = 200.0f;

        //have it store some gameobjects and the "leader"
        GameObject shieldGameObject=null;//the gameobject of the shield in order to detect which one is closest

 
        //check the shields
        for (int i =0; i < activeLength; i++)
        {
            //are the shields active?
            if(shieldsInLevel[i].ShieldActive)
            {
                //get distance from position to shielder
                distance = (m_transform.Position - shieldsInLevel[i].FastTransform.Position).sqrMagnitude;
                //is it close enough?
                if (distance <= distanceActive * distanceActive)
                {
                    //store the shield that it is the closest to
                    if (distance < closeShield)
                    {
                        //Debug.Log(distance);
                        shieldGameObject = shieldsInLevel[i].gameObject;
                        leader = shieldGameObject.GetComponent<AiShieldSeek>();
                        //splitOff = false;
                        closeShield = distance;
                    }
                }

            }
        }
        //has there not been a closest shield found?
        if(!shieldGameObject)
        {
            closeShield = float.MaxValue;
        }
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
