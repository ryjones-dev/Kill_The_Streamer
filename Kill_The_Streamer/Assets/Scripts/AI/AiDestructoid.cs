using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AiDestructoid : AIBase {

    public float destructTimer;//how long till detination
    private float currentTimer;//the current number on the timer

    private GameObject player; //the player

    public float triggerDistance = 4.0f;//the distance at which the player will be within the range to trigger. 
    public float damageDistance = 5.0f;//the range at which the player will take damage

    private SpriteRenderer colorChange;//this will hold the sprite of the MrDestructoid to change the color when it will explode

    private bool toExplode;//tells if the Ai is ready to explode

    private NavMeshAgent nav;//the navmeshAgent for the current AI. All AIs need a navMeshAgent to work.

    private float distance;//the distance from AI to player.

    private Color defaultColor;

    //anarchy and regular values
    public bool anarchyMode = false;
    //default info
    public float defaultSpeed;
    public float defaultRotationSpeed;
    public float defaultAcceleration;
    public float defaultTimer;
    //anarchy info
    private const float c_ANARCHY_SPEED_MULT = 2;
    private const float c_ANARCHY_ROTATION_MULT = 2;
    private const float c_ANARCHY_ACCELERATION_MULT = 2;
    private const float c_ANARCHY_TIMER_MULT = 0.5f;

    // Use this for initialization
    public override void Start () {
        base.Start();
        colorChange = GetComponentInChildren<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");

        nav = GetComponent<NavMeshAgent>();//getting the navMesh component of the AI

        destructTimer = 2;
        toExplode = false;

        defaultSpeed = nav.speed;
        defaultRotationSpeed = nav.angularSpeed;
        defaultAcceleration = nav.acceleration;
        defaultTimer = destructTimer;
        
    }
	
    public override void AILoop()
    {
        distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance < triggerDistance)
        {
            toExplode = true;
            Explode();
        }
        else
        {
            if (!toExplode)
                Seek();
        }
    }

    public override void UpdateSpeed()
    {
        if (m_anarchyMode)
        {
            nav.speed = defaultSpeed * EnemyManager.SpeedMultiplier * c_ANARCHY_SPEED_MULT;
            nav.angularSpeed = defaultRotationSpeed * EnemyManager.SpeedMultiplier * c_ANARCHY_ROTATION_MULT;
            nav.acceleration = defaultAcceleration * EnemyManager.SpeedMultiplier * c_ANARCHY_ACCELERATION_MULT;
            destructTimer = defaultTimer * c_ANARCHY_TIMER_MULT;
        }
        else
        {
            nav.speed = defaultSpeed * EnemyManager.SpeedMultiplier;
            nav.angularSpeed = defaultRotationSpeed * EnemyManager.SpeedMultiplier;
            nav.acceleration = defaultAcceleration * EnemyManager.SpeedMultiplier;
            destructTimer = defaultTimer;
        }
    }

    public override void DealDamage()
    {
        throw new NotImplementedException();
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
    /// If Destructoid is close enough to the player
    /// then the destructoid will become invincible and set off the countdown timer
    /// The destructoid will flash and then explode when it reaches zero
    /// </summary>
    private void Explode()
    {
        if(toExplode==true)
        {
            nav.Stop();
            currentTimer += Time.deltaTime;
            //Change color of sprite
            //Should fade in white
            //If would rather have immediate white, then fade out put "1 - whiteValue"
            float whiteValue = 1 - (currentTimer % 0.4f) * 2.5f;
            //Can't use until spriteRenderer is added
            colorChange.color = new Color(whiteValue, whiteValue, whiteValue, 1);
            //rend.material.color = new Color(whiteValue, whiteValue, whiteValue, 1);

            if (currentTimer >= destructTimer)
            {
                //Add in:
                //Play animation

                //Add in:
                //Deals damage to player
                if (distance <=damageDistance)
                {
                    //do damage

                }
                //deactivates self
                DestroyImmediate(gameObject);
            }
        }
    }

}
