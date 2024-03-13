using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Display : MonoBehaviour
{
    enum eFadeType
    {
        None,
        FadeOut,
        FadeIn
    }

    // 애니메이션 중간에 작업할 메소드를 넣어주는 대리자
    public delegate void CallEvent();
    public CallEvent m_callEvent = null;

    // 애니메이션이 끝나고 나서 작업할 메소드를 넣어주는 대리자.
    public delegate void EndCallEvent();
    public EndCallEvent m_endCallEvent = null;

    bool m_isSwitch = false;

    Animator m_animator;
    Animator m_stage;

    public void SetSwitchPanel()
    {
        gameObject.SetActive(true);
        m_isSwitch = true;
        m_animator.SetTrigger("Switch");
    }

    public void SetDefeatPanel()
    {
        gameObject.SetActive(true);
        m_animator.SetTrigger("Defeat");
    }

    public void SetClearPanel()
    {
        gameObject.SetActive(true);
        m_animator.SetTrigger("Clear");
    }

    public void SetInfomation(string text)
    {
        gameObject.SetActive(true);
        Text infoText = GetComponentInChildren<Text>();
        infoText.text = text;
        m_animator.SetTrigger("Infomation");
    }

    public bool CheckSwitch()
    {
        return m_isSwitch;
    }

    void aEvent_EndCallEvent()
    {
        if (m_endCallEvent != null)
            m_endCallEvent();

        m_isSwitch = false;
        // 혹시 초기화 필요?
        m_endCallEvent = null;
        gameObject.SetActive(false);
    }

    void aEvent_CallEvent()
    {
        if (m_callEvent != null)
            m_callEvent();
    }

    // Start is called before the first frame update
    void Awake()
    {
        m_animator = GetComponent<Animator>();
        gameObject.SetActive(false);
        m_isSwitch = false;
    }

}
