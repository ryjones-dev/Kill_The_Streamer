using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiSeekFlee : MonoBehaviour {

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
    private float anarchySpeed;
    private float anarchyRotationSpeed;
    private float anarchyAcceleration;



    void Start () {
        //finding object with the tag "Player"
        player = GameObject.FindGameObjectWithTag("Player");
        //playerTargetting = player.GetComponent<PlayerLookingAtAI>();
        inPlayerSight = false;
        nav = GetComponent<NavMeshAgent>();//getting the navMesh component of the AI

        defaultSpeed = nav.speed;
        defaultRotationSpeed = nav.angularSpeed;
        defaultAcceleration = nav.acceleration;

        anarchySpeed = defaultSpeed * 2;
        anarchyRotationSpeed = defaultRotationSpeed * 2;
        anarchyAcceleration = defaultAcceleration * 2;

    }
    public bool InPlayerSight
    {
        get { return inPlayerSight; }
        set { inPlayerSight = value; }
    }
	
	// Update is called once per frame
	void Update () {
        CheckPlayerLooking();
        AnarchyEnabled();
        if (inPlayerSight==true)
        {
            Flee();
        }
        else
        {
            Seek();
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
        nav.SetDestination(player.transform.position);//telling the AI to seek out and go to the player's location
    }


    public void Flee()
    {
        Vector3 runTo = multBy * (transform.position - player.transform.position);
        nav.SetDestination(runTo);
    }

    public void CheckPlayerLooking()
    {
        //getting the forward vector of the player
        Vector3 leftAngle = Quaternion.AngleAxis(-45, player.transform.up) * player.transform.forward;
        Vector3 rightAngle = Quaternion.AngleAxis(45, player.transform.up) *player.transform.forward;
        Debug.DrawLine(player.transform.position + rightAngle * 10, player.transform.position, Color.cyan);
        Debug.DrawLine(player.transform.position + leftAngle * 10, player.transform.position, Color.cyan);



        Vector3 playerToEnemy = transform.position - player.transform.position ;
        bool positiveLeft = (Vector3.Dot(leftAngle, playerToEnemy) >= 0);
        //Debug.DrawLine(transform.position, Vector3.Dot(leftAngle, playerToEnemy));
        bool positiveRight = (Vector3.Dot(rightAngle, playerToEnemy) >= 0);
        if (positiveLeft && positiveRight)
        {
            //get distance
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance <= rangeView)
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
        
    }
