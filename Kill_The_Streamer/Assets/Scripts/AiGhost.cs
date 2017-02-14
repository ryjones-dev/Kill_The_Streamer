using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiGhost : MonoBehaviour {

    // Use this for initialization


    public float startSpeed =2.0f;//the starting slow speed for the ghost
    public float chargeSpeed=9.0f;//the charging fast speed for the ghost once player is spotted
    public float rotationSpeed = 10.0f;//how fast the ghost will rotate

    public float pauseTimer=2.0f;//the amount of time the ghost will pause for
    private float currTimer=0.0f;//the current timer the ghost

    private GameObject player; //the player

    private Vector3 chargeLoc;//the location the ghost will charge to once timer is over
	private bool seekOut;//seek out and chase player at slow speed
    private bool seekSpot;//tells if ghost has found where to charge
    private bool toCharge;//tells if ghost is ready to charge

	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        toCharge = false;
        seekSpot = false;
		seekOut = true;

	}
	
	// Update is called once per frame
	void Update () {
        //if the player is out of range, always seek out slowly the player
		if (seekOut == true) {
			Seeking (player.transform.position, startSpeed);
		}
		
		
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

    /// <summary>
    /// Once the ghost spots player, pause for a few seconds.
    /// Pick the location of the player.
    /// When the time hits the timer, that location is the place to charge to.
    /// If enemy enters within a distance of that point, reset.
    /// </summary>
	public void Pause(){
        float dist;
		if (currTimer <= pauseTimer) {
			currTimer += Time.deltaTime;
            chargeLoc = player.transform.position;
        }
        //if enemy is ready.... CHARGE!
		else if (currTimer >= pauseTimer) {
			toCharge = true;
		}
        //check to see distance of enemy and the place to charge to
        dist = Vector3.Distance(chargeLoc, transform.position);
        if (dist <= 1)
        {
            toCharge = false;
            currTimer = 0;
            seekSpot = false;
        }
   
    }

    /// <summary>
    /// For ghost charging the player.
    /// The ghost pauses, takes the current location of the player,
    /// and then charges. Once it is in the range of player it restarts the cycle.
    /// If player goes out of range, go back to seeking.
    /// </summary>
	public void ChargePlayer ()
	{
        seekOut = false;
            Pause ();
			if (toCharge == true) {
                if(seekSpot==false)
                {
                    chargeLoc = player.transform.position;
                    seekSpot = true;
                }
                else if(seekSpot==true)
                {
                    Seeking(chargeLoc, chargeSpeed);
                }
			}
	}

    /// <summary>
    /// If player goes out of the range of the ghost, go back to seeking until
    /// player is back within range.
    /// </summary>
   public void PlayerLeaves()
    {
            seekOut = true;
            toCharge = false;
            seekSpot = false;
            currTimer = 0;
    }

}
