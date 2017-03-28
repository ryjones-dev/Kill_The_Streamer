using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiSeeking : AIBase
{

    // Use this for initialization
    //all AI needs using UnityEngine.AI;
    private GameObject player;//the target to seek (player)
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
    private float anarchySpeed;
    private float anarchyRotationSpeed;
    private float anarchyAcceleration;
    public Transform myTransform;
    public Vector3 myPosition;
    public AiShieldSeek leader;

    private int skipFrame;


    void Start () {
        leader = null;
        //finding object with the tag "Player"
        myTransform = this.transform;
        myPosition = myTransform.position;
        skipFrame = 0;

        player = PlayerController.s_Player.gameObject;
        nav = GetComponent<NavMeshAgent>();//getting the navMesh component of the AI
        closeShield = float.MaxValue;
        //give seeker a random range to be away from the leader
        maxDist = Random.Range(1.5f, 5.0f);
        splitOff = true;//seek out player

        defaultSpeed = nav.speed;
        defaultRotationSpeed = nav.angularSpeed;
        defaultAcceleration = nav.acceleration;

        anarchySpeed = defaultSpeed * 2;
        anarchyRotationSpeed = defaultRotationSpeed * 2;
        anarchyAcceleration = defaultAcceleration * 2;

    }

    void LateUpdate()
    {
        myPosition = myTransform.position;
    }

    // Update is called once per frame
    void Update () {
        // Used for seeking out and going to the player.
        // Based on NavMesh.
        // Changing speed and acceleration can be found in inspector.
        //nav.SetDestination(player.transform.position);//telling the AI to seek out and go to the player's location
        AnarchyEnabled();
        ClosestShield();
        Seek();
        if (leader && !leader.gameObject)
        {
            leader = null;
        }
    }


    /// <summary>
    /// This is the for the speeds and values during anarchy mode.
    /// </summary>
    public void AnarchyEnabled()
    {
        if (anarchyMode == false)
        {
            nav.speed = defaultSpeed;
            nav.angularSpeed = defaultRotationSpeed;
            nav.acceleration = defaultAcceleration;
        }
        else if (anarchyMode)
        {
            nav.speed = anarchySpeed;
            nav.angularSpeed = anarchyRotationSpeed;
            nav.acceleration = anarchyAcceleration;

        }
    }

    /// <summary>
    /// Used for seeking out and going to the player.
    /// Based on NavMesh.
    /// Changing speed and acceleration can be found in inspector.
    /// </summary>
    public void Seek()
    {
        if((PlayerController.s_Player.m_myPosition - myPosition).sqrMagnitude < distanceAwayPlayer * distanceAwayPlayer)
        {
            leader = null;
        }

        if (leader)
        {
            nav.SetDestination(leader.myPosition - leader.myTransform.forward);
        }
        else
        {
            nav.SetDestination(PlayerController.s_Player.m_myPosition);//telling the AI to seek out and go to the player's location
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
                distance = (myPosition - shieldsInLevel[i].myPosition).sqrMagnitude;
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

}
