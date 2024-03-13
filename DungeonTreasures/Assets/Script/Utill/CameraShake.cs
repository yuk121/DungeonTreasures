using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    float m_duration= 1f;
    float m_power = 0.5f;
    float m_checkTime = 0f;
    bool m_isStart;

    Vector3 m_orginPos;

    public void ShakeCamera(float duration, float power)
    {
        m_checkTime = 0f;
        m_isStart = true;
        m_duration = duration;
        m_power = power;
    }
    void Start()
    {
        m_orginPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_isStart)
        {
            m_checkTime += Time.deltaTime;
            var dir = Random.insideUnitCircle;
            transform.position = new Vector3(dir.x, dir.y, m_orginPos.z) * m_power;

            if(m_checkTime > m_duration)
            {
                transform.position = m_orginPos;
                m_checkTime = 0f;
                m_isStart = false;
            }

        }
    }
}
