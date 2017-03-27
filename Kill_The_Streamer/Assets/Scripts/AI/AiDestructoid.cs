using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AiDestructoid : MonoBehaviour {

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
    private float anarchySpeed;
    private float anarchyRotationSpeed;
    private float anarchyAcceleration;
    private float anarchyTimer;

    // Use this for initialization
    void Start () {
        colorChange = GetComponentInChildren<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");

        nav = GetComponent<NavMeshAgent>();//getting the navMesh component of the AI

        destructTimer = 2;
        toExplode = false;

        defaultSpeed = nav.speed;
        defaultRotationSpeed = nav.angularSpeed;
        defaultAcceleration = nav.acceleration;
        defaultTimer = destructTimer;

        anarchySpeed = defaultSpeed * 2;
        anarchyRotationSpeed = defaultRotationSpeed * 2;
        anarchyAcceleration = defaultAcceleration * 2;
        anarchyTimer = defaultTimer / 2;
    }
	
	// Update is called once per frame
	void Update () {
        AnarchyEnabled();
        distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance < triggerDistance)
        {
            toExplode = true;
            Explode();
        }
        else
        {
            if(!toExplode)
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
            destructTimer = defaultTimer;
        }
        else if (anarchyMode)
        {
            nav.speed = anarchySpeed;
            nav.angularSpeed = anarchyRotationSpeed;
            nav.acceleration = anarchyAcceleration;
            destructTimer = anarchyTimer;
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
