using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumParticleSpawner : MonoBehaviour {

    public static VacuumParticleSpawner s_Instance;
    public ParticleSystem m_ParticleSystem;
    public bool active;

    // Use this for initialization
    void Start () {
        s_Instance = this;
        m_ParticleSystem = this.GetComponent<ParticleSystem>();
	}

    void LateUpdate()
    {
        
        if(active)
        {
            active = false;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
