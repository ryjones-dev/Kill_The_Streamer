using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressQScript : MonoBehaviour {

	bool isActive = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!isActive) {
			gameObject.SetActive (false);
		}

		if (Input.GetKeyDown (Settings.switchWeaponKeyCode)) {
			isActive = false;
			gameObject.SetActive (false);
		}
	}
}
