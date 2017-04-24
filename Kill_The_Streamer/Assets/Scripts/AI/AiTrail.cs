using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiTrail : AIBase{

    //all AI needs using UnityEngine.AI;
    private NavMeshAgent nav;//the navmeshAgent for the current AI. All AIs need a navMeshAgent to work.
    private float speedModifier = 0.05f;

    private float trailTimer;
    private float resetTrailTime;

    //anarchy and regular values
    public bool anarchyMode = false;
    //default info
    public float defaultSpeed;
    private float defaultResetTrailTime;
    //anarchy info
    private const float c_ANARCHY_SPEED_MULT = 2.0f;
    public GameObject trailPrefab;

    // Use this for initialization
    public override void Start () {
        //finding object with the tag "Player"
        base.Start();
        nav = GetComponent<NavMeshAgent>();//getting the navMesh component of the AI

        resetTrailTime = 0.14f;
        trailTimer = resetTrailTime;

        //set random rotation
        float randZ = UnityEngine.Random.Range(0, 360);
        gameObject.transform.Rotate(new Vector3(0, randZ, 0));

        defaultSpeed = speedModifier;
        defaultResetTrailTime = resetTrailTime;
        
    }
	
	// Update is called once per frame
	protected override void Update () {
        
        base.Update();
        //keeps the salt flowing
		if (Time.timeScale != 0) {
			DropTrail ();

			Vector3 moveTo = m_transform.Forward.normalized * speedModifier;

			m_transform.Position = m_transform.Position + moveTo;
		}
    }

    public override void AILoop()
    {
        //more of a frantic dash around than a flee;
        //Flee();
    }

    public override void DealDamage()
    {
		Player.s_Player.TakeDamage(10, name, false);
    }

    public override void UpdateSpeed()
    {
        if (m_anarchyMode)
        {
            nav.speed = defaultSpeed * EnemyManager.SpeedMultiplier * c_ANARCHY_SPEED_MULT;
        }
        else
        {
            nav.speed = defaultSpeed * EnemyManager.SpeedMultiplier;
        }
    }

    public void DropTrail()
    {
		/*
        trailTimer -= Time.deltaTime;
        if (trailTimer <= 0)
        {
            //Instantiate(Resources.Load("Trail"), transform.position, transform.rotation);
            GameObject enemy = Instantiate<GameObject>(trailPrefab, transform.position, transform.rotation);
            trailTimer = resetTrailTime;
        }*/

		int texX = 0;
		int texY = 0;
		Vector2 pos = new Vector2(m_transform.Position.x, m_transform.Position.z);
		TrailHandler.s_instance.WorldToTexture(pos, out texX, out texY);
		TrailHandler.s_instance.DrawTrail (texX, texY);
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
        if (Vector3.Distance(gameObject.transform.position, m_target.FastTransform.Position) <= 1f)
        {
            //change direction
            Vector3 lookAtPosition = gameObject.transform.position - m_target.FastTransform.Position;
            gameObject.transform.LookAt(lookAtPosition);
        }
        
            //if distance to wall is small change direction
        if(currentHit.distance <= 0.2)
        {
            //change direction to offset hit off of wall in somewhat predictable direction
            Vector3 lookAtPosition = gameObject.transform.position + gameObject.transform.forward + currentHit.normal;
            gameObject.transform.LookAt(lookAtPosition);
        }
    }
}
