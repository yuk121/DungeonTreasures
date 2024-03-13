using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OKCancelPopup : MonoBehaviour
{
    [SerializeField]
    Text m_noticeText = null;
    [SerializeField]
    Text m_subjectText = null;
    [SerializeField]
    Text m_mainText = null;
    [SerializeField]
    Text m_okButtonText = null;
    [SerializeField]
    Text m_cancelButtonText = null;

    PopupManager.OKButtonDelgate m_okBtnDelegate;
    PopupManager.CancelButtonDelegae m_cancelBtnDelegate;

    public void SetOKCancelPopup(string noticeText, string subjectText, string mainText, PopupManager.OKButtonDelgate okbtnDel, PopupManager.CancelButtonDelegae cancelBtnDel, string okButtonText, string cancelButtonText)
    {
        m_noticeText.text = noticeText;
        m_subjectText.text = subjectText;
        m_mainText.text = mainText;
        m_okButtonText.text = okButtonText;
        m_cancelButtonText.text = cancelButtonText;

        m_okBtnDelegate = okbtnDel;
        m_cancelBtnDelegate = cancelBtnDel;

        var rect = GetComponent<RectTransform>();
        rect.transform.localPosition = Vector3.zero;
        rect.transform.localScale = Vector3.one;
    }

    public void OnPressCancelButton()
    {
        // 할 일이 있다면
        if (m_cancelBtnDelegate != null)
        {
            m_cancelBtnDelegate();
        }
        else
        {
            PopupManager.Instance.ClosePopup();
        }
    }

    public void OnPressOKButton()
    {
        // 할 일이 있다면
        if (m_okBtnDelegate != null)
        {
            m_okBtnDelegate();
        }
        else
        {
            PopupManager.Instance.ClosePopup();
        }
    }
}
