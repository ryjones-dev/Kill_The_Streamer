using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour {

	public static int s_points;
	public Text myText;
	public string preface = "";

	// Use this for initialization
	void Start () {
		myText = this.GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		myText.text = preface + s_points;
	}
}
