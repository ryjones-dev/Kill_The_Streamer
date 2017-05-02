using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiTrail : AIBase{

    //all AI needs using UnityEngine.AI;

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

			Vector3 moveTo = m_transform.Forward.normalized * EnemyManager.SpeedMultiplier * 4 * Time.deltaTime;

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
		TrailHandler.s_instance.DrawTrail (texX, texY, name);
    }


	void OnCollisionEnter(Collision col){

		if(col.collider.CompareTag("Terrain")){
			float randZ = UnityEngine.Random.Range(0, 360);
			gameObject.transform.Rotate(new Vector3(0, randZ, 0));
		}
			

	}

	void OnCollisionStay(Collision col){
		if(col.collider.CompareTag("Terrain")){
			float randZ = UnityEngine.Random.Range(0, 360);
			gameObject.transform.Rotate(new Vector3(0, randZ, 0));
		}
	}
}
