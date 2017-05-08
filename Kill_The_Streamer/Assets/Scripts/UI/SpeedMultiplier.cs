using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedMultiplier : MonoBehaviour {
	public Text myText;
	// Use this for initialization
	void Start () {
		this.myText = this.GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		myText.text = "Intensity: " + EnemyManager.SpeedMultiplier.ToString ("n2") + "x";
	}
}
