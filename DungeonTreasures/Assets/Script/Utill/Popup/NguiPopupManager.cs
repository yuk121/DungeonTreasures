using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NguiPopupManager : DontDestory<NguiPopupManager>
{
    //[SerializeField]
    //GameObject m_PopupOkPrefab;
    //[SerializeField]
    //GameObject m_PopupCancelPrefab;
    //List<GameObject> PopupList = new List<GameObject>();

    //int m_BaseDepth = 100;
    //int m_PanelDepthGap = 10;

    ////확인 버튼이 혹시 할 일이 있다면
    //public delegate void OkBtndelegate();
    //public delegate void CancelBtndelegate();
    
    ////팝업이 떠있는지 확인하는 불린 함수
    //public bool CheckEscape()
    //{
    //    if(PopupList.Count > 0)
    //    {
    //        ClosePopup();
    //        return false;
    //    }
    //    else
    //    {
    //        return true;
    //    }
    //}

    //public void CreatePopupOk(string subject, string body, OkBtndelegate okbtndel ,string PressBtn = "OK")
    //{
    //    var obj = Instantiate(m_PopupOkPrefab) as GameObject;
    //    var panels = obj.GetComponentsInChildren<UIPanel>();

    //    //생성시 깊이 설정
    //    for(int i = 0; i < panels.Length; i++)
    //    {
    //        //팝업창 내에 패널이 많아봐야 10개라고 상정(스크롤뷰등등..)
    //        panels[i].depth = (m_BaseDepth + PopupList.Count * m_PanelDepthGap) + i;   
    //    }
    //    obj.transform.SetParent(transform);
    //    //생성시 UI 초기화를 위해서
    //    var popup = obj.GetComponent<PopupOK>();
    //    popup.SetUI(subject, body, okbtndel, PressBtn);
    //    //생성된 팝업을 리스트에 넣어주기
    //    PopupList.Add(obj);
    //    //생성시 클론이 아니라 이름을 붙여주기 위함
    //    obj.name = string.Format("PopupOK_{0:00}", PopupList.Count);
    //}

    //public void CreatePopupOkCancel(string subject, string body, OkBtndelegate okbtndel, CancelBtndelegate cancelbtndel, string PressOkBtn = "OK", string PressCancelBtn = "Cancel")
    //{
    //    var obj = Instantiate(m_PopupCancelPrefab) as GameObject;
    //    var panels = obj.GetComponentsInChildren<UIPanel>();

    //    //생성시 깊이 설정
    //    for (int i = 0; i < panels.Length; i++)
    //    {
    //        //팝업창 내에 패널이 많아봐야 10개라고 상정(스크롤뷰등등..)
    //        panels[i].depth = (m_BaseDepth + PopupList.Count * m_PanelDepthGap) + i;
    //    }
    //    obj.transform.SetParent(transform);
    //    //생성시 UI 초기화를 위해서
    //    var popup = obj.GetComponent<PopupOKCancel>();
    //    popup.SetUI(subject, body, okbtndel,cancelbtndel,PressOkBtn,PressCancelBtn);
    //    //생성된 팝업을 리스트에 넣어주기
    //    PopupList.Add(obj);
    //    //생성시 클론이 아니라 이름을 붙여주기 위함
    //    obj.name = string.Format("PopupOKCancel_{0:00}", PopupList.Count);
    //}


    //public void ClosePopup()
    //{
    //    //리스트에 팝업이 있다면
    //    if(PopupList.Count > 0)
    //    {
    //        Destroy(PopupList[PopupList.Count - 1]);
    //        PopupList.RemoveAt(PopupList.Count - 1);
    //    }
    //}

    // Start is called before the first frame update
    //protected override void OnStart()
    //{
    //    //동적 로더
    //    m_PopupOkPrefab = Resources.Load("Popup/PopupOK") as GameObject;
    //    m_PopupCancelPrefab = Resources.Load("Popup/PopupOKCancel") as GameObject;
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //    if(Input.GetKeyDown(KeyCode.Space))
    //    {
    //        if (Random.Range(1, 101) % 2 == 0)
    //            CreatePopupOk("오류", "서버에 문제가 생겨 종료합니다.\r\n잠시 후 다시 시작해주세요", null, "확인");
    //        else
    //            CreatePopupOkCancel("[000000]네트워크오류[-]", "네트워크 연결이 끊겼습니다. 다시 연결하시겠습니까?", null, null, "예", "아니오");
    //    }
    //}
}
