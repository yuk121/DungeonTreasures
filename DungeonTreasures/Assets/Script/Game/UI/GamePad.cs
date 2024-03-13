using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GamePad : SingleTonMonoBehaviour<GamePad> , IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField]
    RectTransform m_padBG = null;
    [SerializeField]
    RectTransform m_padBtn =null;

    Vector2 m_basePos;                      // 조이스틱 처음 위치
    Vector2 m_dir;                               // 조이스틱 방향
    float m_radius;                               // 조이스틱 배경의 반지름
    float m_dist;                                   // 조이스틱 버튼과 배경의 거리 
    private void Awake()
    {
        m_dir = Vector2.zero;

        // 버튼의 위치 구하기.
        m_basePos = m_padBtn.GetComponent<RectTransform>().localPosition;
        m_radius = m_padBG.sizeDelta.x * 0.3f;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        m_dir = Vector2.zero;
        m_padBtn.anchoredPosition = m_basePos;   
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos = Vector2.zero;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(m_padBG, eventData.position, eventData.pressEventCamera, out pos))
        {       
            pos = Vector2.ClampMagnitude(pos, m_radius);
            m_dist = Vector2.Distance(m_basePos, pos) / m_radius;
 
            m_dir = pos.normalized;       
            m_padBtn.anchoredPosition = pos;
        }
    }

    public float GetDist()
    {
        return m_dist;
    }

    public Vector2 GetAxis()
    {
        return m_dir;
    }
}
