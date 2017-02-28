using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiShieldSeek : MonoBehaviour {

    // Use this for initialization
    //all AI needs using UnityEngine.AI;
    private GameObject player;//the target to seek (player)
    private NavMeshAgent nav;//the navmeshAgent for the current AI. All AIs need a navMeshAgent to work.

    public bool shieldActive;

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

    void Start()
    {
        //finding object with the tag "Player"
        player = GameObject.FindGameObjectWithTag("Player");
        nav = GetComponent<NavMeshAgent>();//getting the navMesh component of the AI
        shieldActive = true;

        defaultSpeed = nav.speed;
        defaultRotationSpeed = nav.angularSpeed;
        defaultAcceleration = nav.acceleration;

        anarchySpeed = defaultSpeed * 2;
        anarchyRotationSpeed = defaultRotationSpeed * 2;
        anarchyAcceleration = defaultAcceleration * 2;


    }

    // Update is called once per frame
    void Update()
    {
        // Used for seeking out and going to the player.
        // Based on NavMesh.
        // Changing speed and acceleration can be found in inspector.
        //nav.SetDestination(player.transform.position);//telling the AI to seek out and go to the player's location
        Seek();
        AnarchyEnabled();

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
}
