using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OKPopup : MonoBehaviour
{
    [SerializeField]
    Text m_noticeText = null;
    [SerializeField]
    Text m_mainText = null;
    [SerializeField]
    Text m_okButtonText = null;

    PopupManager.OKButtonDelgate m_okBtnDelegate;

    public void SetOKPopup(string noticeText, string mainText, PopupManager.OKButtonDelgate okbtnDel, string okButtonText)
    {
        m_noticeText.text = noticeText;
        m_mainText.text = mainText;
        m_okButtonText.text = okButtonText;

        m_okBtnDelegate = okbtnDel;

        var rect = GetComponent<RectTransform>();
        rect.transform.localPosition = Vector3.zero;
        rect.transform.localScale = Vector3.one;
    }

    public void OnPressOKButton()
    {
        // 할 일이 있다면
        if(m_okBtnDelegate != null)
        {
            m_okBtnDelegate();
        }
        else
        {
            PopupManager.Instance.ClosePopup();
        }
    }

}
