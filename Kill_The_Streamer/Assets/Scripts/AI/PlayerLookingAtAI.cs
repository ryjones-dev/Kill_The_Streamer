using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerLookingAtAI : MonoBehaviour {

    // Use this for initialization

    public float rangeView = 10.0f;//the range of view that the player can see to

	void Start () {
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
        Debug.DrawLine(transform.position + rightAngle*10, transform.position,Color.cyan);
        Debug.DrawLine(transform.position + leftAngle*10, transform.position,Color.cyan);

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("BooAi");

       // bool playerForwardRay = Physics.Raycast(transform.position + Vector3.up)
        for(int i = 0; i < enemies.Length; ++i)
        {
            Vector3 playerToEnemy = enemies[i].transform.position - transform.position;
            bool positiveLeft = (Vector3.Dot(leftAngle, playerToEnemy) >= 0);
            //Debug.DrawLine(transform.position, Vector3.Dot(leftAngle, playerToEnemy));
            bool positiveRight = (Vector3.Dot(rightAngle, playerToEnemy) >= 0);
            if(positiveLeft && positiveRight)
            {
                //get distance
                float distance = Vector3.Distance(transform.position, enemies[i].transform.position);
                if(distance <= rangeView)
                {
                    //runaway
                    enemies[i].GetComponent<AiSeekFlee>().InPlayerSight = true;
                }
                else
                {
                    enemies[i].GetComponent<AiSeekFlee>().InPlayerSight = false;
                }
            }
            else
            {
                enemies[i].GetComponent<AiSeekFlee>().InPlayerSight = false;
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
