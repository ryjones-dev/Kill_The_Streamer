using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void goBack(){
        Debug.Log("Goback");
		SceneManager.UnloadSceneAsync ("Credits");
		//SceneManager.LoadScene ("MainMenu");


	}
}
