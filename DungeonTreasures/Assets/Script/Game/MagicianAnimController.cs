using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicianAnimController : AnimationController
{
    public enum eAnimState
    {
        Idle,
        Skill_01,
        Skill_02,
        Skill_03,
        Block,
        Die,
        Foward,
        Hit
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
