using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TweenMove : MonoBehaviour
{
    public enum eMoveType
    {
        Normal,
        RigidBody,
        CharacterController,
        NavMeshAgent
    }

    public delegate void OnFinish();
    public OnFinish m_onFinish = null;
    public delegate void PlayAnimation();
    public PlayAnimation m_playAnimation = null;

    public eMoveType m_moveType;
    [SerializeField]
    AnimationCurve m_aniCurve = new AnimationCurve();

    [SerializeField]
    Vector3 m_from;
    [SerializeField]
    Vector3 m_to;
    [SerializeField]
    float m_duration;
    float m_checkTime;
    float m_endTime;

    [SerializeField]
    bool m_isStart = false;
    CharacterController m_characterController;
    NavMeshAgent m_navMeshAgent;


    public void MoveToTween(Vector3 from, Vector3 to, float duration)
    {
        if(m_isStart)
        {
            if(m_onFinish != null)
            {
                m_onFinish();
            }
        }

        m_isStart = true;
        m_from = from;
        m_to = to;
        m_duration = duration;
        m_checkTime = 0f;
    }

    public void ResetTween()
    {
        m_isStart = false;
        m_checkTime = 0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        switch (m_moveType)
        {
            case eMoveType.Normal:
                break;
            case eMoveType.RigidBody:
                break;
            case eMoveType.CharacterController:
                m_characterController = GetComponent<CharacterController>();
                break;
            case eMoveType.NavMeshAgent:
                m_navMeshAgent = GetComponent<NavMeshAgent>();
                break;
        }

        m_endTime = m_aniCurve.keys[m_aniCurve.keys.Length - 1].time;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(m_isStart)
        {
            m_checkTime += Time.deltaTime / m_duration;

            var value = m_aniCurve.Evaluate(m_checkTime);
            var dir = Vector3.Lerp(m_from, m_to,value);

            switch (m_moveType)
            {
                case eMoveType.Normal:
                    transform.position = dir;
                    break;
                case eMoveType.RigidBody:
                    break;
                case eMoveType.CharacterController:
                    if (m_playAnimation != null)
                        m_playAnimation();
                    dir = dir - transform.position;
                    m_characterController.Move(dir);

                    break;
                case eMoveType.NavMeshAgent:              
                    if (m_playAnimation != null)
                        m_playAnimation();
                    dir = dir - transform.position;
                    m_navMeshAgent.Move(dir);

                    break;
            }

            if(m_checkTime  >= m_endTime)
            {
                m_isStart = false;

                if (!m_isStart)
                {
                    if (m_onFinish != null)
                    {
                        m_onFinish();
                    }
                }
            }

        }
    }
}
