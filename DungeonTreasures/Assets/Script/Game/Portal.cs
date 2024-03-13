using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ePortalType
{
    Enemy = 0,
    Boss,
    Buff
}

public class Portal : MonoBehaviour
{
    [SerializeField]
    GameObject m_portalInfo = null;
    [SerializeField]
    ePortalType m_portalType;

    bool m_isClear;

    public GameObject GetEnemySpwanPoint()
    {
        return m_portalInfo;
    }

    public void SetClear()
    {
        m_isClear = true;
    }

    public bool CheckClerar()
    {
        return m_isClear;
    }

    public void SetPortalType(ePortalType type)
    {
        m_portalType = type;
    }

    public ePortalType GetPortalType()
    {
        return m_portalType;
    }

    private void Start()
    {
        m_isClear = false;
    }
}
