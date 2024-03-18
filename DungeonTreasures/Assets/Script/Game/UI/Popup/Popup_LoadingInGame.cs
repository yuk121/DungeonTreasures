using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup_LoadingInGame : MonoBehaviour
{
    [SerializeField]
    Image m_titleImg = null;
    [SerializeField]
    RectTransform m_titleRect = null;
    [SerializeField]
    Vector3 m_startPos = Vector3.zero;
    [SerializeField]
    Vector3 m_endPos = Vector3.zero;
    [SerializeField]
    float m_fadeTime = 0f;

    Coroutine m_corLoadingTitleEffect;

    public void SetLoadingEffect(bool loadStart , Action<bool> callback)
    {
        if(m_corLoadingTitleEffect != null)
        {
            StopCoroutine(m_corLoadingTitleEffect);
        }

        m_corLoadingTitleEffect = StartCoroutine(CorLoadingTitleEffect(loadStart, callback)); 
    }

    public IEnumerator CorLoadingTitleEffect(bool fadeIn, Action<bool> callback)
    {
        bool effectEnd = false;
        float time = 0f;

        // 페이드 인 && 타이틀 내려옴
        if(fadeIn == true)
        {
            m_titleRect.anchoredPosition = m_startPos;
           
            float alpha = 0f;
            m_titleImg.color = new Color(m_titleImg.color.r, m_titleImg.color.g, m_titleImg.color.b, alpha);

            while (true)
            {
                time += Time.deltaTime;

                m_titleRect.anchoredPosition = new Vector3(m_startPos.x, Mathf.Lerp(m_startPos.y, m_endPos.y , time / m_fadeTime) , m_startPos.z);
                alpha = Mathf.Lerp(0, 1, time / m_fadeTime);
                m_titleImg.color = new Color(m_titleImg.color.r, m_titleImg.color.g, m_titleImg.color.b, alpha);

                // 연출 끝
                if(m_titleRect.anchoredPosition.y >= m_endPos.y && alpha >= 1)
                {
                    effectEnd = true;
                    break;
                }

                yield return new WaitForEndOfFrame();
            }
        }
        else // 페이드 아웃 && 타이틀 올라감
        {
            m_titleRect.anchoredPosition = m_endPos;
            
            float alpha = 1f;
            m_titleImg.color = new Color(m_titleImg.color.r, m_titleImg.color.g, m_titleImg.color.b, alpha);
           
            while (true)
            {
                time += Time.deltaTime;

                m_titleRect.anchoredPosition = new Vector3(m_endPos.x, Mathf.Lerp(m_endPos.y, m_startPos.y, time / m_fadeTime), m_endPos.z);
                alpha = Mathf.Lerp(0, 1, time / m_fadeTime);
                m_titleImg.color = new Color(m_titleImg.color.r, m_titleImg.color.g, m_titleImg.color.b, alpha);

                // 연출 끝
                if (m_titleRect.anchoredPosition.y <= m_startPos.y && alpha <= 0)
                {
                    effectEnd = true;
                    break;
                }

                yield return new WaitForEndOfFrame();
            }
        }

        callback.Invoke(effectEnd);
    }
}
