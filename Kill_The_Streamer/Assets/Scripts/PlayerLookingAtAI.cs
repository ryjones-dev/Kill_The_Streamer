using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerLookingAtAI : MonoBehaviour {

    // Use this for initialization
    private bool targetAi;//tells if the player is currently looking at the AI that is supposed to avoid the player if he/she is looking at it

    public float rangeMult = 5.0f;//multiplying the field of view for the player. 

	void Start () {
        targetAi = false;
	}

    public bool TargetAi
    {
        get { return targetAi; }
    }
	
	// Update is called once per frame
	void Update () {
        CheckPlayerLooking();
	}

    /// <summary>
    /// Checks to see if the player is looking at the AI
    /// If it is not, then will set a bool to false to seek.
    /// If player is, then run away from the player.
    /// </summary>
    public void CheckPlayerLooking()
    {
        RaycastHit hit;
        //getting the forward vector of the player
        Vector3 leftAngle = Quaternion.AngleAxis(-45, transform.up) * transform.forward;
        Vector3 rightAngle = Quaternion.AngleAxis(45, transform.up) * transform.forward;
        Debug.DrawLine(transform.position + rightAngle, transform.position);
        Debug.DrawLine(transform.position + leftAngle, transform.position);

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("BooAi");

        for(int i = 0; i < enemies.Length; ++i)
        {
            Vector3 playerToEnemy = enemies[0].transform.position - transform.position;
            bool positiveLeft = (Vector3.Dot(leftAngle, playerToEnemy) >= 0);
            bool positiveRight = (Vector3.Dot(rightAngle, playerToEnemy) >= 0);
            if(positiveLeft && positiveRight)
            {
                //runaway

            }

        }


        //test to see if the player is facing an object
        /*
        if (Physics.Raycast(transform.position, forwardsPlayer, out hit, 20.0f ))
        {
            //if the rayCast hits the Ai that needs to test it is hit, then make sure it is fine
            if(hit.collider.gameObject.tag=="BooAi")
            {
                targetAi = true;
            }
            else
            {
                targetAi = false;
            }
        }
        */
    }
}
