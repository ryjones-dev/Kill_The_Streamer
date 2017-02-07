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

    private PlayerLookingAtAI playerTargetting;//the script that tells if the player is looking at the AI


    public int timeResetSeek;//tells how long after the player stops looking at the enemy until it should go back to seeking (should be small. Just as a buffer time)


    void Start () {
        //finding object with the tag "Player"
        player = GameObject.FindGameObjectWithTag("Player");
        playerTargetting = player.GetComponent<PlayerLookingAtAI>();
        nav = GetComponent<NavMeshAgent>();//getting the navMesh component of the AI

    }
	
	// Update is called once per frame
	void Update () {
        if(playerTargetting.TargetAi==true)
        {
            Flee();
        }
        else
        {
            Seek();
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

}
