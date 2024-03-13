using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimController : AnimationController
{
    public enum eAnimState
    {
        Idle,
        run,
        Die,
        Hit,
        Skill_01,
        Skill_02,
    }
    eAnimState m_state;

    public eAnimState GetAniState()
    {
        return m_state;
    }

    public void Play(eAnimState state, bool isFade = true)
    {
        m_state = state;
        Play(m_state.ToString(), isFade);
    }
}
