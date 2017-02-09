using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiGhost : MonoBehaviour {

    // Use this for initialization

    public float startSpeed =2.0f;//the starting slow speed for the ghost
    public float chargeSpeed=9.0f;//the charging fast speed for the ghost once player is spotted

    public float pauseTimer=2000.0f;//the amount of time the ghost will pause for
    private float currTimer=0.0f;//the current timer the ghost

    private GameObject player; //the player

    private Vector3 chargeLoc;//the location the ghost will charge to once timer is over


	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Seeking(Vector3 p_goTo)
    {

    }


}
