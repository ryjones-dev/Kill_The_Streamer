using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAi : MonoBehaviour {

    // Use this for initialization
    public int shieldHP = 6;//the amount of hits a shield can take

   public bool shieldActive=true;//tells if the shield is up
	void Start () {
        //double checking to make sure no one set the shield hp to something lower than 2 at the startup
        if(shieldHP<=2)
        {
            shieldHP = 3;
        }
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


}
