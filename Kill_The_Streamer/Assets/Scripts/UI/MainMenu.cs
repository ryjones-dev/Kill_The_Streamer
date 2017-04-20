using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
	}

	public void StartGame()
	{
		SceneManager.LoadScene ("Arena_1");
	}


	public void ClickQuit(){
		Application.Quit ();

	}

}
