using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tornado : MonoBehaviour {
    public static GameObject s_Object;
    public static Tornado s_Tornado;

    RectTransform myRect;
    float rotation = 0.0f;
    public float isAlive = 0;
	// Use this for initialization
	void Start () {
        myRect = this.GetComponent<RectTransform>();
        s_Object = this.gameObject;
        s_Tornado = this;
        gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        rotation -= 360 *Time.deltaTime;
        if(rotation < 0.0f)
        {
            rotation += 360.0f;
        }
        myRect.localRotation = Quaternion.Euler(0, 0, rotation);
        isAlive-= Time.deltaTime;
        if(isAlive <= 0)
        {
            this.gameObject.SetActive(false);
        }
	}
}
