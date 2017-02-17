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

    public float maxDist;//the max distance away from the "leader"

	void Start () {
        //finding object with the tag "Player"
        player = GameObject.FindGameObjectWithTag("Player");
        nav = GetComponent<NavMeshAgent>();//getting the navMesh component of the AI
		
	}
	
	// Update is called once per frame
	void Update () {
        // Used for seeking out and going to the player.
        // Based on NavMesh.
        // Changing speed and acceleration can be found in inspector.
        //nav.SetDestination(player.transform.position);//telling the AI to seek out and go to the player's location
        Seek();
        //ClosestShield();

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
        /*
        //check to make sure there is atleast one shield in the scene
        if(GameObject.FindGameObjectsWithTag("Shield")!=null)
        {
            GameObject[] shieldsInLevel = GameObject.FindGameObjectsWithTag("Shield");
            float closeDist = 1000.0f;//setting it to some arbitrarily high num to then change

            foreach(GameObject closeObject in shieldsInLevel)
            {
                float dist = Vector3.Distance(this.transform.position, closeObject.transform.position);

                if(dist < closeDist)
                {
                    closeDist = dist;
                    shieldGameObject = closeObject;
                }

            }
            nav.SetDestination(shieldGameObject.transform.position);

        }
        */
        

    }

    public void LeaderFollowing()
    {
        //float dist = Vector3.Distance(this.transform.position, shieldGameObject.transform.position);

        
    }

}
