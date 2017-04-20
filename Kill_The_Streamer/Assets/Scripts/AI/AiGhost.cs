using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiGhost : AIBase {

    // Use this for initialization


    public float startSpeed =2.0f;//the starting slow speed for the ghost
    public float chargeSpeed=9.0f;//the charging fast speed for the ghost once player is spotted
    public float rotationSpeed = 10.0f;//how fast the ghost will rotate

    public float pauseTimer=2.0f;//the amount of time the ghost will pause for
    private float currTimer=0.0f;//the current timer the ghost

    private Vector3 chargeLoc;//the location the ghost will charge to once timer is over
    private bool seekSpot;//tells if ghost has found where to charge
    private bool toCharge;//tells if ghost is ready to charge

    private float playerDistance;//the distance between the ghost and the actual player object

    public float triggerDistance = 6.0f;//the distance at which the player will be within the ghost's radius

    //keeping track of the defaults for each
    public float defaultSpeed;
    public float defaultChargeSpeed;
    public float defaultTimer;
    public float defaultRotationSpeed;

    //anarchy speed for each
    private const float c_ANARCHY_SPEED_MULT = 2;
    private const float c_ANARCHY_ROTATION_MULT = 2;
    private const float c_ANARCHY_TIMER_MULT = 0.5f;
    private const float c_ANARCHY_CHARGE_MULT = 2;

    public override void Start () {
        base.Start();
        toCharge = false;
        seekSpot = false;
        defaultSpeed = startSpeed;
        defaultChargeSpeed = chargeSpeed;
        defaultTimer = pauseTimer;
        defaultRotationSpeed = rotationSpeed;
        

}

    protected override void Update()
    {
        base.Update();
        //get the distance between ghost and player
        playerDistance = (m_target.FastTransform.Position - m_transform.Position).sqrMagnitude;
        //if player is not in distance, seek it out slowly
        if (playerDistance > triggerDistance * triggerDistance)
        {
            Seeking(m_target.FastTransform.Position, startSpeed);
            PlayerLeaves();
        }
        //if player is within the triggerdistance, activate charge!
        else
        {
            ChargePlayer();
        }
    }
    public override void AILoop()
    {
        
    }

    public override void UpdateSpeed()
    {
        if (m_anarchyMode)
        {
            startSpeed = defaultSpeed * EnemyManager.SpeedMultiplier * c_ANARCHY_SPEED_MULT;
            rotationSpeed = defaultRotationSpeed * EnemyManager.SpeedMultiplier * c_ANARCHY_ROTATION_MULT;
            pauseTimer = defaultTimer * c_ANARCHY_TIMER_MULT / EnemyManager.SpeedMultiplier;
            chargeSpeed = defaultChargeSpeed * EnemyManager.SpeedMultiplier * c_ANARCHY_CHARGE_MULT;
        }
        else
        {
            startSpeed = defaultSpeed * EnemyManager.SpeedMultiplier;
            rotationSpeed = defaultRotationSpeed * EnemyManager.SpeedMultiplier;
            pauseTimer = defaultTimer / EnemyManager.SpeedMultiplier;
            chargeSpeed = defaultChargeSpeed * EnemyManager.SpeedMultiplier;
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
            chargeLoc = m_target.FastTransform.Position;
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
            Pause ();
			if (toCharge == true) {
                if(seekSpot==false)
                {
				Vector2 variance = UnityEngine.Random.insideUnitCircle;
				chargeLoc = m_target.FastTransform.Position +  5.0f * (new Vector3(variance.x, 0, variance.y));
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
            toCharge = false;
            seekSpot = false;
            currTimer = 0;
    }

    public override void DealDamage()
    {
		Player.s_Player.TakeDamage(1000, name, true);
    }

	void OnTriggerEnter(Collider col){
		if(col.CompareTag("PlayerHitbox")){
			DealDamage();
		}
	}
	void OnTriggerStay(Collider col){
		if(col.CompareTag("PlayerHitbox")){
			DealDamage();
		}
	}
}
