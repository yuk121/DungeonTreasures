using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    protected Animator m_animator;
    string m_prevState;

    public void Pause()
    {
        m_animator.enabled = false;
    }
    public void Resume()
    {
        m_animator.enabled = true;
    }

    public void SetFloat(string parameter, float value)
    {
        m_animator.SetFloat(parameter, value);
    }

    public void Play(string state, bool isFade = true)
    {
        //이전에 남아있는 스테이트가 있다면 비워준다.
        if (!string.IsNullOrEmpty(m_prevState))
        {
            m_animator.ResetTrigger(m_prevState);
            m_prevState = string.Empty;
        }
        if (isFade) //블렌드를 넣는 경우
        {
            m_animator.SetTrigger(state.ToString());
        }
        else
        {
            m_animator.Play(state.ToString(), 0, 0.0f);
        }
        m_prevState = state;
    }
    // Start is called before the first frame update
    void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_prevState = string.Empty;
    }
}
