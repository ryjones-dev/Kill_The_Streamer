using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targetable : MonoBehaviour
{
    protected Quaternion m_leftAngle = Quaternion.AngleAxis(-45, Vector3.up);
    protected Quaternion m_rightAngle = Quaternion.AngleAxis(45, Vector3.up);

    protected Vector3 m_leftVisionAngle;
    protected Vector3 m_rightVisionAngle;

    public Vector3 LeftVisionAngle { get { return m_leftVisionAngle; } }
    public Vector3 RightVisionAngle { get { return m_rightVisionAngle; } }

    protected FastTransform m_transform;
    public FastTransform FastTransform { get { return m_transform; } }

    protected virtual void Awake()
    {
        m_transform = GetComponent<FastTransform>();
    }

    protected virtual void Update()
    {
        m_leftVisionAngle = m_leftAngle * m_transform.Forward;
        m_rightVisionAngle = m_rightAngle * m_transform.Forward;
        Debug.DrawLine(m_transform.Position + m_leftVisionAngle * 10, m_transform.Position, Color.cyan);
        Debug.DrawLine(m_transform.Position + m_rightVisionAngle * 10, m_transform.Position, Color.cyan);
    }
}
