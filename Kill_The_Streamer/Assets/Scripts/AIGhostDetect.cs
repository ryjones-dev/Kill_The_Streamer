using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGhostDetect : MonoBehaviour {

    // Use this for initialization
    //THIS SCRIPT IS FOR THE EMPTY CHILD OF THE GHOST WITH JUST A SPHERE COLLIDER
    //THIS IS FOR DETECTING RANGE OF GHOST

    private GameObject ghostBody;//get the parent
	void Start () {
        ghostBody = transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// For detecting to see if the player stays in the range of the ghost.
    /// </summary>
    /// <param name="coll"></param>
    void OnTriggerStay(Collider coll)
    {
        if(coll.CompareTag("Player"))
        {
            //if player is within the range, activate the charging.
            ghostBody.GetComponent<AiGhost>().ChargePlayer();
        }
    }

    /// <summary>
    /// If player is no longer in the ghost's range,
    /// go back to seeking out the player
    /// </summary>
    /// <param name="coll"></param>
    void OnTriggerExit(Collider coll)
    {
        if(coll.CompareTag("Player"))
        {
            ghostBody.GetComponent<AiGhost>().PlayerLeaves();
        }
    }
}
