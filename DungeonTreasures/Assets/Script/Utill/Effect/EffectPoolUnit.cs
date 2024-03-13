using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EffectPoolUnit : MonoBehaviour
{
    public float m_delay = 1f;  // 트레일이 있는 경우를 대비한 변수
    DateTime m_inactiveTime;  //이펙트가 꺼졌을때의 시간을 가지는 변수
    EffectPool m_objectPool; 
    string m_effectName; // 이펙트 이름을 키값으로 삼기 위한 변수

    //이펙트 초기화
    public void SetObjectPool (string effectName, EffectPool objectPool)
    {
        m_effectName = effectName;
        m_objectPool = objectPool;
        ResetParent();
    }

    //이펙트를 사용할 수있는가를 체크해주는 함수
    public bool IsReady()
    {
        if(!gameObject.activeSelf)
        {
            // 현재시간과 꺼졌을 때의 시간의 차 = 얼마나 지났는가 
            TimeSpan timeSpan = DateTime.Now - m_inactiveTime;

            // 1초 이상 지난 경우에 (트레일 오류방지) 사용 가능
            if (timeSpan.TotalSeconds > m_delay)
                return true;
        }
        return false;
    }

    public void ResetParent()
    {
        transform.SetParent(m_objectPool.transform);
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
    }

    // active가 꺼졌을때 들어오는 이벤트함수
    private void OnDisable()
    {
        m_inactiveTime = DateTime.Now;
        m_objectPool.AddPoolUnit(m_effectName, this);
    }

    private void OnDestroy()
    {
        m_objectPool.RemovePoolUnit(m_effectName, this);
    }

}
