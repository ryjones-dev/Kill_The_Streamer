using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAi : MonoBehaviour {

    // Use this for initialization
    private GameObject player;
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(player.transform.position);
	}
}
