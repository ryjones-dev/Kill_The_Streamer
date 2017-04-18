using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiShieldSeek : AIBase{

    // Use this for initialization
    //all AI needs using UnityEngine.AI;
    private NavMeshAgent nav;//the navmeshAgent for the current AI. All AIs need a navMeshAgent to work.

    private GameObject shield;
    public int shieldHealthStart;
    private int shieldHealth;

    private bool shieldActive;
    public bool ShieldActive { get { return shieldActive; } }

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



	public override void Start()
    {
        base.Start();
        
        nav = GetComponent<NavMeshAgent>();//getting the navMesh component of the AI


        shield = transform.FindChild("Shield").gameObject;
        shieldActive = true;

        defaultSpeed = nav.speed;
        defaultRotationSpeed = nav.angularSpeed;
        defaultAcceleration = nav.acceleration;


    }

    public override void AILoop()
    {
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
        nav.SetDestination(m_target.Position);//telling the AI to seek out and go to the player's location
    }

    public void ShieldTakeDamage()
    {
        shieldHealth--;
        if(shieldHealth <= 0)
        {
            shieldActive = false;
            shield.SetActive(false);
        }
    }

	public override void Initialize(){
        base.Initialize();

		shieldActive = true;
		shieldHealth = shieldHealthStart;
		shield.SetActive(true);
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
