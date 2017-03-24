using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiTrail : MonoBehaviour {

    //all AI needs using UnityEngine.AI;
    private GameObject player;//the target to seek (player)
    private NavMeshAgent nav;//the navmeshAgent for the current AI. All AIs need a navMeshAgent to work.
    public float speedModifier = 0.05f;

    private float trailTimer;
    private float resetTrailTime;

    //anarchy and regular values
    public bool anarchyMode = false;
    //default info
    public float defaultSpeed;
    private float defaultResetTrailTime;
    //anarchy info
    private float anarchySpeed;
    private float anarchyResetTrailTime;

    // Use this for initialization
    void Start () {
        //finding object with the tag "Player"
        player = GameObject.FindGameObjectWithTag("Player");
        nav = GetComponent<NavMeshAgent>();//getting the navMesh component of the AI
        Instantiate(Resources.Load("Trail"), transform.position, transform.rotation);


        resetTrailTime = 0.14f;
        trailTimer = resetTrailTime;

        //set initialPosition
        NavMeshHit hit;
        nav.FindClosestEdge(out hit);

        defaultSpeed = speedModifier;
        defaultResetTrailTime = resetTrailTime;

        anarchySpeed = defaultSpeed * 2;
        anarchyResetTrailTime = defaultResetTrailTime / 2;
    }
	
	// Update is called once per frame
	void Update () {
        AnarchyEnabled();
        //more of a frantic dash around than a flee;
        Flee();
        //keeps the salt flowing
        DropTrail();
    }

    /// <summary>
    /// This is the for the speeds and values during anarchy mode.
    /// </summary>
    public void AnarchyEnabled()
    {
        if (anarchyMode == false)
        {
            speedModifier = defaultSpeed;
            resetTrailTime = defaultResetTrailTime;
        }
        else if (anarchyMode)
        {
            speedModifier = anarchySpeed;
            resetTrailTime = anarchyResetTrailTime;
        }
    }

    public void DropTrail()
    {
        trailTimer -= Time.deltaTime;
        if (trailTimer <= 0)
        {
            Instantiate(Resources.Load("Trail"), transform.position, transform.rotation);
            trailTimer = resetTrailTime;
        }
    }
    //prefered way of working
    //Start: find closest edge then run away from that
    //if within 1 unit of another edge update position to run away from that edge
    //repeat
    public void Flee()
    {
        NavMeshHit currentHit;
        nav.FindClosestEdge(out currentHit);

        //if it gets too close to the player, change direction
        if (Vector3.Distance(gameObject.transform.position, player.transform.position) <= 1f)
        {
            //change direction
            Vector3 lookAtPosition = gameObject.transform.position - player.transform.position;
            gameObject.transform.LookAt(lookAtPosition);
        }
        
            //if distance to wall is small change direction
        if(currentHit.distance <= 0.2)
        {
            //change direction to offset hit off of wall in somewhat random direction
            Vector3 lookAtPosition = gameObject.transform.position + -gameObject.transform.right + currentHit.normal;
            gameObject.transform.LookAt(lookAtPosition);

            //initialHitPosition = currentHit.position;
        }

        Vector3 moveTo = gameObject.transform.forward.normalized * speedModifier;

        gameObject.transform.position = gameObject.transform.position + moveTo;
    }
}
