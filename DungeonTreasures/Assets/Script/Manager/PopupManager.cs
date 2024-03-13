using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : DontDestory<PopupManager>
{
    [SerializeField]
    GameObject m_okPopupObj = null;
    [SerializeField]
    GameObject m_okCancelPopup = null;

    GameObject m_canvas = null;
    List<GameObject> m_popupList = new List<GameObject>();

    int m_backCount = 0;

    public delegate void OKButtonDelgate();
    public delegate void CancelButtonDelegae();

    public void CreateOKPopup(string notice, string main, OKButtonDelgate okbtndel, string okbtn = "확인")
    {
        var obj = Instantiate(m_okPopupObj) as GameObject;
        obj.transform.SetParent(m_canvas.transform);

        var okPopup = obj.GetComponent<OKPopup>();

        okPopup.SetOKPopup(notice, main, okbtndel, okbtn);
        m_popupList.Add(obj);
        Debug.Log(m_popupList.Count);
        obj.name = string.Format("OKPopup {0}", m_popupList.Count);
    }

    public void CreateOKCancelPopup(string notice, string subject, string main, OKButtonDelgate okbtndel, CancelButtonDelegae cancelbtndel, string okbtn = "확인", string cancelbtn = "취소")
    {
        var obj = Instantiate(m_okCancelPopup) as GameObject;

        if(m_canvas != null)
        {
             obj.transform.SetParent(m_canvas.transform);
        }
        var okCancelPopup = obj.GetComponent<OKCancelPopup>();

        okCancelPopup.SetOKCancelPopup(notice, main, okbtndel, cancelbtndel, okbtn, cancelbtn);
        m_popupList.Add(obj);
        obj.name = string.Format("OKCancelPopup {0}", m_popupList.Count);
    }

    public void ClosePopup()
    {
        if (m_popupList.Count > 0)
        {
            Debug.Log("#로그 ClosePopup 메소드 들어옴");
            Destroy(m_popupList[m_popupList.Count - 1]);
            m_popupList.RemoveAt(m_popupList.Count - 1);
        }
    }

    public void SetPopupManager()
    {
        if (GameObject.Find("UI Canvas") != null)
        {
            m_canvas = GameObject.Find("UI Canvas") as GameObject;
        }
        else if (GameObject.Find("Canvas") != null)
        {
            m_canvas = GameObject.Find("Canvas") as GameObject;
        }
    }

    void GameQuit()
    {

    }

    
    protected override void OnStart()
    {
        SetPopupManager();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            m_backCount++;

            if(m_backCount > 1)
            {
                ClosePopup();
            }

            CreateOKCancelPopup("Notice", "확인", "게임을 종료하시겠습니까?", () =>
            {
#if (UNITY_EDITOR)
                UnityEditor.EditorApplication.isPlaying = false;
#endif

#if (UNITY_ANDROID || UNITY_IPHONE)
                Application.Quit();
#endif
            },
            () =>
            {
                ClosePopup();
            }
            , "예", "아니오");
        }
    }

}
