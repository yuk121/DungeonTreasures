using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectUnit : MonoBehaviour
{
    ParticleSystem m_particle;

    public void Play()
    {
        m_particle.Play();
    }
    // Start is called before the first frame update
    void Awake()
    {
        m_particle = GetComponent<ParticleSystem>();
        gameObject.SetActive(false);
    }

}
