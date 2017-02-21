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
    private GameObject shieldGameObject;//the gameobject of the shield in order to detect which one is closest
    private NavMeshAgent leaderNav;//the navmesh for the leader (shield)
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
        ClosestShield();
        if (splitOff == true)
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

    /// <summary>
    /// This is for checking and finding the closest shield and if so then see it and follow until close enough to player
    /// </summary>
    public void ClosestShield()
    {
        
        //check to make sure there is atleast one shield in the scene
        if(GameObject.FindGameObjectsWithTag("Shielder")!=null)
        {
            GameObject[] shieldsInLevel = GameObject.FindGameObjectsWithTag("Shielder");
            float distance = 200.0f;
            float playerDist = Vector3.Distance(this.transform.position, player.transform.position);
            //Debug.Log(playerDist);
            if (playerDist >= distanceAwayPlayer)
            {
               // Debug.Log("Away from player");
                for (int i =0; i < shieldsInLevel.Length;i++)
                {
                    distance = Vector3.Distance(this.transform.position,shieldsInLevel[i].transform.position);
                    //Debug.Log("distance at:" + i + " dist "+distance);

                    if (distance <= 15.0f && shieldsInLevel[i].activeSelf==true)
                    {

                        if (distance < closeShield)
                        {
                            //Debug.Log(distance);
                            shieldGameObject = shieldsInLevel[i];
                            leaderNav = shieldsInLevel[i].gameObject.GetComponent<NavMeshAgent>();
                            splitOff = false;
                            closeShield = distance;
                        }
                    }
                    else
                    {
                        splitOff = true;
                    }

                }
                if(distance > 15.0f)
                {
                    closeShield = 200.0f;
                    splitOff = true;
                }
            }
            else
            {
                splitOff = true;
            }
            if (splitOff == false)
            {
                nav.SetDestination(LeaderFollowing());
            }
            //Debug.Log(splitOff);

        }

       // Collider[] shieldColliders = Physics.OverlapSphere(transform.position, 15, 1 << LayerMask.NameToLayer("Shield"));

    }

   private Vector3 LeaderFollowing()
    {
        //float dist = Vector3.Distance(this.transform.position, shieldGameObject.transform.position);
        Vector3 leaderPos = shieldGameObject.transform.position + (-leaderNav.velocity).normalized * maxDist;
        return leaderPos;

        
    }

}
