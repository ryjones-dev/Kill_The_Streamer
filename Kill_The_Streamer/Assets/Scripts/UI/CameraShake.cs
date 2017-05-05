using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake
{
    public float intensity;
    public float time;

    public Shake(float intensity, float time)
    {
        this.intensity = intensity;
        this.time = time;
    }
}

public class CameraShake : MonoBehaviour {

    public Vector3 m_startingPosition;
    public float m_shakeIntensity;

    public static CameraShake s_instance;
    public const float RATE_INTENSITY_ATROPHY = 0.9f;

    public List<Shake> m_currentShakes;

	// Use this for initialization
	void Start () {
        m_currentShakes = new List<Shake>();
        m_startingPosition = this.transform.localPosition;
        m_shakeIntensity = 0;
        s_instance = this;
	}
	
	// Update is called once per frame
	void Update () {
        float intensity = 0.0f;
        if (m_currentShakes.Count > 0)
        {
            for (int i = m_currentShakes.Count; i-- != 0;)
            {
                intensity += m_currentShakes[i].intensity;
                m_currentShakes[i].time -= Time.deltaTime;
                if(m_currentShakes[i].time <= 0)
                {
                    m_currentShakes.RemoveAt(i);
                }
            }
        }
        
        if (m_shakeIntensity > 0.01f)
        {
            intensity += m_shakeIntensity;
            m_shakeIntensity *= RATE_INTENSITY_ATROPHY;
        }
        else
        {
            m_shakeIntensity = 0;
        }

        if(intensity != 0.0f)
        {
            if(intensity > 10.0f)
            {
                intensity = 10.0f;
            }
            Vector3 newPosition = m_startingPosition;
            newPosition.x += Random.Range(-intensity, intensity);
            newPosition.y += Random.Range(-intensity, intensity);

            this.transform.localPosition = newPosition;
        }
        else
        {
            this.transform.localPosition = m_startingPosition;
        }
	}

    public static void AddShake(Shake newShake)
    {
        s_instance.m_currentShakes.Add(newShake);
    }

    public static void AddFadeShake(float impulse)
    {
        s_instance.m_shakeIntensity += impulse;

    }
}
