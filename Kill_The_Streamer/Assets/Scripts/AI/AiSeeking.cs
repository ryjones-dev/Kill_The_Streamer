using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiSeeking : MonoBehaviour {

    // Use this for initialization
    //all AI needs using UnityEngine.AI;
    private GameObject player;//the target to seek (player)
    private NavMeshAgent nav;//the navmeshAgent for the current AI. All AIs need a navMeshAgent to work.
    private ShieldAi shield;//the air to detect if the shield is up to follow

    public float distanceActive =12.0f;//will give the distance that it will be able to go to another shield

 
    private float maxDist;//the distance behind the leader the UI will go to
    private bool splitOff;//tells if the UI should split off away from the shielder and seek out the player instead

    private float closeShield;//gives the closest shield dist

    public float distanceAwayPlayer;//gives the distance away from player before the seeks will seek out the player and ignore shielder


	void Start () {
        //finding object with the tag "Player"
        player = GameObject.FindGameObjectWithTag("Player");
        nav = GetComponent<NavMeshAgent>();//getting the navMesh component of the AI
        closeShield = 100.00f;
        //give seeker a random range to be away from the leader
        maxDist = Random.Range(1.5f, 5.0f);
        splitOff = true;//seek out player

	}
	
	// Update is called once per frame
	void Update () {
        // Used for seeking out and going to the player.
        // Based on NavMesh.
        // Changing speed and acceleration can be found in inspector.
        //nav.SetDestination(player.transform.position);//telling the AI to seek out and go to the player's location
        
        if (Vector3.Distance(this.transform.position, player.transform.position) <= distanceAwayPlayer)
        {
            Seek();
        }
        else
        {
            ClosestShield();
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
    /// This is for checking and finding the closest shield and if so then see it and follow until close enough to player.
    /// Will get the closest enabled shield. If not, continue seeking.
    /// </summary>
    public void ClosestShield()
    {
        //get all shielders in level
        GameObject[] shieldsInLevel = GameObject.FindGameObjectsWithTag("Shielder");
        //make sure they are enabled
        if (shieldsInLevel.Length == 0)
        {
            //splitOff = true;
            Seek();
            return;
        }

        //get some random large distance for now
        float distance = 200.0f;

        //have it store some gameobjects and the "leader"
        GameObject shieldGameObject=null;//the gameobject of the shield in order to detect which one is closest

        NavMeshAgent leaderNav=null;//the navmesh for the leader (shield)
        //check the shields
        for (int i =0; i < shieldsInLevel.Length; i++)
        {
            //are the shields active?
            if(shieldsInLevel[i].GetComponent<AiShieldSeek>().shieldActive)
            {
                //get distance from position to shielder
                distance = Vector3.Distance(this.transform.position, shieldsInLevel[i].transform.position);
                //is it close enough?
                if (distance <= distanceActive)
                {
                    //store the shield that it is the closest to
                    if (distance < closeShield)
                    {
                        //Debug.Log(distance);
                        shieldGameObject = shieldsInLevel[i];
                        leaderNav = shieldsInLevel[i].gameObject.GetComponent<NavMeshAgent>();
                        //splitOff = false;
                        closeShield = distance;
                        Debug.Log(closeShield);
                    }
                }

            }
        }
        //has there not been a closest shield found?
        if(shieldGameObject ==false && leaderNav ==false)
        {
            closeShield = 100.0f;
            //splitOff = true;
            Seek();
            //return;
        }
        //seek out closest shield and leader follow it
        else
        {
            Debug.Log(shieldGameObject.transform.position);
            nav.SetDestination(LeaderFollowing(shieldGameObject,leaderNav));
        }
    }

    /// <summary>
    /// For seeking and going behind the closest Shielder object.
    /// </summary>
    /// <param name="shielderObject">The closest shielder in the level</param>
    /// <param name="leaderAgent">The navmeshagent of the closest Shielder</param>
    /// <returns></returns>
   private Vector3 LeaderFollowing(GameObject shielderObject, NavMeshAgent leaderAgent)
    {
        //float dist = Vector3.Distance(this.transform.position, shieldGameObject.transform.position);
        Vector3 leaderPos = shielderObject.transform.position + (-leaderAgent.velocity).normalized * maxDist;
        return leaderPos;

        
    }

}
