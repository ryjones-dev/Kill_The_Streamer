using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRotation : MonoBehaviour {

	private SpriteRenderer m_renderer;

	// Use this for initialization
	void Start () {
		m_renderer = this.GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		float rotate = transform.rotation.eulerAngles.y;
	
		m_renderer.flipY = !(rotate < 90 || rotate > 270);

		if (rotate > 180) {
			m_renderer.sortingOrder = 1;
		} else {
			m_renderer.sortingOrder = 3;
		}
	}
}
