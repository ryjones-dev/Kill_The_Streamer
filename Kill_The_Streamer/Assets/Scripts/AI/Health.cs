using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    public int healthIncrease = 100;

	// Use this for initialization
	void Start () {
		//Player.s_Player
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider coll)
    {
        if(coll.tag=="Player")
        {
            Player.s_Player.m_health += healthIncrease;
        }
    }
}
