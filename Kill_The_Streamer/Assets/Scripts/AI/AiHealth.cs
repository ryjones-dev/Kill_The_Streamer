using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiHealth : AIBase {

    // Use this for initialization
    //all AI needs using UnityEngine.AI;
    private NavMeshAgent nav;//the navmeshAgent for the current AI. All AIs need a navMeshAgent to work.
    public float speed = 3.0f;

    public GameObject healthPrefab;
    public override void Start () {
		base.Start ();
        nav = GetComponent<NavMeshAgent>();//getting the navMesh component of the AI
		UpdateSpeed();
    }

    // Update is called once per frame
    /*
	public override void Update () {
        base.Start();
	}
    */

    public override void AILoop()
    {
		Seek ();
    }


    public override void DealDamage()
    {
        Player.s_Player.TakeDamage(10000, name, true);
    }


    public override void UpdateSpeed()
    {
        nav.speed = speed;
    }

    public override void TakeDamage()
    {
        health--;
        if (health <= 0)
        {
            GameObject pack = (GameObject)Instantiate(healthPrefab, this.transform.position,Quaternion.Euler(90,0,0));
            pack.name = this.name;

            EnemyManager.DestroyEnemy(aiType, index);
            

        }
    }

	/// <summary>
	/// Used for seeking out and going to the player.
	/// Based on NavMesh.
	/// Changing speed and acceleration can be found in inspector.
	/// </summary>
	public void Seek()
	{
		nav.SetDestination(m_target.FastTransform.Position);//telling the AI to seek out and go to the player's location
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
