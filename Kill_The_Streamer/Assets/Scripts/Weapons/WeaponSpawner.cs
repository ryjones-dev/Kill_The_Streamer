using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour {

	public Weapon m_spawnedWeapon = null;
	public float m_timerReset;
	public float m_timer;
	public TextMesh m_textRenderer;

	// Use this for initialization
	void Start () {
		m_timer = m_timerReset;
		m_textRenderer = this.GetComponentInChildren<TextMesh> ();
	}
	
	// Update is called once per frame
	void Update () {
		m_timer -= Time.deltaTime;
		m_textRenderer.text = "" + Mathf.CeilToInt (m_timer);

		if (m_spawnedWeapon != null) {
			

			if (m_spawnedWeapon.m_held) {
				m_spawnedWeapon = null;
				return;
			}

			if (m_timer > 10.0f) {
				m_spawnedWeapon.m_arenaTimer = 10.0f;
			}

		} else {

			if (m_timer < 0) {
				
				m_spawnedWeapon = Tool_WeaponSpawner.s_instance.SpawnWeapon (this.transform.position).GetComponent<Weapon>();
				m_timer = m_timerReset;
			}
		}
	}
}
