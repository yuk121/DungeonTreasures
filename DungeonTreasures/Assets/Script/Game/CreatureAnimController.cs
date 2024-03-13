using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureAnimController : AnimationController
{
    public enum eAnimState
    {
        Idle,
        Hit,
        Foward,
        Die,
        Skill_01,
        Skill_02,
        Skill_03
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
