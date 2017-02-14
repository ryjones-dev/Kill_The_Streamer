using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAi : MonoBehaviour {

    // Use this for initialization
    private GameObject player;

    public int shieldHP = 6;//the amount of hits a shield can take
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(player.transform.position);
	}
}
