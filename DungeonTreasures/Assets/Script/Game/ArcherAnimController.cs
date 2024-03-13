
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAnimController : AnimationController
{
    public enum eAnimState
    {
        Idle,
        DrawArrow,
        AimOverDraw,
        DisArm,
        Equip,
        Die,
        Block,
        Hit,
        Foward,
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
