using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleAnimationController : MonoBehaviour
{
    [SerializeField]
    GameObject m_touchStart = null;
    public GameObject TouchStart {get { return m_touchStart; } }

    [SerializeField]
    float m_playTime = 1f;

   
    bool m_isLogoFadeEnd;
    public bool isLogoFadeEnd {  get { return m_isLogoFadeEnd; } }
   
    private Image m_titleImg = null;
    private Coroutine m_corLogoFade;

    public void ShowLogo()
    {
        if(m_corLogoFade != null)
        {
            StopCoroutine(m_corLogoFade);
        }

        m_corLogoFade = StartCoroutine(CorLogoFade());
    }

    private IEnumerator CorLogoFade()
    {
        float alpha = 0;
        float time = 0;

        while (alpha < 1)
        {
            time += Time.deltaTime;
            alpha = Mathf.Lerp(0, 1, time / m_playTime );
            m_titleImg.color = new Color(m_titleImg.color.r, m_titleImg.color.g, m_titleImg.color.b, alpha);
            yield return null;
        }

        m_isLogoFadeEnd = true;
    }

    public void ShowStartToTuch()
    {
        m_touchStart.SetActive(true);
    }

    private void Awake()
    {
        m_isLogoFadeEnd = false;
        m_titleImg = GetComponent<Image>();
        m_titleImg.color = new Color(m_titleImg.color.r, m_titleImg.color.g, m_titleImg.color.b, 0f);
    }
    
}
