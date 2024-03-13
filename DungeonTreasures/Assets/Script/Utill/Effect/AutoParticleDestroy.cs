using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoParticleDestroy : MonoBehaviour
{
    public enum EDsetroyType
    {
        Destroy = 0,
        Inactive
    }
    public float m_lifeTime = 0.0f;
    //디폴트로 Inactive로 설정
    public EDsetroyType m_destroyType = EDsetroyType.Inactive;
    float m_tempTime;
    bool m_isAlive = false;
    EffectPoolUnit objectPoolUnit;
    //이펙트는 여러개의 파티클로 구성되어있을 확률이 높다.
    ParticleSystem[] m_psystem;
 
    void Awake()
    {
        objectPoolUnit = gameObject.GetComponent<EffectPoolUnit>();
        m_psystem = GetComponentsInChildren<ParticleSystem>();
    }

    private void OnEnable()
    {
        m_isAlive = true;
        m_tempTime = Time.time;
    }

    void DestrtoyParticle()
    {
        switch(m_destroyType)
        {
            case EDsetroyType.Destroy:
                Destroy(gameObject);
                break;
            case EDsetroyType.Inactive:
                if(objectPoolUnit != null)
                {
                    objectPoolUnit.ResetParent();
                }
                gameObject.SetActive(false);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(m_isAlive)
        {
            if(m_lifeTime > 0.0f)
            {
                if(Time.time > m_tempTime + m_lifeTime)
                {
                    m_isAlive = false;
                    DestrtoyParticle();
                }
            }
            else
            {
                bool isPlaying = false;
                for(int i = 0; i < m_psystem.Length; i++)
                {
                    if(m_psystem[i].isPlaying)
                    {
                        isPlaying = true;
                        break;
                    }
                }

                if(!isPlaying)
                {
                    m_isAlive = false;
                    DestrtoyParticle();
                }
            }
        }
    }
}
