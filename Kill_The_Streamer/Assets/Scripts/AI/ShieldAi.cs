using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAi : AIBase{

    // Use this for initialization

   public bool shieldActive=true;//tells if the shield is up
	void Start () {

	}

    //property for shieldActive
    public bool ShieldActive
    {
        get { return shieldActive; }
    }
	
	// Update is called once per frame
	void Update () {
       // gameObject.SetActive(shieldActive);
      
	}

    public override void TakeDamage()
    {
        health--;
        if(health <= 0)
        {
            shieldActive = false;
            EnemyManager.DestroyEnemy(aiType, index);
        }
    }



}
