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

    private Quaternion m_leftAngle = Quaternion.AngleAxis(-45, Vector3.up);
    private Quaternion m_rightAngle = Quaternion.AngleAxis(45, Vector3.up);

    private Vector3 m_leftVisionAngle;
    private Vector3 m_rightVisionAngle;

    public Vector3 LeftVisionAngle { get { return m_leftVisionAngle; } }
    public Vector3 RightVisionAngle { get { return m_rightVisionAngle; } }

    // Use this for initialization
    void Start () {
        this.m_trans = this.transform;
        this.m_position = this.m_trans.position;
	}
	
	// Update is called once per frame
	void Update () {
        this.m_position = this.m_trans.position;

        m_leftVisionAngle = m_leftAngle * this.m_trans.forward;
        m_rightVisionAngle = m_rightAngle * this.m_trans.forward;
        //Debug.DrawLine(this.position + m_leftVisionAngle * 10, this.position, Color.cyan);
        //Debug.DrawLine(this.position + m_rightVisionAngle * 10, this.position, Color.cyan);
    }
}
