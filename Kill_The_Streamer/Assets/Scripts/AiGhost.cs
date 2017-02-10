using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public enum ghostStates
    {
        slowSeek,
        chargeSeek,
        standStill
    }
public class AiGhost : MonoBehaviour {

    // Use this for initialization


    public float startSpeed =2.0f;//the starting slow speed for the ghost
    public float chargeSpeed=9.0f;//the charging fast speed for the ghost once player is spotted
    public float rotationSpeed = 10.0f;//how fast the ghost will rotate

    public float pauseTimer=2000.0f;//the amount of time the ghost will pause for
    private float currTimer=0.0f;//the current timer the ghost

    private GameObject player; //the player

    private Vector3 chargeLoc;//the location the ghost will charge to once timer is over

    private ghostStates ghostMovement;
    private bool toCharge;

	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

   /// <summary>
   /// Moving the ghost to a location of the player(will change based on what state it is in.
   /// </summary>
   /// <param name="p_goTo">The position that the ghost will go to</param>
   /// <param name="p_speed">The speed that the ghost will go to that position at</param>
    public void Seeking(Vector3 p_goTo, float p_speed)
    {
        Vector3 ghostPosition = transform.position;
        //rotating the ghost
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(p_goTo - transform.position), rotationSpeed * Time.deltaTime);

        //moving the ghost
        ghostPosition += this.transform.forward * p_speed * Time.deltaTime;
        transform.position = ghostPosition;
    }


}
