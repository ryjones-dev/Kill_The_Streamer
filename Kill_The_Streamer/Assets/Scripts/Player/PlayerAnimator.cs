using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {

	private Animator m_animator;
	private PlayerController m_parent;

	void Start(){
		m_animator = this.GetComponentInChildren<Animator> ();
		m_parent = this.GetComponentInParent<PlayerController> ();
	}

	// Update is called once per frame
	void Update () {
		//Determine direction
		float rotate = transform.rotation.eulerAngles.y;

		if (rotate > 337.5f || rotate < 22.5f) {
			m_animator.SetInteger ("direction", 0);
		} else if (rotate < 67.5f) {
			m_animator.SetInteger ("direction", 1);
		} else if (rotate < 112.5f) {
			m_animator.SetInteger ("direction", 2);
		} else if (rotate < 157.5f) {
			m_animator.SetInteger ("direction", 3);
		} else if (rotate < 202.5f) {
			m_animator.SetInteger ("direction", 4);
		} else if (rotate < 247.5f) {
			m_animator.SetInteger ("direction", 5);
		} else if (rotate < 292.5f) {
			m_animator.SetInteger ("direction", 6);
		} else {
			m_animator.SetInteger ("direction", 7);
		}

		//Determine state
		if (m_parent.velocity.sqrMagnitude <= 0.01f) { //Not moving
			m_animator.SetInteger ("state", 0);

		} else if (m_parent.dash) { //Moving but dashing
			m_animator.SetInteger ("state", 2);
			if (m_parent.velocity.x > 0) {
				m_animator.SetInteger ("direction", 0);
			} else {
				m_animator.SetInteger ("direction", 7);
			}
		} else { //Moving but not dashing
			m_animator.SetInteger ("state", 1);
		}

	}
		
			
}
