using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps track of the transform for an object.  For some reason or other I saw a significant performance gain
/// when keeping track of the transform... so this does that.
/// 
/// Calling transform.position is also an expensive call so I keep track of position every frame here.
/// 
/// Note that this class in Script Execution Order always goes first, since it's necessary to update appopriately.
/// </summary>
public class FastTransform : MonoBehaviour {

    private Transform m_trans;
    private Vector3 m_position;

    public Transform Trans
    {
        get
        {
            return m_trans;
        }
    }

    public Vector3 Position
    {
        get
        {
            return m_position;
        }
        set
        {
            m_position = value;
            m_trans.position = value;
        }
    }

	// Use this for initialization
	void Start () {
        this.m_trans = this.transform;
        this.m_position = this.m_trans.position;
	}
	
	// Update is called once per frame
	void Update () {
        this.m_position = this.m_trans.position;
	}
}
