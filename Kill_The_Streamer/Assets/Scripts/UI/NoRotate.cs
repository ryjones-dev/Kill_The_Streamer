using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoRotate : MonoBehaviour {
	
	public static Quaternion worldRotation = Quaternion.Euler (new Vector3 (90.0f, 0, 0));

	// Update is called once per frame
	void LateUpdate () {
		this.transform.rotation = worldRotation;
	}
}
